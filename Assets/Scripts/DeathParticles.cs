using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticles : MonoBehaviour
{
    private ParticleSystem PS;

    void Start() => PS = GetComponent<ParticleSystem>();

    void Update()
    {
        if (!PS.IsAlive()) { Destroy(gameObject); }
    }
}
