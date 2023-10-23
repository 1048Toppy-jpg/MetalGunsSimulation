using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponImage : MonoBehaviour
{

    private Image image = null;
    
    private RectTransform rcTrans = null;

    // Start is called before the first frame update
    void Start()
    {

        if (TryGetComponent(out image) == false)
        {
            Debug.LogError("Imageが見つかりません");
        }

        if (TryGetComponent(out rcTrans)==false)
        {
            Debug.LogError("rectTransformが見つかりません");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        //rcTrans.rect.Set(170, Screen.height - 55, 100, 50);
        rcTrans.transform.position.Set(170, Screen.height - 55, 0.0f);
        //rcTrans.position.Set(60, /*Screen.height - 50*/(Screen.height / 2) - 50, 0.0f);
    //    transform.position.Set(60, /*Screen.height - 50*/(Screen.height / 2) - 50, 0.0f);
      // Debug.Log("画面サイズ縦" + Screen.height);
    }

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;

    }
}
