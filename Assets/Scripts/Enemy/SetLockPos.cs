using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLockPos : MonoBehaviour
{

    [SerializeField] private GameObject setObject = null;
    [SerializeField] private float flontValue = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (setObject)
        {
            transform.position = setObject.transform.position;//セットしたオブジェクトの座標をその

            var flontVec = setObject.transform.forward;
            flontVec = flontVec.normalized;
            flontVec *= flontValue;
            transform.position += flontVec;
        }
        else
        {
          //  transform.position = addSetPos;
        }
    }
}
