using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponText : MonoBehaviour
{

    private int bulletNum = 0;
    private int unsetBulletNum = 0;

    Text text = null;

    // Start is called before the first frame update
    void Start()
    {

        //テキストの取得
        if (TryGetComponent(out text)==false)//存在しない場合
        {
            Debug.LogError("Textが見つかりません");//エラーメッセージ
        }

        //テキストの内容変更
        text.text = bulletNum + " / " + unsetBulletNum;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetBullet(int bullet, int unsetBullet)
    {
        bulletNum = bullet;
        unsetBulletNum = unsetBullet;


        if (TryGetComponent(out text) == false)
            Debug.LogError("Textが見つかりません");
        else
            text.text = bulletNum + "/" + unsetBulletNum;

    }

}