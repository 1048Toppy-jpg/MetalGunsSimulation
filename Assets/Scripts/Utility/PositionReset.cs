using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionReset : MonoBehaviour
{

    Vector3 defaultPosition = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Execute()
    {
        transform.position = defaultPosition;
    }

}
