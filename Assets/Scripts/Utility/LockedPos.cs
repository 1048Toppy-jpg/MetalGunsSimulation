using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedPos : MonoBehaviour
{

    private Vector3 basePos;

    // Start is called before the first frame update
    void Start()
    {
        basePos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != basePos)
        {

            transform.position = basePos;
        }

    }
}
