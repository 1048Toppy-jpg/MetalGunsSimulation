using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoClear : MonoBehaviour
{

    [SerializeField] private float fadeInSeconds = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")//プレイヤーの場合
        {
            FadeManager.Instance.LoadScene("Clear", fadeInSeconds, Color.white);
        }

    }

    public void StartClear()
    {

        FadeManager.Instance.LoadScene("Clear", fadeInSeconds, Color.white);
    }

}
