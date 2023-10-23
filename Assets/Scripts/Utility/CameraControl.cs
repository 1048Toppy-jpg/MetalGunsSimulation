using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    private GameObject mainCamObj = null;
    private Camera mainCamera = null;
    private GameObject subCamObj = null;
    private Camera subCamera = null;

    //メインのカメラ操作
    private Axis axis = null;
    //サブのカメラ操作
    private Look look = null;


    private bool setupFlg = false;

    // Start is called before the first frame update
    void Start()
    {
        //=======================
        //カメラの取得
        //=======================

        //メイン
        mainCamObj = GameObject.Find("MainCamera");
        if (!mainCamObj) Debug.LogError("メインカメラオブジェクトが見つかりませんでした");
        if (mainCamObj.TryGetComponent(out mainCamera) == false) Debug.LogError("メインカメラがありません");
        //操作
        var AxisObj = GameObject.Find("MainCameraAxis");
        if (!AxisObj) Debug.LogError("AxisObjectがありません");
        else if (AxisObj.TryGetComponent(out axis) == false) Debug.LogError("Axisがありません");

        //サブ
        subCamObj = GameObject.Find("SubCamera");
        if (!subCamObj) Debug.LogError("サブカメラオブジェクトが見つかりませんでした");
        if (subCamObj.TryGetComponent(out subCamera) == false) Debug.LogError("サブカメラがありません");
        //操作
        if (subCamObj.TryGetComponent(out look) == false) Debug.LogError("Lookオブジェクトがありません");


        //優先順位(高ければ優先される)
        mainCamera.depth = 1;
        subCamera.depth = 0;


    }

    // Update is called once per frame
    void Update()
    {
  
    }

}