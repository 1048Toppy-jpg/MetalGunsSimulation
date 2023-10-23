using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//参考サイト　：　https://hatsuka.frontl1ne.net/



public class Look : MonoBehaviour
{
    //Playerのtransform
    private Transform playerTransform;
    private Transform horRot;


    [SerializeField] private bool invertX = false;
    [SerializeField] private bool invertY = false;

    private CameraControl camControl = null;

    [SerializeField] private Transform rotPoint = null;

    private Vector3 defaultPos;

    private GameObject player = null;

    // Start is called before the first frame update
    void Start()
    {

        playerTransform = transform.parent;
        horRot = GetComponent<Transform>();

        var camControlObj = GameObject.Find("CameraController");
        if(!camControlObj)Debug.LogError("カメラコントーラーオブジェクトがありませんでした");
        if (camControlObj.TryGetComponent(out camControl) == false)
            Debug.LogError("カメラコントーラーがありませんでした");

    }

    // Update is called once per frame
    void Update()
    {
        float X_Rotation = Input.GetAxis("Mouse X");
        float Y_Rotation = Input.GetAxis("Mouse Y");

        if (invertX) X_Rotation *= -1.0f;
        if (invertY) Y_Rotation *= -1.0f;

        //Debug.Log(X_Rotation);
        playerTransform.transform.Rotate(0, -X_Rotation, 0);
        
        var rotatePoint = rotPoint.position;


        float maxX = 60;
        float minX = maxX *= -1;

        maxX *= Mathf.Deg2Rad;
        minX *= Mathf.Deg2Rad;

        horRot.transform.RotateAround(rotatePoint, -transform.right, -Y_Rotation);
        
        if (horRot.transform.rotation.x > maxX)
        {
            horRot.transform.rotation.Set(
                maxX,
                 horRot.transform.rotation.y,
                 horRot.transform.rotation.z,
                horRot.transform.rotation.w
                );
        }

        //Debug.Log(horRot.transform.rotation);
        //Debug.Log(X_Rotation);


        //horRot.transform.Rotate(Y_Rotation, 0, 0);
        
    }
}
