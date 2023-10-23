/// <summary>
/// Health.cs
/// Author: MutantGopher
/// This is a sample health script.  If you use a different script for health,
/// make sure that it is called "Health".  If it is not, you may need to edit code
/// referencing the Health component from other scripts
/// </summary>

using UnityEngine;
using System.Collections;

public class Health : BaseController
{
	public bool canDie = true;                  // Whether or not this health can die

	public float startingHealth = 100.0f;       // The amount of health to start with
	public float maxHealth = 100.0f;            // The maximum amount of health
	private float currentHealth;                // The current ammount of health

	public bool replaceWhenDead = false;        // Whether or not a dead replacement should be instantiated.  (Useful for breaking/shattering/exploding effects)
	public GameObject deadReplacement;          // The prefab to instantiate when this GameObject dies
	public bool makeExplosion = false;          // Whether or not an explosion prefab should be instantiated
	public GameObject explosion;                // The explosion prefab to be instantiated

	public bool isPlayer = false;               // Whether or not this health is the player
	public GameObject deathCam;                 // The camera to activate when the player dies

	private bool dead = false;                  // Used to make sure the Die() function isn't called twice

	[SerializeField] private HPBar hpBar = null;

	//追加****************
	private Rigidbody rigidBody = null;


	//デス後SE
	//SEの ON・OFF
	[SerializeField] private bool playDestroySE = false;

	//音声本体
	[SerializeField] private AudioClip destroySE = null;

	//オーディオソース
	private AudioSource audioSource = null;

	public GameObject itemDrop;

	[SerializeField] private Transform startPoint =null;
	[SerializeField] private Transform firstBasePoint = null;
	[SerializeField] private Transform secondBasePoint = null;


	public float CurrentHealth
    {
        get { return this.currentHealth; }//取得用
		private set { this.currentHealth = value; }
    }

	public bool JudgeMaxHealth()//HPがマックスかどうか
	{
		if (currentHealth == maxHealth)//マックスな場合
			return true;
		else//そうじゃない場合
			return false;
	}

	public void ChangeHealth(float amount)
	{
		// Change the health by the amount specified in the amount variable
		currentHealth += amount;

		Debug.Log("現在のHP =" + currentHealth);

		//現在の体力
        //********
        if (isPlayer)
        {
			if (hpBar == null)//ない場合
			{
				if (gameObject.TryGetComponent(out hpBar) == false)//見つからなかった場合
					Debug.LogError("HPBarが見つかりませんでした");
				else//見つかった場合
				{
					hpBar.ChangeHP((int)amount);
				}
			}
			else//ある場合
			{
				hpBar.ChangeHP((int)amount);
			}
		}
		//********

		// If the health runs out, then Die.
		if (currentHealth <= 0 && !dead && canDie)
			Die();

		// Make sure that the health never exceeds the maximum health
		else if (currentHealth > maxHealth)
			currentHealth = maxHealth;
	}

	public void Die()
	{
		// This GameObject is officially dead.  This is used to make sure the Die() function isn't called again
		dead = true;

		// Make death effects
		if (replaceWhenDead)
			Instantiate(deadReplacement, transform.position, transform.rotation);
		if (makeExplosion)
			Instantiate(explosion, transform.position, transform.rotation);

		//追加**************************
		//SE再生
		if (playDestroySE && destroySE)
			audioSource.PlayOneShot(destroySE);
		//******************************

		if (isPlayer && deathCam != null)
        {
			deathCam.SetActive(true);

			AudioListener listener = null;
			if (deathCam.gameObject.TryGetComponent(out listener))
				listener.enabled = true;
        }

		//if (itemDrop.activeSelf)
		//	itemDrop.SetActive(true);
		if (itemDrop != null)
			Instantiate(itemDrop, transform.position + new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity).SetActive(true);
		
		// Remove this GameObject from the scene
		//Destroy(gameObject);
		//追加**************************
		gameObject.SetActive(false);
		//******************************
	}

	//追加**************************
	public override void Restart()
	{
		positionReset.Execute();//座標をリセットする
		gameObject.SetActive(true);

		//rigidBodyの感性をなくす
		if (rigidBody)
		{
			rigidBody.velocity = Vector3.zero;
			rigidBody.angularVelocity = Vector3.zero;
		}
		// Initialize the currentHealth variable to the value specified by the user in startingHealth
		currentHealth = startingHealth;
		dead = false;

		if (isPlayer)
		{
			if (hpBar == null)//ない場合
			{
				if (gameObject.TryGetComponent(out hpBar) == false)//見つからなかった場合
					Debug.LogError("HPBarが見つかりませんでした");
				else//見つかった場合
				{
					hpBar.ChangeHP((int)maxHealth);
				}
			}
			else//ある場合
			{
				hpBar.ChangeHP((int)maxHealth);
			}
		}
	}
	public override void Init()
	{
		// Initialize the currentHealth variable to the value specified by the user in startingHealth
		currentHealth = startingHealth;

		if (TryGetComponent(out positionReset) == false)
		{
			Debug.Log("PositionResetが無かったので追加します");
			positionReset = GetComponent<PositionReset>();
		}

		//AudioSourceの取得
		if (GameObject.Find("AudioManager").TryGetComponent(out audioSource) == false)
		{

			Debug.LogError("AudioManagerが見つかりませんでした");
			audioSource = null;
		}

		//RigidBody
		if (TryGetComponent(out rigidBody) == false)
		{
			//Debug.LogWarning(transform.name + "にrigidBodyが見つかりませんでした");
			rigidBody = null;
		}

		if (isPlayer)
		{
			if (hpBar == null)//ない場合
			{
				if (gameObject.TryGetComponent(out hpBar) == false)//見つからなかった場合
					Debug.LogError("HPBarが見つかりませんでした");
				else//見つかった場合
				{
					hpBar.ChangeHP((int)maxHealth);
				}
			}
			else//ある場合
			{
				hpBar.ChangeHP((int)maxHealth);
			}
		}
	}
    //******************************


    private void Update()
    {
        if(isPlayer){

			if (Input.GetButtonDown("Weapon 6"))
				gameObject.transform.position = startPoint.position;
			if (Input.GetButtonDown("Weapon 7"))
				gameObject.transform.position = firstBasePoint.position;
			if (Input.GetButtonDown("Weapon 8"))
				gameObject.transform.position = secondBasePoint.position;

		}
    }
}
