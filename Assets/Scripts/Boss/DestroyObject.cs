using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{

    [SerializeField] private GameObject[] objects;

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
        if (other.gameObject.tag == "Player")
        {
            foreach (var obj in objects)
            {
                foreach (Transform childTrans in obj.transform)
                {
                    if (childTrans.tag == "StageObject")
                    {
                        Health health;
                        if (childTrans.TryGetComponent(out health) == true)
                            health.GameOver();

                    }
                    else if (childTrans.tag == "Enemy")
                    {

                        EnemyController enemy;
                        if (childTrans.TryGetComponent(out enemy) == true)
                            enemy.GameOver();
                    }

                }


                Destroy(obj);
            }

        }
    }

}
