using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSmoke : MonoBehaviour
{
    [SerializeField] GameObject damegeParticleObject = null;
    [SerializeField] private GameObject modelObject = null;
    bool Bossflg = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Smoke()
    {
        Bossflg = true;
    }

    // Update is called once per frame
    public void Update()
    {
        if (Bossflg == true)
        {
            var damageCloneParticle = Instantiate(damegeParticleObject);
            damageCloneParticle.transform.position = modelObject.transform.position;
        }
    }
}
