using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSpawn : MonoBehaviour
{
    public GameObject guard;

    void Start()
    {
        Invoke("SpawnGuard", 0.5f);
    }

    void SpawnGuard()
    {
        Instantiate(guard, this.transform.position, this.transform.rotation);
    }
}
