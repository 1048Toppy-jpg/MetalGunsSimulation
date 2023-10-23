using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class EnemyController : BaseController
{

    //名前
    [SerializeField] private string enemyTag = "Drone";

    //パーティクル
    [SerializeField] GameObject hitParticleObject = null;       //爆発
    [SerializeField] GameObject destroyParticleObject = null;   //大爆発
    [SerializeField] GameObject smokeParticleObject = null;     //煙
    [SerializeField] private GameObject modelObject = null;     //敵本体

    //SE
    [SerializeField] private AudioClip hitSE = null;
    [SerializeField] private AudioClip destroySE = null;
    private AudioSource audioSource = null;

    //HP関連
    private EnemyHPBar hpBar = null;
    [SerializeField] private int baseHp = 30;
    private int hp;

    //アニメーション(Animator)
    private Animator animator = null;

    //武器関連
    private EnemyShoot enemyShoot = null;//敵の攻撃情報

    //横揺れのアニメーション
    private bool swayFlg = false;   //揺れているかどうか
    [SerializeField] private int swayDefaultInterval = 60;//揺れる間隔をセット(デフォルト値)
    private int swayInterval;//計算用


    //煙
    [SerializeField] private int smokeDefaultInterval = 20;//煙の間隔(デフォルト値)
    [SerializeField] private float smokeKillHeigth = 2.0f;//煙が消える高度(低さ)　　※この値より低いと消える
    private int smokeInterval = 0;//計算用

    //破壊フラグ
    private bool crashFlg = false;//壊れているかどうか
    private float modelDefaultHeight = 0.0f;//リセット用にmodelのY座標情報を確保しておく

    public bool CrashFlg
    {
        get { return this.crashFlg; }//取得用
        private set { this.crashFlg = value; } //値入力用
    }

    //Warrior用
    private Chace2 chace;


    [SerializeField] private int explosionWaitDefaultTime = 60;//破壊してから爆破するまでの時間
    private int explosionWaitTime;

    public float CrashHeight = 2.0f;


    private GameObject playerObj = null;


    [SerializeField] private float activeLength = 100.0f;
    private NavMeshAgent agent = null;


    public override void Restart()
    {

        positionReset.Execute();//座標をリセットする
        gameObject.SetActive(true);

        //Y座標の設定
        var newPos = modelObject.transform.position;//model座標の取得
        newPos.y = modelDefaultHeight;//Y座標の変更
        modelObject.transform.position = newPos;//変更した座標に更新する


        //HP関連

        hp = baseHp;//体力リセット

        if (TryGetComponent(out hpBar) == false)
            Debug.Log("HPバーがありません");
        else
            //体力バーのリセット
            hpBar.SetBaseHP(baseHp);


        if (animator == null)
        {
            if (gameObject.TryGetComponent(out animator) == false)
            {
                Debug.LogError("アニメーターが見つかりませんでした");
            }
            else
                animator.SetTrigger("Reset");
        }
        

        if (enemyShoot != null)
            enemyShoot.gameObject.SetActive(true);


        swayFlg = false;


        smokeInterval = smokeDefaultInterval;

        swayInterval = swayDefaultInterval;

        crashFlg = false;

        //爆発するまでの時間のセット
        explosionWaitTime = explosionWaitDefaultTime;

        //Chase2を取得
        if (gameObject.TryGetComponent(out chace) == false)
            Debug.LogError("Chaceが見つかりませんでした");
        else
        {
            chace.tracking = false;
            chace.SetcanShoot = true;
            chace.Restart();
        }


    }

    public override void Init()
    {


        //SE
        if (GameObject.Find("AudioManager").TryGetComponent(out audioSource) == false)
        {
            Debug.LogError("AudioManagerが見つかりませんでした。");
            audioSource = null;
        }

        //HP
        if (TryGetComponent(out hpBar) == false)
        {
            Debug.Log("HPバーがありません");
            hpBar = GetComponent<EnemyHPBar>();
        }
        hp = baseHp;
        hpBar.SetBaseHP(baseHp);


        //アニメーション
        if (TryGetComponent(out animator) == false)
        {
            Debug.LogError("アニメーターが見つかりませんでした");
        }


        //武器
        var shootObj = GameObject.Find("ShootObj");//オブジェクトを取得
        if (shootObj == null) Debug.LogError("ShootObj");//存在しない場合、エラーメッセージ


        if (shootObj.TryGetComponent(out enemyShoot) == false)//取得
        {//存在しない
            Debug.LogError("ShotObjが見つかりませんでした。");//エラーメッセージ
        }

        //煙のintervalをセット
        smokeInterval = smokeDefaultInterval;

        //揺れるアニメ―ション
        swayInterval = swayDefaultInterval;

        //モデルのY座標のデフォルト値を記憶
        modelDefaultHeight = modelObject.transform.position.y;

        //Chase2を取得
        if (gameObject.TryGetComponent(out chace) == false)
            Debug.LogError("Chaceが見つかりませんでした");
        else
            chace.SetcanShoot = true;
        //爆発するまでの時間のセット
        explosionWaitTime = explosionWaitDefaultTime;

        playerObj = GameObject.Find("Player");
        if (playerObj == null)
            Debug.LogError("プレイヤーが見つかりません");

        if (gameObject.TryGetComponent(out agent) == false)
            Debug.LogError("NavMeshAgentが見つかりません");
    }

    public void HitBullet(int damage, Vector3 damagePos)
    {
        //*******************************
        //Drone・Warrior　共通
        //*******************************

       if(!crashFlg)//まだ壊れていない場合
            if (hitSE && audioSource) audioSource.PlayOneShot(hitSE);

        //HPの減少処理
        hp -= damage;

        if (hpBar)
        {
            hpBar.DamageHP(damage);
        }
        if (hp <= 0)//破壊された場合
        {
            //GameOver();
            if (!crashFlg)//破壊された直後の場合
            {


                crashFlg = true;

                //アニメーター
                animator.SetTrigger("Destroy");
                

                if (agent.enabled == true)//追尾を辞める
                {
                    agent.enabled = false;
                }
                if (chace.enabled == true)
                {
                    chace.enabled = false;
                }

                //攻撃をやめる
                if (enemyShoot)
                    enemyShoot.gameObject.SetActive(false);
                else
                    Debug.LogError("enemyShootが見つかりませんでした");

                //デバッグログ
                Debug.Log(this.name + " = 破壊");

                //SE
                if (destroySE && audioSource) audioSource.PlayOneShot(destroySE);

                //パーティクル
                var deathCloneParticle = Instantiate(destroyParticleObject);
                deathCloneParticle.transform.position = modelObject.transform.position;
            }
        }
        else
        {
            if (swayFlg == false)
            {

                if (enemyTag == "Drone")//Drone専用
                {
                    int random = Random.Range((int)1, (int)3);
                    if (random == 1)
                        animator.SetTrigger("SwayRight");
                    else
                        animator.SetTrigger("SwayLeft");
                }
                else if (enemyTag == "Warrior")//Warrior専用
                {
                    //Chase2を取得
                    if (gameObject.TryGetComponent(out chace) == false)
                        Debug.LogError("Chaceが見つかりませんでした");
                    chace.Stop();
                    animator.SetTrigger("Damage");

                }
                swayFlg = true;

            }

            //パーティクル
            var cloneParticle = Instantiate(hitParticleObject);
            cloneParticle.transform.position = damagePos;

        }

    }

    private void FixedUpdate()
    {
        //プレイヤーがいない場合通らない
        if (playerObj == null)
        {
            //NavMeshAgent・Chaceの停止
            if (agent.enabled)
                agent.enabled = false;
            if (chace.enabled)
                chace.enabled = false;
            return;
        }

        var enemyPos = gameObject.transform.position;
        var playerPos = playerObj.transform.position;
        var playerLength = (enemyPos - playerPos).magnitude;

        //動ける範囲にいない場合通らない
        if (activeLength < playerLength)
        {
            //NavMeshAgent・Chaceの停止
            if (agent.enabled)
                agent.enabled = false;
            if (chace.enabled)
                chace.enabled = false;
            return;
        }
        //NavMeshAgent・Chaceの起動
        if (!agent.enabled && !crashFlg)
            agent.enabled = true;
        if (!chace.enabled && !crashFlg)
            chace.enabled = true;



        if (hp < (baseHp / 2))//体力が半分以下に減ったら
        {
            //モデルから地面までのベクトルを取得
            var lengthVec =
                modelObject.transform.position -    //モデルの座標から
                gameObject.transform.position;      //地面までの座標を引く

            float length = lengthVec.magnitude;

            if (length > smokeKillHeigth)//指定した高さより高かったら
            {
                //煙の間隔処理
                smokeInterval--;
                if (smokeInterval <= 0)//0になったら
                {
                    if (smokeParticleObject != null)//煙情報が存在する場合
                    {
                        //クローンを生成
                        var smokeCloneParticle = Instantiate(smokeParticleObject);
                        smokeCloneParticle.transform.position = modelObject.transform.position;//座標を合わせる
                    }
                    //カウントをリセットする
                    smokeInterval = smokeDefaultInterval;
                }
            }
        }

        if (swayFlg)//Swayのアニメーションが行われている場合
        {
            //カウントを減らす
            swayInterval--;
            if (swayInterval <= 0)//0になった場合
            {
                //フラグをfalseにする
                swayFlg = false;
                //カウントをリセットする
                swayInterval = swayDefaultInterval;

                if (enemyTag == "Warrior")//Warrior専用
                {
                    //Chase2を取得
                    if (gameObject.TryGetComponent(out chace) == false)
                        Debug.LogError("Chaceが見つかりませんでした");
                    chace.Restart();
                }
            }
        }


        if (crashFlg)//破壊された場合
        {
            if (enemyTag == "Drone")//Drone専用
            {
                //墜落する
                modelObject.transform.position += new Vector3(0.0f, -0.1f, 0.0f);

                if (modelObject.transform.position.y < gameObject.transform.position.y + CrashHeight)
                {

                    //Y座標の設定
                    //var newPos = modelObject.transform.position;//model座標の取得
                    //newPos.y = gameObject.transform.position.y;//Y座標の変更
                    //modelObject.transform.position = newPos;//変更した座標に更新する

                    GameOver();
                    //SE
                    if (destroySE && audioSource) audioSource.PlayOneShot(destroySE);

                    //パーティクル
                    var deathCloneParticle = Instantiate(destroyParticleObject);
                    deathCloneParticle.transform.position = modelObject.transform.position;
                }
            }
            else if (enemyTag == "Warrior")//Warrior専用
            {
                //Chase2を取得
                if (gameObject.TryGetComponent(out chace) == false)
                    Debug.LogError("Chaceが見つかりませんでした");
                chace.Stop();

                explosionWaitTime--;
                if (explosionWaitTime <= 0)
                {
                    GameOver();
                    //SE
                    if (destroySE && audioSource) audioSource.PlayOneShot(destroySE);

                    //パーティクル
                    var deathCloneParticle = Instantiate(destroyParticleObject);
                    deathCloneParticle.transform.position = modelObject.transform.position;
                }
            }
            chace.SetcanShoot = false;
        }
    }
}