using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//参考サイトURL　：　https://unity-shoshinsha.biz/archives/574

public class Axis : MonoBehaviour
{

    //X軸の角度を制御するための変数
    [SerializeField]private float angleUp = 60.0f;
    [SerializeField]private float angleDown = -60.0f;

    //オブジェクト
    
    private GameObject player = null;
    private Camera camera = null;

    //Cameraが回転するスピード
    [SerializeField] private float rotateSpeed = 3.0f;

    //Axisの位置を決める変数
    [SerializeField] private Vector3 axisPos;

    //マウススクロールの値を入れる
    [SerializeField] private float scroll;
    //マウスホイールの値を保存
    [SerializeField] private float scrollLog;

    //プレイヤーからカメラまでの距離
    [SerializeField] private float nearLength = 3.0f;

    [SerializeField] private bool invertX = false;
    [SerializeField] private bool invertY = false;


    // Start is called before the first frame update
    void Start()
    {

        //プレイヤーの取得
        player = GameObject.Find("Player");
        if (!player) Debug.LogError("Playerが見つかりませんでした");//なかった場合

        var cameraObj = GameObject.Find("MainCamera");
        if (cameraObj)//ある場合
        {
            if (cameraObj.TryGetComponent(out camera) == false) 
                Debug.LogError("Cameraが見つかりませんでした");
        }
        else//なかった場合
        {
           
            Debug.LogError("CameraObjectが見つかりませんでした");
        }




        //CameraにAxisに相対的な位置をlocalPositionで指定
        camera.transform.localPosition = new Vector3(0, 0, -nearLength);
        //CameraとAxisの向きを最初だけ揃える
        camera.transform.localRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        //Axisの位置をプレイヤーの位置 + axisPosで決める
        transform.position = player.transform.position + axisPos;
        //三人称の時のCameraの位置にマウススクロールの値を足して位置を調整
        //thirdPosAdd = thirdPos + new Vector3(0, 0, scrollLog);

        //マウススクロールの値を入れる
        scroll = Input.GetAxis("Mouse ScrollWheel");
        //scrollAdd += Input.GetAxis("Mouse ScrollWheel") * -10;
        //マウススクロールの値は動かさないと0になるのでここで保存
        scrollLog += Input.GetAxis("Mouse ScrollWheel");

        //Cameraの位置、z座標にスクロール分を加える
        camera.transform.localPosition =
            new Vector3(
                camera.transform.localPosition.x,
                camera.transform.localPosition.y,
                camera.transform.localPosition.z + scroll);

        //マウスを反転する変数を追加
        int invertXNum = 1;
        if (invertX) invertXNum = -1;
        int invertYNum = 1;
        if (invertY) invertYNum = -1;


        //Cameraの角度にマウスからとった値を入れる
        transform.eulerAngles += new Vector3(
            Input.GetAxis("Mouse Y") * rotateSpeed * invertYNum,
            Input.GetAxis("Mouse X") * rotateSpeed * invertXNum,
            0);

        //X軸の角度
        float angleX = transform.eulerAngles.x;
        //X軸の値を180超えたら360引くことで制限しやすくする
        if (angleX>=180)
        {
            angleX -= 360;
        }
        //Mathf.Clamp(値、最小値、最大値)でX軸の値を制限する
        transform.eulerAngles = new Vector3(

            Mathf.Clamp(angleX, angleDown, angleUp),
            transform.eulerAngles.y,
            transform.eulerAngles.z);

    }
}
