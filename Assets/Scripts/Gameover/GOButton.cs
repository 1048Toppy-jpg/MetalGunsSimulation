using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class GOButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //ボタンタグ(どのボタンかを識別するため)
    [SerializeField] private string buttonTag = "";

    //カーソルのオブジェクト
    [SerializeField] private GunCursor cursorObj = null;
    
    //ボタンを押したかどうかのフラグ
    private bool clickButtonFlg = false;

    //SE (選択時・決定時)
    [SerializeField] private AudioClip focusSE = null;
    [SerializeField] private AudioClip selectSE = null;
    private AudioSource audioSource = null;

    [SerializeField] private GOButton anotherButton = null;

    [SerializeField] private Image buttonImage = null;


    // Start is called before the first frame update
    void Start()
    {
        clickButtonFlg = false;//ボタンを押していない状態

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
        if (clickButtonFlg)//ボタンが押されていない場合
        {
            cursorObj.SelectButton();//決定する
        }
    }

    public void OnClickContinue()
    {
        //デバッグログ
        //Debug.Log("ContinueButtonが押されました");
        if (!clickButtonFlg)//ボタンが押されていない場合
        {
            clickButtonFlg = true;//オンにする
            FadeManager.Instance.LoadScene("Game", 2.0f);//sceneをGameに変える

            if (selectSE && audioSource) audioSource.PlayOneShot(selectSE);//SEがあれば流す
            anotherButton.PushButton();
        }

    }
    public void OnClickExit()
    {
        //デバッグログ
        //Debug.Log("ExitButtonが押されました");
        if (!clickButtonFlg)//ボタンが押されていない場合
        {
            clickButtonFlg = true;//オンにする
            FadeManager.Instance.LoadScene("Title", 2.0f);//sceneをTitleに変える
            if (selectSE && audioSource) audioSource.PlayOneShot(selectSE);//SEがあれば流す
            anotherButton.PushButton();
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!clickButtonFlg)////ボタンが押されていない場合
        {
            buttonImage.color = Color.red;
            if (buttonTag == "Up")//タグがUPの場合
            {
                cursorObj.Up();//カーソルを上にあげる
            }
            else if (buttonTag == "Down")//タグがDownの場合
            {
                cursorObj.Down();//カーソルを下に下げる
            }
            if (focusSE && audioSource) audioSource.PlayOneShot(focusSE);//SEがあれば流す
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!clickButtonFlg)////ボタンが押されていない場合
        {
            buttonImage.color = Color.white;

        }
    }

    public void PushButton()
    {
        clickButtonFlg = true;
    }
}