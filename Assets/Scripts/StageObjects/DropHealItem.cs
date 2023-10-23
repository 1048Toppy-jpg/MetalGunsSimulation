using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DropHealItem : MonoBehaviour
{
    //即使用するかどうかのフラグ
    [SerializeField] private bool quickUse = true;

    //取得数
    [SerializeField] private int getNum = 1;

    //回復量
    [SerializeField] private int healNum = 20;

    private void FixedUpdate()
    {
        //バグ対策(地面を突き抜けて落ちた場合Activeをfalseに変える)

        //座標の取得
        var position = gameObject.transform.position;
        if (position.y < -10.0f)//ある程度落ちたら
        {
            //activeをfalseに変える
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //デバッグログ
        Debug.Log("回復アイテムに当たりました");
        
        //当たったオブジェクトの名前を取得
        var name = other.gameObject.name;

        if (name == "Player")//プレイヤーの場合
        {
            if (quickUse)//即時使用の場合
            {
                //HPBarの取得
                HPBar hpBar = null;

                if (other.gameObject.TryGetComponent(out hpBar) == false)//HPBarが存在しない場合
                {
                    Debug.LogError("HPバー(Script)見つかりませんでした");//エラーメッセージ
                    return;
                }

                //↓見つかった場合
                hpBar.ChangeHP(healNum * getNum);//回復処理
            }
            else//ストックする場合
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
                                cureBoxText.AddCureBox(getNum, healNum);
                            break;
                        }
                    }
                }
            }
            //回復アイテムを消す
            gameObject.SetActive(false);

            //colliderである親オブジェクトも消す

            var ParentObj = gameObject.transform.parent.gameObject;
            //
            if (ParentObj != null)
                ParentObj.SetActive(false);
        }
    }
   
}