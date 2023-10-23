using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarDirection : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main != null)
            canvas.transform.rotation =
                Camera.main.transform.rotation;
    }
}