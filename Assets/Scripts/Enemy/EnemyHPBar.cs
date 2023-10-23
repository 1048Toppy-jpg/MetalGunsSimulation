using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    int maxHp = 100;
    int currentHp;

    //Sliderを入れる
    [SerializeField] private Slider slider;



    // Start is called before the first frame update
    void Start()
    {
        //Sliderを満タンにする。
        slider.value = 1;
 
       // Debug.Log("Start currentHp :" + currentHp);
    }
    public void SetBaseHP(int baseHp)
    {
        //Sliderを満タンにする。
        slider.value = 1;

        //現在のHPを最大HPと同じに。
        currentHp = maxHp = baseHp;
        //Debug.Log("Start currentHp :" + currentHp);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void DamageHP(int damage) {
        //現在のHPからダメージを引く
        currentHp = currentHp - damage;
        //Debug.Log("After currentHp : " + currentHp);

        //最大HPにおける現在のHPをSliderに反映。
        //int同士の割り算は小数点以下は0になるので、
        //(float)をつけてfloatの変数として振舞わせる。
        slider.value = (float)currentHp / (float)maxHp; ;
        //Debug.Log("slider.value : " + slider.value);
    }

}
