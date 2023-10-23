using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CureBoxText : MonoBehaviour
{
    //起動時にデフォルトで持っている数
    [SerializeField] private int defaultHaveNum = 0;
    //現在持っている数
    private int haveingNum = 0;

    //持っている回復アイテムの回復量
    private int healingNum = 20;

    //数を表示するテキストデータ
    Text text = null;

    // Start is called before the first frame update
    void Start()
    {
        //持っている数を設定した値に変更する
        haveingNum = defaultHaveNum;

        //テキストの取得
        if (TryGetComponent(out text) == false)//存在しない場合
        {
            Debug.LogError("Textが見つかりません");//エラーメッセージ
        }
        //テキストの内容変更
        text.text = "" + haveingNum;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddCureBox(int getNum,int healNum)//回復アイテムの追加
    {
        //持っている値の変更
        haveingNum += getNum;

        //回復量の変更
        healingNum = healNum;

        //再度Textデータを取得
        if (TryGetComponent(out text) == false)//存在しない場合
            Debug.LogError("Textが見つかりません");//エラーメッセージ
        else //存在する場合
            text.text = "" + haveingNum;//Text内容を更新
    }

    public bool UseCureBox()
    {
        //回復アイテムを一個以上持っていた場合
        if (0 < haveingNum)
        {
            haveingNum -= 1;//一個消費する
            text.text = "" + haveingNum;//Text内容を更新

            return true;//消費ができたらtrueを返す
        }
        return false;//できなかったらfalseを返す
    }

    public int GetHealNum()
    {
        return healingNum;
    }

}
