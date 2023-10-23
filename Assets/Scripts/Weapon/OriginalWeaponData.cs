using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginalWeaponData : BaseController
{

    //武器のシルエット画像
    [SerializeField] private Sprite sprite = null;

    [SerializeField] private int power = 10;

    //残弾数
    [SerializeField] private int unsetBulletNum = 40;//装填前の弾の数
    [SerializeField] private int setBulletNum = 4;//装填した弾の数

    private int defaultUnsetBulletNum;//リセットした際に呼び出す装填前の弾の数
    private int defaultSetBulletNum;//リセットした際に呼び出す装填した弾の数

    //UI関連
    private WeaponImage weaponImage = null;
    private WeaponText weaponText = null;

    //Audio
    [SerializeField] private AudioClip getBulletSE = null;

    private AudioSource audioSource = null;


    public int GetPower() { return power; }

    public int GetBulletNum()
    {
        int Out = setBulletNum;
        return Out;
    }
    public int GetUnsetBulletNum()
    {
        int Out = unsetBulletNum;
        return Out;
    }

    public override void Restart()
    {
        unsetBulletNum = defaultUnsetBulletNum;
        setBulletNum = defaultSetBulletNum;
    }

    public override void Init()
    {
        //弾のデフォルト値をセット
        defaultUnsetBulletNum = unsetBulletNum;
        defaultSetBulletNum = setBulletNum;

        //UIのデータを取得
        GetUIData();


        //AudioSourceの取得
        //if (GameObject.Find("AudioManager").TryGetComponent(out audioSource) == false)
        if (gameObject.TryGetComponent(out audioSource) == false)
        {

            Debug.LogError("AudioManagerが見つかりませんでした");
            audioSource = null;
        }

    }

    public bool CanFire()
    {
        if (setBulletNum <= 0)
        {
            return false;
        }
        return true;
    }

    public void UseBulletOnce()
    {
        if (setBulletNum > 0)
        {
            setBulletNum--;
        }

    }

    public bool Reload()//リロード
    {
        //リロードする数を計算する
        int setNum = 0;

        //現在必要な弾の数 = 弾が入るマックス数 - 現在弾が入っている数
        setNum = defaultSetBulletNum - setBulletNum;

        //必要な弾の数が0の場合 falseを返す
        if (setNum == 0) return false;

        //必要数分リロードできる場合
        if (unsetBulletNum >= setNum) //替えの弾 >= 必要数分
        {
            //Maxになる弾分入れる
            setBulletNum = defaultSetBulletNum;

            //替えの弾を必要数分減らす
            unsetBulletNum -= setNum;
        }
        else//必要数分リロードができない場合
        {
            //弾がない場合 falseを返す
            if (unsetBulletNum == 0)
                return false;
            else//弾がある場合
            {
                //替えの弾すべて、リロードする
                setBulletNum += unsetBulletNum;

                //替えの弾を0にする
                unsetBulletNum = 0;
            }
        }
        UpdateUI();
        return true;
    }

    public void UpdateUI()
    {
        //UIのデータがどちらか欠けていたら
        if (!weaponText || !weaponImage)
        {
            GetUIData();//データを取得
        }
        weaponText.SetBullet(setBulletNum, unsetBulletNum);//弾数を更新
        weaponImage.SetImage(sprite);//画像を更新

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            unsetBulletNum += 10;
            UpdateUI();
        }

    }

    public Sprite GetSprite() { return sprite; }
    // Start is called before the first frame update

    public void AddWeaponBullet(int num)
    {
        //弾の数を追加する
        unsetBulletNum += num;

        //UIを更新する
        if (gameObject.activeSelf)
            UpdateUI();

        //SEがあれば再生する
        if (audioSource != null &&
            getBulletSE != null) 
            audioSource.PlayOneShot(getBulletSE); 
    }

    private void GetUIData()
    {
        //UIのオブジェクトを取得する
        var weaponUIObj = GameObject.Find("WeaponUI");
        if (weaponUIObj)//ある場合
        {
            foreach (Transform child in weaponUIObj.transform)//子オブジェクトから検索する
            {
                //Textという名前のオブジェクトが見つかった場合
                if (child.gameObject.name == "Text")
                {
                    if (child.gameObject.TryGetComponent(out weaponText) == false)//取得
                    {
                        //WeaponTextコンポーネントがない場合
                        Debug.LogError("weaponTextが見つかりませんでした"); //エラーテキスト
                    }
                }
                //Imageという名前のオブジェクトが見つかった場合
                else if (child.gameObject.name == "Image")
                {
                    if (child.gameObject.TryGetComponent(out weaponImage) == false)//取得
                    {
                        //WeaponImageコンポーネントがない場合
                        Debug.LogError("weaponImageが見つかりませんでした"); //エラーテキスト
                    }
                }
                //両方取得した場合、ループから抜ける
                if (weaponImage != null &&
                    weaponText != null) break;
            }
        }
        else//ない場合
        {
            Debug.LogError("WeaponUIのオブジェクトが存在しません");
        }
    }
}