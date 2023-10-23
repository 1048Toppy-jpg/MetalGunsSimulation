using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{

    private Slider slider;

    private Slider sourceSlider;

    private bool startFlg=true;

    [SerializeField] private double fullHPTime = 1.0;

    private double deltaTime = 0.0;

    private BossController bossController = null;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.TryGetComponent(out slider) == false)
            Debug.LogError("ボスのHPバーが見つかりません");

        startFlg = true;

        slider.value = 0;
        deltaTime = 0.0;
    }

    // Update is called once per frame
    void Update()
    {
        if (startFlg)
        {
            if (gameObject.activeSelf)
            {
                deltaTime += Time.deltaTime;
                if (deltaTime > fullHPTime)
                {
                    slider.value = 1.0f;

                    //slider
                    var firstSliderObj = GameObject.Find("BossSlider");
                    if (firstSliderObj.TryGetComponent(out sourceSlider) == false)
                        Debug.LogError("Sliderが見つかりません");
                    else
                    {
                        slider.value = sourceSlider.value;
                    }

                    //BossController
                    var firstBossObj = GameObject.Find("BossObj(Clone)");
                    if (firstBossObj.TryGetComponent(out bossController) == false)
                        Debug.LogError("bossControllerが見つかりません");
                    else
                    {
                        slider.value = sourceSlider.value;
                    }



                    startFlg = false;
                }
                slider.value = (float)deltaTime / (float)fullHPTime;
            }

            return;
        }

        if (sourceSlider == null)
        {
            var sliderObj = GameObject.Find("BossSlider");
            if (sliderObj.TryGetComponent(out sourceSlider) == true)
                Debug.LogError("Sliderが見つかりません");
            else
            {
                slider.value = sourceSlider.value;
            }
        }
        else
        {
            slider.value = sourceSlider.value;
        }

        //
    }
}
