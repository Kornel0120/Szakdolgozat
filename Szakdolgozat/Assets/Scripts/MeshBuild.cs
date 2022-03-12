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

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isMeshGenerated == false && templates.isFinishRoomSpawned == true)
        {
            surface.BuildNavMesh();
            GameObject.FindGameObjectWithTag("FinishRoom").GetComponent<GuardSpawn>().enabled = true;
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
