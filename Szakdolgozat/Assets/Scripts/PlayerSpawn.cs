using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public bool isPlayerSpawned = false;
    public GameObject player;
    private GameObject playerSpawnPoint;

    void Start()
    {
        Invoke("playerSpawn", 0.1f);
    }

    void playerSpawn()
    {
        playerSpawnPoint = GameObject.FindWithTag("PlayerSpawnPoint");
        if (isPlayerSpawned == false)
        {
            Instantiate(player,playerSpawnPoint.transform.position,Quaternion.identity);
            isPlayerSpawned = true;

        }
        
    }
}
