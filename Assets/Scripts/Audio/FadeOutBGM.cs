using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutBGM : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] private bool isFade;
    public bool IsFade { get { return isFade; } }//ゲッター
    [SerializeField] private double fadeOutSeconds = 1.0;
    double fadeDeltaTime = 0;
    [SerializeField] private float maxValue = 1.0f;

    //フェードイン
    [SerializeField] private FadeInBGM fadeInBGM = null;

    [SerializeField] private bool faded = false;

    void Start()
    {
        if (gameObject.TryGetComponent(out audioSource) == false)
            Debug.LogError("AudioSourceが見つかりませんでした。");

        if (maxValue > 1.0f)//1以上の修正
            maxValue = 1.0f;
        else if (maxValue < 0.0f)//0未満の修正
            maxValue = 0.0f;

        if (faded)//あらかじめフェードインが終わっている場合
            fadeDeltaTime = fadeOutSeconds;
    }

    void Update()
    {

    }
    private void FixedUpdate()
    {
        if (isFade)
        {
            fadeDeltaTime += Time.deltaTime;
            if (fadeDeltaTime > fadeOutSeconds)
            {
                fadeDeltaTime = fadeOutSeconds;
                isFade = false;
                faded = true;
            }
            //本来
            //audioSource.volume = (float)(1.0 - FadeDeltaTime / FadeOutSeconds);
            audioSource.volume = (float)(1.0 - (fadeDeltaTime / fadeOutSeconds)) * maxValue;
        }
    }

    public void FadeOut()
    {

        //起動していない場合 また フェードアウトしていない場合
        if (!isFade && !faded)
        {
            // if (fadeInBGM.IsFade)//フェードインが起動している場合
            fadeDeltaTime = fadeInBGM.FadeOut();

            isFade = true;//フェードアウトする
        }
    }
    public double FadeIn()
    {
        isFade = false;
        faded = false;
        return (fadeOutSeconds - fadeDeltaTime);
    }
}
