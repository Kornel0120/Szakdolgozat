using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnRoomSpawn : MonoBehaviour
{
    public NavMeshSurface surface;

    private RoomTemplates templates;
    public bool isSpawnRoomSpawned = false;
    
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("SpawnRoom", 0.1f);
    }

    void SpawnRoom()
    {
        if(isSpawnRoomSpawned == false)
        {
            templates.prevRoom = Instantiate(templates.SpawnRoom, new Vector3(0.5f, 0, 0.5f), Quaternion.identity, templates.Parent.transform);
        }
        surface.BuildNavMesh();
    }
}
