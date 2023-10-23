using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEnd : MonoBehaviour
{

    private void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}
