using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

using UnityEngine;

//エフェクト
[System.SerializableAttribute]//数種類の情報をListで管理するために入れる
public class Effect
{
    public GameObject effectObj;
    public bool normal;
}
public class StageObject : BaseController
{
    [SerializeField] private List<Effect> hitEffects = new List<Effect>();
    [SerializeField] private List<Effect> destroyEffects = new List<Effect>();


    //体力(入力値が0ならば死なない)
    [SerializeField] private int baseHp = 0;
    private int hp = 0;

    [SerializeField] private bool canDie = false;

    //Ray判定(当たった時情報を取得する)
    RaycastHit hitData;//当たった時の情報

    //SE
    [SerializeField] private AudioClip hitSE = null;//ヒット時
    [SerializeField] private AudioClip destroySE = null;//破壊時

    private AudioSource audioSource = null;


    public override void Restart()
    {
        positionReset.Execute();//座標をリセットする
        gameObject.SetActive(true);
        hp = baseHp;
    }

    public override void Init()
    {

        hp = baseHp;
        if (!canDie)//死ねない場合
        {

        }

        //SE
        if (GameObject.Find("AudioManager").TryGetComponent(out audioSource) == false)
        {
            Debug.LogError("AudioManagerが見つかりませんでした");
            audioSource = null;
        }


    }

    //弾丸が当たった時の処理
    public void HitBullet(RaycastHit hit)
    {
        //Ray判定のデータを格納
        hitData = hit;

        for (int index = 0; index < hitEffects.Count; index++)
        {
            //エフェクトの生成
            var cloneParticle = Instantiate(hitEffects[index].effectObj);
            cloneParticle.transform.position = hitData.point;//座標の設定
            //法線方向場合
            if (hitEffects[index].normal) cloneParticle.transform.localRotation =
                    Quaternion.LookRotation(hitData.normal);

            //親を木箱に設定
            cloneParticle.transform.SetParent(transform);
        }

        if (canDie)//無敵じゃない場合
        {
            hp--;//ダメージ加算
            if (hp <= 0)
            {
                BreakDown();//体力がなくなったら消す

            }
        }
        if (hitSE) audioSource.PlayOneShot(hitSE);
    }

    public void BreakDown()
    {
        for (int index = 0; index < hitEffects.Count; index++)
        {
            //エフェクトの生成
            var cloneParticle = Instantiate(destroyEffects[index].effectObj);
            cloneParticle.transform.position = transform.position;//座標の設定

            //法線方向の場合
            if (destroyEffects[index].normal) cloneParticle.transform.localRotation =
                    Quaternion.LookRotation(hitData.normal);//回転方向の設定
        }

        if (destroySE) audioSource.PlayOneShot(destroySE);

        //ヒットエフェクトを消す
        for (int index = 0; index < transform.childCount; index++)
        {
            if (transform.GetChild(index).tag == "Particle")
            {
                Destroy(transform.GetChild(index).gameObject);
            }
        }
        GameOver();
    }


}