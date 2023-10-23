using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    //SE (選択時・決定時)
    [SerializeField] private AudioClip focusSE = null;
    [SerializeField] private AudioClip selectSE = null;
    private AudioSource audioSource = null;

    //カーソルのオブジェクト
    [SerializeField] private GunCursor cursorObj = null;

    //ボタンを押したかどうかのフラグ
    private bool pushFlg = false;


    [SerializeField] private Image buttonImage = null;

    // Start is called before the first frame update
    void Start()
    {
        pushFlg = false;//ボタンを押していない状態

        //SE
        if (GameObject.Find("AudioManager").TryGetComponent(out audioSource) == false)
        {
            Debug.LogError("AudioManagerが見つかりませんでした。");
            audioSource = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pushFlg)
        {
            if (cursorObj != null)//カーソルが存在する場合
                cursorObj.SelectButton();//決定する
        }
    }


    public void PushStart()
    {
        if (!pushFlg)//ボタンが押されていない場合
        {
            pushFlg = true;//オンにする
            FadeManager.Instance.LoadScene("Game", 2.0f);//sceneをGameに変える
            if (selectSE && audioSource) audioSource.PlayOneShot(selectSE);//SEがあれば流す
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!pushFlg)//ボタンが押されていない場合
        {

            buttonImage.color = Color.red;
            if (focusSE && audioSource) audioSource.PlayOneShot(focusSE);//SEがあれば流す
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        
        if (!pushFlg)//ボタンが押されていない場合
        {
            buttonImage.color = Color.white;
        }
    }
}
