using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeshBuild : MonoBehaviour
{
    public NavMeshSurface surface;
    private RoomTemplates templates;


    private byte KeyPickedUp = 0;
    private bool isMeshGenerated = false;
    private bool isMeshUpdated = false;
    private bool isSpawnRoomsFound = false;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        isSpawnRoomsFound = true;
    }
    void FixedUpdate()
    {
        if(isMeshGenerated == false && templates.isFinishRoomSpawned == true)
        {
            surface.BuildNavMesh();
            isMeshGenerated = true;
        }

        if(isMeshUpdated = false && templates.KeyPickedUp > this.KeyPickedUp)
        {
            surface.BuildNavMesh();
            isMeshGenerated = true;
            this.KeyPickedUp = templates.KeyPickedUp;
        }
    }
}
