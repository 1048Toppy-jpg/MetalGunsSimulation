using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{

    [SerializeField] private string objName = "";
    [SerializeField] private float range = 0.0f;


    private GameObject testObject = null;

    private Animator animator = null;

    bool openFlg = false;

    //AudioSource
    private AudioSource audioSource = null;

    //SE
    [SerializeField] private AudioClip openSE = null;
    [SerializeField] private AudioClip closeSE = null;

    //lock(敵を倒す鍵が必要)
    [SerializeField] private bool enemyLock = false;

    [SerializeField] private GameObject[] enemies = null;



    private bool outSideLock = false;

    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent(out animator) == false)
        {
            Debug.LogError("Animatorが見つかりません");
        }

        testObject = GameObject.Find(objName);
        if (!testObject)
        {
            Debug.LogError("検索するオブジェクトが見つかりません");
        }

        //AudioSourceの取得
        if (GameObject.Find("AudioManager").TryGetComponent(out audioSource) == false)
        {

            Debug.LogError("AudioManagerが見つかりませんでした");
            audioSource = null;
        }
        outSideLock = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        if (outSideLock)
        {
            return;
        }

        if (enemyLock)
        {//EnemyLockを設定している場合
            for (int index = 0; index < enemies.Length; index++)
            {
                if (enemies[index].activeSelf)//設定した敵が1体でも生きていれば
                    return;//開閉処理はせず終わる
            }
        }

        //距離を測る
        float length = 0.0f;

        // 距離 = (扉の座標 - プレイヤーの座標)のベクトルの長さ 
        length = (transform.position - testObject.transform.position).magnitude;

        //距離が設定した値より短くなったら
        if (length < range)
        {
            //扉が閉まっていれば
            if (openFlg == false)
            {
                //扉を開ける
                animator.SetTrigger("Open");

                if (audioSource && openSE)//SEを設定していれば流す
                    audioSource.PlayOneShot(openSE);
                //開いている状態にする
                openFlg = true;
            }

        }
        else
        {
            //扉が開いていれば
            if (openFlg == true)
            {
                //扉を閉める
                animator.SetTrigger("Close");
                if (audioSource && closeSE)//SEを設定していれば流す
                    audioSource.PlayOneShot(closeSE);

                //閉じている状態にする
                openFlg = false;
            }
        }
    }
    public void Lock()
    {
        outSideLock = true;

        //扉が開いていれば
        if (openFlg == true)
        {
            //扉を閉める
            animator.SetTrigger("Close");
            if (audioSource && closeSE)//SEを設定していれば流す
                audioSource.PlayOneShot(closeSE);

            //閉じている状態にする
            openFlg = false;
        }
    }
}
