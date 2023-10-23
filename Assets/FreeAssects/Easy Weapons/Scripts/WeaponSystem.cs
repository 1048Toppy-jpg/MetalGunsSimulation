/// <summary>
/// WeaponSystem.cs
/// Author: MutantGopher
/// This script manages weapon switching.  It's recommended that you attach this to a parent GameObject of all your weapons, but this is not necessary.
/// This script allows the player to switch weapons in two ways, by pressing the numbers corresponding to each weapon, or by scrolling with the mouse.
/// </summary>

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponSystem : MonoBehaviour
{
	public GameObject[] weapons;				// The array that holds all the weapons that the player has
	public int startingWeaponIndex = 0;			// The weapon index that the player will start with
	private int weaponIndex;                    // The current index of the active weapon

	/*追加*/

	[SerializeField] private bool[] canUseWeapons = new bool[5];


	// Use this for initialization
	void Start()
	{
		// Make sure the starting active weapon is the one selected by the user in startingWeaponIndex
		weaponIndex = startingWeaponIndex;
		SetActiveWeapon(weaponIndex);

		//武器の分だけbool型を生成する
		//canUsingWeapons = new bool[weapons.Length];
		//for (int index = 0; index < canUsingWeapons.Length; index++)
		//{
		//	canUsingWeapons[index] = false;//全てfalseにする
		//}
		//初期の武器だけtrueにする
		canUseWeapons[0] = true;

	}
	
	// Update is called once per frame
	void Update()
	{
		// Allow the user to instantly switch to any weapon
		if (Input.GetButtonDown("Weapon 1")) 
			SetActiveWeapon(0);
		if (Input.GetButtonDown("Weapon 2")) {
			SetActiveWeapon(2);
			canUseWeapons[2] = true;
		}
		if (Input.GetButtonDown("Weapon 3")) { 
			SetActiveWeapon(1);
			canUseWeapons[1] = true;
		}
		if (Input.GetButtonDown("Weapon 4")) {
			SetActiveWeapon(3);
			canUseWeapons[3] = true;
		}
		if (Input.GetButtonDown("Weapon 5"))
		{
			SetActiveWeapon(4);
			canUseWeapons[4] = true;
		}
		if (Input.GetButtonDown("Weapon 6"))
			SetActiveWeapon(5);
		if (Input.GetButtonDown("Weapon 7"))
			SetActiveWeapon(6);
		if (Input.GetButtonDown("Weapon 8"))
			SetActiveWeapon(7);
		if (Input.GetButtonDown("Weapon 9"))
			SetActiveWeapon(8);

		// Allow the user to scroll through the weapons
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
			PreviousWeapon();
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
			NextWeapon();
	}




	void OnGUI()
	{


	}

	public void SetActiveWeapon(int index)
	{
		// Make sure this weapon exists before trying to switch to it
		if (index >= weapons.Length || index < 0)
		{
			Debug.LogWarning("Tried to switch to a weapon that does not exist.  Make sure you have all the correct weapons in your weapons array.");
			return;
		}

		// Send a messsage so that users can do other actions whenever this happens
		SendMessageUpwards("OnEasyWeaponsSwitch", SendMessageOptions.DontRequireReceiver);

		// Make sure the weaponIndex references the correct weapon
		weaponIndex = index;

		// Make sure beam game objects aren't left over after weapon switching
		weapons[index].GetComponent<Weapon>().StopBeam();

		// Start be deactivating all weapons
		for (int i = 0; i < weapons.Length; i++)
		{
			weapons[i].SetActive(false);
		}

		// Activate the one weapon that we want
		weapons[index].SetActive(true);

		//追加***************************

		OriginalWeaponData weaponData = null;
		if (weapons[index].TryGetComponent(out weaponData))
		{
			weaponData.UpdateUI();
		}
		//*******************************

	}

	public void NextWeapon()
	{
		//次の武器へ
		weaponIndex++;
		if (weaponIndex > weapons.Length - 1)
			weaponIndex = 0;
		while (canUseWeapons[weaponIndex] == false)//指定した武器が使えない場合
		{
			//もう一つ次の武器へ
			weaponIndex++;
			if (weaponIndex > weapons.Length - 1)
				weaponIndex = 0;
		}

		//使える武器まで来たらActive
		SetActiveWeapon(weaponIndex);
	}

	public void PreviousWeapon()
	{
		//前の武器へ
		weaponIndex--;
		if (weaponIndex < 0)
			weaponIndex = weapons.Length - 1;
		while (canUseWeapons[weaponIndex] == false)//指定した武器が使えない場合
		{
			//もう一つ前の武器へ
			weaponIndex--;
			if (weaponIndex < 0)
				weaponIndex = weapons.Length - 1;
		}

		//使える武器まで来たらActive
		SetActiveWeapon(weaponIndex);
	}

	public void SetCanUseWeapon(int index,bool flg)
    {
		canUseWeapons[index] = flg;
    }

	public GameObject GetWeapon(int weaponNum)
	{
		return weapons[weaponNum];
	}

}
