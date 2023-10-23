using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCursor : MonoBehaviour
{
    //RectTransformを取得
    private RectTransform rcTrans = null;

    //元のX座標の取得用
    private float defaultPosX = 0.0f;

    //移動量を設定
    [SerializeField] private float moveValue = 0.0f;


    [SerializeField] private float upY;
    [SerializeField] private float downY;

    private float selectTime = 0.0f;

    [SerializeField] private Blink blink = null;

    [SerializeField] private RectTransform rcUpButton = null;
    [SerializeField] private RectTransform rcDownButton = null;


    private bool onceAlphaClear = false;
    // Start is called before the first frame update
    void Start()
    {

        //RectTransformを取得
        if (gameObject.TryGetComponent(out rcTrans) == false)//見つからない場合
        {
            Debug.LogError("RectTransformが見つかりませんでした。");
        }
        else//見つかった場合
        {
            //X軸を取得
            defaultPosX = rcTrans.position.x;
        }

        onceAlphaClear = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Up()
    {
        var tempPos = rcTrans.position;//座標を取得
        //tempPos.y = upY;          //Y座標だけ変更する
        tempPos.y = rcUpButton.position.y;          //Y座標だけ変更する
        rcTrans.position = tempPos;     //元のデータに入れる
        blink.AlphaClear();             //一度カーソルの点滅を初期化する
    }
    public void Down()
    {
        var tempPos = rcTrans.position; //座標を取得
        //tempPos.y = downY;         //Y座標だけ変更する   
        tempPos.y = rcDownButton.position.y;         //Y座標だけ変更する   
        rcTrans.position = tempPos;      //元のデータに入れる
        blink.AlphaClear();              //一度カーソルの点滅を初期化する
    }

    public void SelectButton()
    {

        blink.speed = 5.0f;     //点滅速度を上げる
        selectTime += 1.0f;     //タイマーを動かす
        if (selectTime > 120.0f) //一秒経過したら
        {
            if (!onceAlphaClear)
            {
                onceAlphaClear = true;
                blink.AlphaClear();    //点滅を消す
            }
            blink.FadeOut();    //点滅を消す
        }
    }
}