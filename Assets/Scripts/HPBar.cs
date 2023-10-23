using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{

    [SerializeField]private int defaultMaxHp = 100;
    private int currentHp;

    [SerializeField] int knockBacknum = 10;
    [SerializeField] bool normalize = false;


    //Sliderを入れる
    [SerializeField]private Slider slider;

    //RigidBody
    private Rigidbody rigidBody = null;

    //SE
    [SerializeField] private AudioClip healSE = null;

    private AudioSource audioSource = null;

    // Start is called before the first frame update
    void Start()
    {
        //Sliderを満タンにする。
        slider.value = 1;
        
        //現在のHPを最大HPと同じに。
        currentHp = defaultMaxHp;
        
        slider.value = (float)currentHp / (float)defaultMaxHp; 

        //Debug.Log("Start currentHp :" + currentHp);


        //rigidBodyの取得
        if (gameObject.TryGetComponent(out rigidBody) == false)
        {
            Debug.LogError("RigidBodyが見つかりません");
        }

        //AudioSourceの取得
        if (GameObject.Find("AudioManager").TryGetComponent(out audioSource) == false)
        {
            Debug.LogError("AudioManagerが見つかりませんでした。");
            audioSource = null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))//Qキーを押した場合
        {
            //UIのオブジェクトを取得
            var CureBoxUI = GameObject.Find("CureBoxUI");

            if (CureBoxUI == null)//存在しない場合
                Debug.LogError("CureBoxUIが見つかりませんでした");//エラーメッセージ
            else//存在する場合
            {
                //子オブジェクトの中からTextオブジェクトを取得する
                foreach (Transform child in CureBoxUI.transform)
                {
                    if (child.name == "Text")//テキストオブジェクトがあった場合
                    {
                        //回復アイテムのUI情報を取得
                        CureBoxText cureBoxText = null;

                        //存在しない場合
                        if (child.gameObject.TryGetComponent(out cureBoxText) == false)
                            Debug.LogError("CureBoxTextが見つかりませんでした");
                        else//存在する場合
                        {       
                            //回復処理
                            Health health = null;
                            if (gameObject.TryGetComponent(out health) == false)
                                Debug.LogError("healthが見つかりませんでした");

                            if (!health.JudgeMaxHealth())//HPがマックスじゃない場合
                            {
                                if (cureBoxText.UseCureBox())//使用された場合
                                {
                                    health.ChangeHealth(cureBoxText.GetHealNum());
                                    //SE
                                    if (audioSource && healSE)
                                        audioSource.PlayOneShot(healSE);
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }
    }
   
    //ColliderオブジェクトのIsTriggerにチェック入れること。
    private void OnTriggerStay(Collider collider)
    {
        //Enemyタグのオブジェクトに触れると発動
        if (collider.gameObject.tag == "Enemy")
        {
            //ダメージは1～100の中でランダムに決める。
            int damage = Random.Range(1, 100);
            Debug.Log("damage : " + damage);

            //現在のHPからダメージを引く
            currentHp = currentHp - damage;
            Debug.Log("After currentHp : " + currentHp);

            //最大HPにおける現在のHPをSliderに反映。
            //int同士の割り算は小数点以下は0になるので、
            //(float)をつけてfloatの変数として振舞わせる。
            slider.value = (float)currentHp / (float)defaultMaxHp; 
            Debug.Log("slider.value : " + slider.value);
        }
    }

    public void ChangeHP(int addNum)
    {
        currentHp += addNum;

        if (currentHp <= 0)//HPが0を下回った場合
        {
            currentHp = 0;//値を0にする
        }

        if (currentHp > defaultMaxHp)//Max値を超えた場合
        {
            currentHp = defaultMaxHp;//Max値に合わせる
        }

        //UIを更新する
        slider.value = (float)currentHp / (float)defaultMaxHp;

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.tag == "Collider")
        {
            var parentObj = collision.transform.parent;
            if (parentObj.tag == "Enemy")
            {
                Vector3 KnockBackVec = Vector3.zero;//ノックバックするベクトル
                Vector3 PlayerPos = gameObject.transform.position;  //プレイヤーの座標
                Vector3 EnemyPos = parentObj.transform.position;    //敵の座標
                KnockBackVec = PlayerPos - EnemyPos;//敵の反対方向に飛ばす
                KnockBackVec = KnockBackVec.normalized;//正規化

                rigidBody.AddForce(KnockBackVec * 10.0f, ForceMode.VelocityChange);
                Debug.Log("ノックバックしました");
            }
        }

    }
}
