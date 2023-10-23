using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBGMTrigger : MonoBehaviour
{

    [SerializeField] private bool fadeInFlg = false;
    [SerializeField] private bool fadeOutFlg = false;

    [SerializeField] private GameObject bgm = null;

    private FadeInBGM inBGM = null;
    private FadeOutBGM outBGM = null;

    // Start is called before the first frame update
    void Start()
    {
        if ((fadeInFlg && fadeOutFlg) ||//フェードインとアウトが両方trueの場合
            (!fadeInFlg && !fadeOutFlg))//もしくは両方falseの場合
        {
            fadeInFlg = true;//フェードインをtrueに
            fadeOutFlg = false;//アウトをfalseに
        }

        if (bgm != null)//BGMが入っている場合
        {
            if (bgm.TryGetComponent(out inBGM) == false)//フェードインをするスクリプトが入っていない場合
                Debug.LogError("FadeInBGMが見つからなかったよ");
            if (bgm.TryGetComponent(out outBGM) == false)//フェードアウトをするスクリプトが入っていない場合
                Debug.LogError("FadeOutBGMが見つからなかったよ");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {

            if (fadeInFlg && inBGM != null)
            {
                inBGM.FadeIn();
                //Debug.Log("BGMをフェードインしたよ");
            }
            else if (fadeOutFlg && outBGM != null)
            {

                outBGM.FadeOut();
                //Debug.Log("BGMをフェードアウトしたよ");
            }
        }
    }

    public void FadeIn()
    {
        if (fadeInFlg && inBGM != null)
        {
            inBGM.FadeIn();
            //Debug.Log("BGMをフェードインしたよ");
        }
    }

    public void FadeOut()
    {
        if (fadeOutFlg && outBGM != null)
        {

            outBGM.FadeOut();
            //Debug.Log("BGMをフェードアウトしたよ");
        }
    }

}
