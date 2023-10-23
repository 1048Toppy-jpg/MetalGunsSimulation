using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

//NavMeshAgent使うときに必要
using UnityEngine.AI;

//オブジェクトにNavMeshAgentコンポーネントを設置
[RequireComponent(typeof(NavMeshAgent))]


public class Chace2 : MonoBehaviour
{
    public Transform[] points;
    [SerializeField] int destPoint = 0;
    private NavMeshAgent agent;

    private float defaultAgentSpeed;

    Vector3 playerPos;
    GameObject player;
    float distance;
    [SerializeField] float trackingRange;
    [SerializeField] float quitRange;
    [SerializeField]public bool tracking = false;
  

    private EnemyShoot shoot = null;      //弾発射用

    private bool canShoot = true;
    public bool SetcanShoot{ set{ canShoot = value; } }

    [SerializeField] private bool isBoss = false;
    [SerializeField] private double startBossValue = 8.0;

    private EnemyShoot[] BossShoot = new EnemyShoot[12];      //弾発射用
    private bool AdvanceFlg = false;//前進フラグ
    private double deltaTime = 0.0;


    [SerializeField] private float halfAttackSpan = 0.3f;

    private int useBulletNum = 6;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.TryGetComponent(out agent) == false)
        {
            Debug.LogError("NavAgentがいません");
            agent = GetComponent<NavMeshAgent>();
        }

        // autoBraking を無効にすると、目標地点の間を継続的に移動します
        //(つまり、エージェントは目標地点に近づいても
        // 速度をおとしません)
        agent.autoBraking = false;
        defaultAgentSpeed = agent.speed;//スピードを記憶

        GotoNextPoint();

        //追跡したいオブジェクトの名前を入れる
        player = GameObject.FindWithTag("Player");

        if (isBoss)//ボスの場合
        {
            GameObject[] shootObj = new GameObject[12];

            shootObj[0] = GameObject.Find("BossShoot (0)");
            shootObj[1] = GameObject.Find("BossShoot (1)");
            shootObj[2] = GameObject.Find("BossShoot (2)");
            shootObj[3] = GameObject.Find("BossShoot (3)");
            shootObj[4] = GameObject.Find("BossShoot (4)");
            shootObj[5] = GameObject.Find("BossShoot (5)");
            shootObj[6] = GameObject.Find("BossShoot (6)");
            shootObj[7] = GameObject.Find("BossShoot (7)");
            shootObj[8] = GameObject.Find("BossShoot (8)");
            shootObj[9] = GameObject.Find("BossShoot (9)");
            shootObj[10] = GameObject.Find("BossShoot (10)");
            shootObj[11] = GameObject.Find("BossShoot (11)");

            for(int index = 0; index < 12; index++)
            {
                if (shootObj[index].TryGetComponent(out BossShoot[index]) == false)
                    Debug.LogError("ボスのShootObjectが見つかりませんした。");
            }
            AdvanceFlg = true;
            deltaTime = 0.0;
            useBulletNum = 6;
        }
        else //通常の敵
        {

            //Shootオブジェクトを取得
            var shootObj = gameObject.transform.Find("ShootObj").gameObject;
            if (shootObj == null)//見つからなかった場合
                Debug.LogError("EnemyShootObjが見つかりませんでした");//エラーメッセージ

            //EnemyShootを取得
            if (shootObj.TryGetComponent(out shoot) == false)//見つからなかった場合
                Debug.LogError("EnemyShootが見つかりませんでした");//エラーメッセージ

        }
    }

    void GotoNextPoint()
    {
        // 地点がなにも設定されていないときに返します
        if (points.Length == 0)
            return;

        // エージェントが現在設定された目標地点に行くように設定します
        if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
        {
            agent.destination = points[destPoint].position;
        }
        // 配列内の次の位置を目標地点に設定し、
        // 必要ならば出発地点にもどります
        destPoint = (destPoint + 1) % points.Length;
    }

    // Update is called once per frame
    void Update()
    {
        //if (isBoss)
        //{
        //    if(AdvanceFlg)//前進
        //    {
        //
        //
        //
        //        return;
        //    }
        //}

        if (player.activeSelf)
        {
            //Playerとこのオブジェクトの距離を測る
            playerPos = player.transform.position;
            distance = Vector3.Distance(this.transform.position, playerPos);

            if (tracking)
            {
                //追跡の時、quitRangeより距離が離れたら中止
                if (distance > quitRange)
                    tracking = false;


                //Playerを目標とする
                if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
                    agent.destination = playerPos;

                if (canShoot)//打てる場合
                {

                    if (isBoss)//ボスの場合
                    {
                        for (int index = 0; index < useBulletNum; index++)
                        {
                            if (index > 11) break;//12以上は使わない(エラー)

                            if (BossShoot[index] == null)//弾情報がない場合
                            {
                                //エラーメッセージ
                                Debug.LogError("EnemyShootが見つかりませんでした");//エラーメッセージ
                            }
                            else
                            {
                                BossShoot[index].Shoot();//打つ
                            }
                        }
                    }
                    else//通常の敵の場合
                    {
                        if (shoot == null)
                        {
                            Debug.LogError("EnemyShootが見つかりませんでした");//エラーメッセージ
                        }
                        else
                        {
                            shoot.Shoot();//打つ
                        }
                    }
                }
            }
            else
            {
                //PlayerがtrackingRangeより近づいたら追跡開始
                if (distance < trackingRange)
                    tracking = true;

                // エージェントが現目標地点に近づいてきたら、
                // 次の目標地点を選択します
                if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
                    if (!agent.pathPending && agent.remainingDistance < 0.5f)
                        GotoNextPoint();
            }
        }
        else
        {
            if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }
    }

    void OnDrawGizmosSelected()
    {
        //trackingRangeの範囲を赤いワイヤーフレームで示す
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trackingRange);

        //quitRangeの範囲を青いワイヤーフレームで示す
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, quitRange);
    }

    public void Stop()
    {
        if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
            agent.speed = 0.0f;
        //デバッグログ
        Debug.Log("Stop");
    }
    public void Restart()
    {
        if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
            agent.speed = defaultAgentSpeed;
        Debug.Log("Restart");
    }

    public void SetTimeBullet()
    {
        for (int index = 0; index < 6; index++)
        {
            BossShoot[index].SetTimer((float)index * halfAttackSpan);
            BossShoot[index + 6].SetTimer((float)index * halfAttackSpan);
        }

        useBulletNum = 12;
    }

}
