using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : singleton<GameManager>
{
   // [SerializeField] GameObject gameoverObject = null;
   //private ActionController actionController= null;

    public List<BaseController> Controllers { get; private set; } = new List<BaseController>();

    private bool fadeinFlg = false;

    public void GameOver()
    {
       // gameoverObject.SetActive(true);
       // actionController.GameOver();

        foreach(var Controller in Controllers)
        {
            Controller.GameOver();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
        }

        //プレイヤーの情報を取得
        var player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            if (!fadeinFlg)
            {
                fadeinFlg = true;
                FadeManager.Instance.LoadScene("Gameover", 2.0f);

            }

            return;//下を通らず終える
        }

        //プレイヤーの遠くに敵がいた場合、敵を消す処理
        foreach (var Controller in Controllers)
        {

            if (Controller.tag == "Enemy")//敵の場合
            {
                var EnemyObj = Controller.gameObject;//敵のオブジェクトを取得

                EnemyController enemyController = null;//controllerを取得
                if (EnemyObj.TryGetComponent(out enemyController) == false)//存在しない場合
                {
                    Debug.LogError("EnemyControllerが見つかりませんでした。");//エラーメッセージ
                    continue;//次の検索にかかる
                }

                //ある場合

                if (enemyController.CrashFlg)//敵が壊れていたら
                    continue;//次の検索にかかる

                var enemyPos = EnemyObj.transform.position;
                var playerPos = player.transform.position;
                var playerLength = (enemyPos - playerPos).magnitude;
                // Debug.LogError("Enemyが見つかりません");
                if (100.0f < playerLength)//100m以上離れていたら
                {
                    if (Controller.gameObject.activeSelf)
                    {
                        Controller.gameObject.SetActive(false);
                        //デバッグ
                        //Debug.Log("GameManagerでフラグが折れました");
                    }
                }
                else//近づいたら
                {
                    if (!Controller.gameObject.activeSelf)
                    {
                        Controller.gameObject.SetActive(true);
                        //Debug.Log("GameManagerでフラグを立てました");
                    }
                }
            }
        }
    }

    private void Restart()
    {
       // gameoverObject.SetActive(false);
        
        //actionController.Restart();

        foreach (var Controller in Controllers)
        {
            Controller.Restart();
        }
    }
    private void Start()
    {
        var player = GameObject.FindWithTag("Player");
        if (!player) Debug.LogError("Playerが見つかりません");
        //else if (player.TryGetComponent(out actionController) == false)
        //{
        //    Debug.LogError("アクションコントローラーが見つかりません");
        //
        //}
        fadeinFlg = false;
    }
}
