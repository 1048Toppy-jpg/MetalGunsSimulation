using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{

    //public
    public float speed = 1.0f;

    //private
    private Text text;
    private Image image;
    private float time;

    private bool fadeOutFlg = false;

    private enum ObjType
    {
        TEXT,
        IMAGE
    };
    private ObjType thisObjType = ObjType.TEXT;

    void Start()
    {
        //アタッチしてるオブジェクトを判別
        if (this.gameObject.GetComponent<Image>())
        {
            thisObjType = ObjType.IMAGE;
            image = this.gameObject.GetComponent<Image>();
        }
        else if (this.gameObject.GetComponent<Text>())
        {
            thisObjType = ObjType.TEXT;
            text = this.gameObject.GetComponent<Text>();
        }
        fadeOutFlg = false;
    }

    void Update()
    {
        if (fadeOutFlg) return;

        //オブジェクトのAlpha値を更新
        if (thisObjType == ObjType.IMAGE)
        {
            image.color = GetAlphaColor(image.color);
        }
        else if (thisObjType == ObjType.TEXT)
        {
            text.color = GetAlphaColor(text.color);
        }
    }

    //Alpha値を更新してColorを返す
    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * 5.0f * speed;
        color.a = Mathf.Sin(time) * 0.5f + 0.5f;

        return color;
    }
    public void FadeOut()
    {
        fadeOutFlg = true;
        if (thisObjType == ObjType.IMAGE)
        {
            image.color = new Color(image.color.r,
                image.color.g,
                image.color.b,
                image.color.a - 0.005f);
        }
        else if (thisObjType == ObjType.TEXT)
        {
            text.color += new Color(text.color.r,
                text.color.g,
                text.color.b,
                text.color.a - 0.005f);
        }
    }
    public void AlphaClear()
    {
        //点滅に使うカウントをリセットする
        time = 0.0f;
    }

}