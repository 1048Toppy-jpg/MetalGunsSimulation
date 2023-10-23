using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionTrigger : MonoBehaviour
{

    //UnityEvent自体にコールバックの機能がついているがジェネリックであるので
    //抽象的なクラスとして扱うUnity上で表示できない
    //なのでUnity上で設定する場合は継承してジェネリック部分を埋める必要がある

    [SerializeField] public class CallBackFunction : UnityEvent<GameObject> { }


    //当たり判定初めに呼ばれる関数
    [field:SerializeField]public CallBackFunction enterFunction { get; set; }
    
    //当たり判定終わりに呼ばれる関数
    [field:SerializeField]public CallBackFunction exitFunction { get; set; }

    //タグを複数持てるようリスト管理
    [SerializeField] List<string> tags = new List<string>();



    private void OnTriggerEnter(Collider other)
    {
        //リストの中から、collision.tagと一致するものがあるかどうか
        if (tags.Contains(other.tag))
        {
            enterFunction.Invoke(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //上と同じく
        if (tags.Contains(other.tag))
        {
            exitFunction.Invoke(other.gameObject);
        }
    }





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
