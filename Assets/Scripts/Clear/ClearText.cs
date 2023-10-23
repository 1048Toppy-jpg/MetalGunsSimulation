using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearText : MonoBehaviour
{

    private Image image = null;
    private double deltaTime = 0.0;
    [SerializeField] private double fadeInSeconds = 1.0;
    private bool isFade = true;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.TryGetComponent(out image) == false)

            Debug.LogError("Imageが見つかりませんでした");
        else
        {
            image.color = new Color(
                image.color.r,
                image.color.g,
                image.color.b,
                0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFade)
        {
            deltaTime += Time.deltaTime;
            if (deltaTime > fadeInSeconds)
            {
                deltaTime = fadeInSeconds;
                isFade = false;
            }

            image.color = new Color(
                image.color.r,
                image.color.g,
                image.color.b,
                (float)(deltaTime / fadeInSeconds));
        }

    }
}
