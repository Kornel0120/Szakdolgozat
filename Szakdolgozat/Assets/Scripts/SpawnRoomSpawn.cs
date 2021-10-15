using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoomSpawn : MonoBehaviour
{
    private RoomTemplates templates;
    public bool spawnRoomSpawned = false;
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("SpawnRoom", 0.1f);
    }

    void SpawnRoom()
    {
        if(spawnRoomSpawned == false)
        {
            Instantiate(templates.SpawnRoom, new Vector3(0.5f, 0, 0.5f), Quaternion.identity);
            spawnRoomSpawned = true;
        }
    }
}
