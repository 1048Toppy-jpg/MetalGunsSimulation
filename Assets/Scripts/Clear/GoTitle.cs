using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoTitle : MonoBehaviour
{

    [SerializeField] private double canClickSeconds = 1.0;//クリック可能までの時間

    private double deltaTime = 0;//計算用

    [SerializeField] private bool canClickFlg = false;//クリック可能かどうか

    // Start is called before the first frame update
    void Start()
    {
        deltaTime = 0;//リセット
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;//秒数を足しこむ
        if (deltaTime > canClickSeconds)//設定した秒数を超えたら
        {
            deltaTime = canClickSeconds;//値を同じにする
            canClickFlg = true;//クリック可能にする
        }


        if (canClickFlg)//クリック可能な場合
        {
            if (Input.GetMouseButtonDown(0))//左クリックを押した場合
            {
                FadeManager.Instance.LoadScene("Title", 3.0f);//タイトルにシーン切り替え
            }
        }
    }
}