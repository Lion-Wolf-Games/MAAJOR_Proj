using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    [SerializeField] private GameObject smokeParticle;
    [SerializeField] private GameObject grassParticle;

    [SerializeField] private Transform smokeSpawnPoint;
    [SerializeField] private Transform grassSpawnPoint;

    public void Step()
    {
        Instantiate(smokeParticle, smokeSpawnPoint.position, Quaternion.identity);
        Instantiate(grassParticle, grassSpawnPoint.position, Quaternion.identity);
    }
}
