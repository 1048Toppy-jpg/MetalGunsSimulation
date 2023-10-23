using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossBattle : MonoBehaviour
{

    [SerializeField] private GameObject bossObj = null;
    [SerializeField] private GameObject startPosObj = null;


    [SerializeField] private GameObject bossBar = null;


    private bool onceFlg = false;

    [SerializeField] private OpenDoor lastDoor = null;


    // Start is called before the first frame update
    void Start()
    {
        onceFlg = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (onceFlg == false)
            {
                onceFlg = true;

                if (bossObj != null)
                {
                    if (startPosObj != null)
                    {
                        //bossObj.SetActive(true);
                        var CloneBoss = Instantiate(bossObj);
                        CloneBoss.transform.position = startPosObj.transform.position;
                        CloneBoss.transform.rotation = startPosObj.transform.rotation;
                        //Destroy(startPosObj);
                    }
                }
                lastDoor.Lock();

                if (bossBar != null)
                    if (bossBar.activeSelf == false)
                        bossBar.SetActive(true);
            }
        }

    }
}