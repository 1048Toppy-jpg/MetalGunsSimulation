using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWeapon : MonoBehaviour
{

    //WeaponSystem
    private WeaponSystem weaponSystem = null;

    //武器を特定する番号
    [SerializeField] private int DropNumber = 1;

    //弾か本体か(true : 弾  false : 本体)
    [SerializeField] private bool isBullet = false;
    [SerializeField] private int getBulletNum = 5;

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
        //Debug.Log("武器に当たりました");

        //当たったオブジェクトの名前を取得
        var name = other.gameObject.name;

        if (name == "Player")//プレイヤーの場合
        {
            //WeaponSystemの取得
            var weapons = GameObject.Find("Weapons");
            if (weapons == null)
                Debug.LogError("weaponsが見つかりません");
            else if (weapons.TryGetComponent(out weaponSystem) == false)
            {
                Debug.LogError("weaponSystemが見つかりません");
                return;
            }

            if (isBullet)//弾の場合
            {
                //拾った弾の武器オブジェクトを取得
                var weaponObj = weaponSystem.GetWeapon(DropNumber);
                if (weaponObj == null)//オブジェクトがない場合
                    Debug.LogError("WeaponObjが見つかりませんでした");

                else//武器が存在する場合(まだ持っていなくても取得可能(つまりActiveは関係ない))
                {
                    //武器情報を取得
                    OriginalWeaponData weaponData = null;
                    if (weaponObj.TryGetComponent(out weaponData) == false)//情報が存在しない場合
                        Debug.LogError("WeaponDataが見つかりませんでした");
                    else//存在する場合
                    {
                        //指定の武器に球を追加する
                        weaponData.AddWeaponBullet(getBulletNum);
                    }

                }

            }
            else//本体の場合
            {
                //指定した武器の有効化
                weaponSystem.SetCanUseWeapon(DropNumber, true);
                //指定した武器に持ち替える
                weaponSystem.SetActiveWeapon(DropNumber);
            }

            //武器を消す
            gameObject.SetActive(false);
            
            //弾の場合は地面に接しているコライダーが入った親オブジェクトも消す
            if (isBullet)
            {
                var ParentObj = gameObject.transform.parent.gameObject;
                if (ParentObj != null)
                    ParentObj.SetActive(false);
            }
        }
    }

}
