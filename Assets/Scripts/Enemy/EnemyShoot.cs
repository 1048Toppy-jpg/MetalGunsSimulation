using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{

    [SerializeField] string EnemyTag = "Drone";
    public GameObject bulletPrefab;
    public float shootSpeed;
    public float timeBetweenShot = 0.35f;
    private float timer;
    [SerializeField] private int maxShootCount = 5;
    private int shootCount;
    public float reloadTime = 1.0f;
    private float Reloadtimer;

    public void SetTimer(float time) { timer = time; }


    public void Shoot()
    {

        // タイマーの時間を動かす
        timer += Time.deltaTime;

        //弾発射処理
        if (timer > timeBetweenShot)
        {
            // タイマーの時間を０に戻す。
            timer = 0.0f;
            if (shootCount < 1)
                return;

            Quaternion rotVec = Quaternion.identity;

            var playerObj = GameObject.Find("Player");
            if (playerObj == null)
            {
                Debug.LogError("プレイヤーが見つかりませんでした");
            }

            GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position,
                                       //Quaternion.Euler(45,210,0));
                                       rotVec);
            if (EnemyTag == "Drone")
                bullet.transform.LookAt(playerObj.transform);

            if (EnemyTag == "Warrior")
                bullet.transform.LookAt(gameObject.transform.position + gameObject.transform.forward);


            Destroy(bullet, 2.0f);

            // shootCountの数値を１ずつ減らす。
            shootCount -= 1;

        }

        //弾リロード処理
        if (shootCount <= 0)
        {
            Reloadtimer += Time.deltaTime;
            if (Reloadtimer > reloadTime)
            {
                shootCount = maxShootCount;
                Reloadtimer = 0.0f;
            }
        }
    }




}
