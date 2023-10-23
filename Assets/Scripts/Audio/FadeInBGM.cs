using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInBGM : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] private bool isFade;
    public bool IsFade { get { return isFade; } }
    [SerializeField] private double fadeInSeconds = 1.0;
    double fadeDeltaTime = 0;
    [SerializeField] private float maxValue = 1.0f;

    //フェードアウト
    [SerializeField] private FadeOutBGM fadeOutBGM = null;
    [SerializeField]private bool faded = false;

    [SerializeField] private bool isBoss = false;

    void Start()
    {
        if (gameObject.TryGetComponent(out audioSource) == false)
            Debug.LogError("AudioSourceが見つかりませんでした。");

        if (maxValue > 1.0f)//1以上の修正
            maxValue = 1.0f;
        else if (maxValue < 0.0f)//0未満の修正
            maxValue = 0.0f;

        if (faded)//あらかじめフェードインが終わっている場合
            fadeDeltaTime = fadeInSeconds;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (isFade)
        {
            fadeDeltaTime += Time.deltaTime;
            if (fadeDeltaTime > fadeInSeconds)
            {
                fadeDeltaTime = fadeInSeconds;
                isFade = false;
                faded = true;
            }
            audioSource.volume = (float)(fadeDeltaTime / fadeInSeconds) * maxValue;
        }
    }

    public void FadeIn()
    {
        //起動していない場合 また フェードアウトしていない場合
        if (!isFade && !faded)
        {

            // if (fadeInBGM.IsFade)//フェードインが起動している場合
            fadeDeltaTime = fadeOutBGM.FadeIn();

            if (isBoss)
            {
                //ボスBGMの場合
                if (audioSource.isPlaying == false)
                {
                    fadeDeltaTime = fadeInSeconds;
                    audioSource.Play();
                }
            }
            isFade = true;//フェードアウトする
        }

    }

    public double FadeOut()
    {
        isFade = false;
        faded = false;
        return (fadeInSeconds - fadeDeltaTime);
    }
}
