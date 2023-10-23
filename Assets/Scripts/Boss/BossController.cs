using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class BossController : BaseController
{

    //パーティクル
    [SerializeField] GameObject hitParticleObject = null;       //爆発
    [SerializeField] GameObject fallParticleObject = null;   //大爆発
    [SerializeField] GameObject destroyParticleObject = null;   //大爆発
    [SerializeField] GameObject smokeParticleObject = null;     //煙
    [SerializeField] private GameObject modelObject = null;     //敵本体
    [SerializeField] private GameObject scratchObject = null;     //敵本体


    //SE
    [SerializeField] private AudioClip hitSE = null;
    [SerializeField] private AudioClip destroySE = null;
    private AudioSource audioSource = null;

    //HP関連
    private EnemyHPBar hpBar = null;
    [SerializeField] private int baseHp = 30;
    private int hp;


    //煙
    [SerializeField] private int smokeDefaultInterval = 20;//煙の間隔(デフォルト値)
    [SerializeField] private float smokeKillHeigth = 2.0f;//煙が消える高度(低さ)　　※この値より低いと消える
    private int smokeInterval = 0;//計算用

    //破壊フラグ
    private bool crashFlg = false;//壊れているかどうか
    private float modelDefaultHeight = 0.0f;//リセット用にmodelのY座標情報を確保しておく

    public bool CrashFlg
    {
        get { return this.crashFlg; }//取得用
        private set { this.crashFlg = value; } //値入力用
    }

    //Warrior用
    private Chace2 chace;


    [SerializeField] private int explosionWaitDefaultTime = 60;//破壊してから爆破するまでの時間
    private int explosionWaitTime;

    public float CrashHeight = 2.0f;


    private GameObject playerObj = null;


    private NavMeshAgent agent = null;


    [SerializeField]private double breakSpan = 0.5;

    [SerializeField] private double spawnTime = 5.0;
    [SerializeField] private float spawnWalkSpeed = 0.1f;
    private double spawnDeltaTime = 0.0;


    private bool spawnFlg = true;

    private bool halfLifeFlg = false;

    public override void Restart()
    {

        positionReset.Execute();//座標をリセットする
        gameObject.SetActive(true);

        //Y座標の設定
        var newPos = modelObject.transform.position;//model座標の取得
        newPos.y = modelDefaultHeight;//Y座標の変更
        modelObject.transform.position = newPos;//変更した座標に更新する


        //HP関連

        hp = baseHp;//体力リセット

        if (TryGetComponent(out hpBar) == false)
            Debug.Log("HPバーがありません");
        else
            //体力バーのリセット
            hpBar.SetBaseHP(baseHp);





        smokeInterval = smokeDefaultInterval;


        crashFlg = false;

        //爆発するまでの時間のセット
        explosionWaitTime = explosionWaitDefaultTime;

        //Chase2を取得
        if (gameObject.TryGetComponent(out chace) == false)
            Debug.LogError("Chaceが見つかりませんでした");
        else
        {
            chace.tracking = false;
            chace.SetcanShoot = true;
            chace.Restart();
        }

        halfLifeFlg = false;

    }

    public override void Init()
    {


        //SE
        if (GameObject.Find("AudioManager").TryGetComponent(out audioSource) == false)
        {
            Debug.LogError("AudioManagerが見つかりませんでした。");
            audioSource = null;
        }

        //HP
        if (TryGetComponent(out hpBar) == false)
        {
            Debug.Log("HPバーがありません");
            hpBar = GetComponent<EnemyHPBar>();
        }
        hp = baseHp;
        hpBar.SetBaseHP(baseHp);


        //煙のintervalをセット
        smokeInterval = smokeDefaultInterval;


        //モデルのY座標のデフォルト値を記憶
        modelDefaultHeight = modelObject.transform.position.y;

        //Chase2を取得
        if (gameObject.TryGetComponent(out chace) == false)
            Debug.LogError("Chaceが見つかりませんでした");
        else
            chace.SetcanShoot = true;
        //爆発するまでの時間のセット
        explosionWaitTime = explosionWaitDefaultTime;

        playerObj = GameObject.Find("Player");
        if (playerObj == null)
            Debug.LogError("プレイヤーが見つかりません");

        if (gameObject.TryGetComponent(out agent) == false)
            Debug.LogError("NavMeshAgentが見つかりません");


        halfLifeFlg = false;


    }

    public void HitBullet(int damage, Vector3 damagePos)
    {
        //*******************************
        //Drone・Warrior　共通
        //*******************************

        if (!crashFlg)//まだ壊れていない場合
            if (hitSE && audioSource) audioSource.PlayOneShot(hitSE);

        //HPの減少処理
        hp -= damage;

        if (hpBar)
        {
            hpBar.DamageHP(damage);
        }
        if (hp <= 0)//破壊された場合
        {
            //GameOver();
            if (!crashFlg)//破壊された直後の場合
            {

                crashFlg = true;
                
                if (agent.enabled == true)//追尾を辞める
                {
                    agent.enabled = false;
                }
                if (chace.enabled == true)
                {
                    chace.enabled = false;
                }


                //デバッグログ
                Debug.Log(this.name + " = 破壊");

                //SE
                if (destroySE && audioSource) audioSource.PlayOneShot(destroySE);

                //パーティクル
                var deathCloneParticle = Instantiate(fallParticleObject);
                deathCloneParticle.transform.position = modelObject.transform.position;


                //BGMを消す

                var BGMtriggerObj = GameObject.Find("BossTrigger");
                if(BGMtriggerObj!=null)
                {
                    FadeBGMTrigger BGMTrigger;

                    if (BGMtriggerObj.TryGetComponent(out BGMTrigger))
                    {

                        BGMTrigger.FadeOut();
                    }

                }


            }
        }
        else
        {

            //パーティクル
            var cloneParticle = Instantiate(hitParticleObject);
            cloneParticle.transform.position = damagePos;

        }

    }

    private void FixedUpdate()
    {
        //プレイヤーがいない場合通らない
        if (playerObj == null)
        {
            //NavMeshAgent・Chaceの停止
            if (agent == null)
            {
                agent = GetComponent<NavMeshAgent>();
            }
            if (agent.enabled)
                agent.enabled = false;

            if (chace.enabled)
                chace.enabled = false;
            return;
        }
        /*
        var enemyPos = gameObject.transform.position;
        var playerPos = playerObj.transform.position;
        var playerLength = (enemyPos - playerPos).magnitude;

        //動ける範囲にいない場合通らない
        if (activeLength < playerLength)
        {
            //NavMeshAgent・Chaceの停止
            if (agent.enabled)
                agent.enabled = false;
            if (chace.enabled)
                chace.enabled = false;
            return;
        }*/

        //NavMeshAgent・Chaceの起動
        //壊れていない場合 且つ 出現フェイズじゃない場合
        if (!agent.enabled && !crashFlg)
            agent.enabled = true;
        if (!chace.enabled && !crashFlg)
            chace.enabled = true;


        if (spawnFlg)
        {
            spawnDeltaTime += Time.deltaTime;
            if (spawnDeltaTime > spawnTime)
            {
                spawnFlg = false;
            }
            chace.tracking = false;
        }



        if (hp < (baseHp / 2))//体力が半分以下に減ったら
        {
            if (!halfLifeFlg)
            {
                //この処理を一度だけ通るためフラグをtrueにして
                //通れなくする
                halfLifeFlg = true;
                chace.SetTimeBullet();//弾の出し方を変える


                for (int index = 0; index < 3; index++)
                {
                    //パーティクル
                    var ScratchCloneParticle = Instantiate(scratchObject);
                    ScratchCloneParticle.transform.position = modelObject.transform.position;

                    var randX = (Random.Range(0.0f, 30.0f)) - 15.0f;
                    var randZ = (Random.Range(0.0f, 10.0f)) - 5.0f;
                    ScratchCloneParticle.transform.position += new Vector3(randX, -1.5f, randZ);
                    ScratchCloneParticle.transform.parent = this.transform;
                }


            }
            //モデルから地面までのベクトルを取得
            var lengthVec =
                modelObject.transform.position -    //モデルの座標から
                gameObject.transform.position;      //地面までの座標を引く

            float length = lengthVec.magnitude;

            if (length > smokeKillHeigth)//指定した高さより高かったら
            {
                //煙の間隔処理
                smokeInterval--;
                if (smokeInterval <= 0)//0になったら
                {
                    if (smokeParticleObject != null)//煙情報が存在する場合
                    {
                        //クローンを生成
                        var smokeCloneParticle = Instantiate(smokeParticleObject);
                        smokeCloneParticle.transform.position = modelObject.transform.position;//座標を合わせる
                    }
                    //カウントをリセットする
                    smokeInterval = smokeDefaultInterval;
                }
            }
        }

        if (crashFlg)//破壊された場合
        {

            breakSpan -= Time.deltaTime;

            if (breakSpan < 0.0)
            {
                breakSpan = 0.5;
                //SE
                if (destroySE && audioSource) audioSource.PlayOneShot(hitSE);

                //パーティクル
                var deathCloneParticle = Instantiate(fallParticleObject);
                deathCloneParticle.transform.position = modelObject.transform.position;

                var randX = (Random.Range(0.0f, 30.0f)) - 15.0f;
                var randZ = (Random.Range(0.0f, 10.0f)) - 5.0f;
                deathCloneParticle.transform.position += new Vector3(randX, -3.0f, randZ);
            }


            //墜落する
            modelObject.transform.position += new Vector3(0.0f, -0.1f, 0.0f);

            if (modelObject.transform.position.y < gameObject.transform.position.y + CrashHeight)
            {


                GoClear clear;
                if (gameObject.TryGetComponent(out clear) == false)
                    Debug.LogError("クリアが見つかりませんでした。");
                else
                    clear.StartClear();


                //Y座標の設定
                //var newPos = modelObject.transform.position;//model座標の取得
                //newPos.y = gameObject.transform.position.y;//Y座標の変更
                //modelObject.transform.position = newPos;//変更した座標に更新する

                GameOver();
                //SE
                if (destroySE && audioSource) audioSource.PlayOneShot(destroySE);

                //パーティクル
                var deathCloneParticle = Instantiate(destroyParticleObject);
                deathCloneParticle.transform.position = modelObject.transform.position;
            }

            chace.SetcanShoot = false;
        }

    }

    public void Die()
    {
        GameOver();
        //SE
        if (destroySE && audioSource) audioSource.PlayOneShot(destroySE);

        //パーティクル
        var deathCloneParticle = Instantiate(fallParticleObject);
        deathCloneParticle.transform.position = modelObject.transform.position;
    }


}