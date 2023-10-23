using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
	private AsyncOperation async;
	//　シーンロード中に表示するUI画面
	[SerializeField]
	private GameObject loadUI;
	//　読み込み率を表示するスライダー
	[SerializeField]
	private Slider slider;


	//SE (選択時・決定時)
	[SerializeField] private AudioClip selectSE = null;
	private AudioSource audioSource = null;

	void Start()
	{
		//SE
		if (GameObject.Find("AudioManager").TryGetComponent(out audioSource) == false)
		{
			Debug.LogError("AudioManagerが見つかりませんでした。");
			audioSource = null;
		}
	}

	public void NextScene()
	{
		if (selectSE && audioSource) audioSource.PlayOneShot(selectSE);//SEがあれば流す
		
		//　ロード画面UIをアクティブにする
		loadUI.SetActive(true);


		Invoke("tempMethod", 0.8f);

	}

	private void tempMethod()
	{
		//　コルーチンを開始
		StartCoroutine(LoadData());
	} 

	private IEnumerator LoadData()
	{

		// シーンの読み込みをする
		async = SceneManager.LoadSceneAsync("Game");
		//　読み込みが終わるまで進捗状況をスライダーの値に反映させる
		while (!async.isDone)
		{
			var progressVal = Mathf.Clamp01(async.progress / 0.9f);
			slider.value = progressVal;
			yield return null;
		
		}
	}
}