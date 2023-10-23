using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedRot : MonoBehaviour
{

    private Quaternion baseRot;
    // Start is called before the first frame update
    void Start()
    {
        baseRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation != baseRot)
        {

            transform.rotation = baseRot;
        }

    }
}
