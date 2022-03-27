using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteStageRooms : MonoBehaviour
{
    private RoomTemplates templates;
    private int isFirstRoomsLength;
    private RoomSpawner occupiedSpawner;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        foreach (RoomSpawner rs in gameObject.GetComponentsInChildren<RoomSpawner>())
        {
            if(rs.nextRoom == null)
                occupiedSpawner = rs;
        }
        isFirstRoomsLength = templates.isFirstRoomsGenerated.Count - 1;
        Debug.Log("Start: " + templates.isFirstRoomsGenerated.Count);
        Invoke("DeleteRooms", 0.1f);
    }

    private void OnEnable()
    {
        foreach (RoomSpawner rs in gameObject.GetComponentsInChildren<RoomSpawner>())
        {
            if (rs.nextRoom == null)
                occupiedSpawner = rs;
        }
        isFirstRoomsLength = templates.isFirstRoomsGenerated.Count - 1; //nullref
        Invoke("DeleteRooms", 0.1f);
    }

    private void DeleteRooms()
    {
        for (int i = 0; i < this.gameObject.GetComponentsInChildren<RoomSpawner>().Length - 1; i++)
        {
            Debug.Log(this.gameObject.GetComponentsInChildren<RoomSpawner>().Length);
            for (int j = 0; j < templates.g.roomLists[this.gameObject.GetComponentsInChildren<RoomSpawner>()[i].GetComponentInParent<AddRoom>().lastIndex + i].Count; j++)
            {
                if (!templates.g.roomLists[this.gameObject.GetComponentsInChildren<RoomSpawner>()[i].GetComponentInParent<AddRoom>().lastIndex + i][j].gObject.CompareTag("CheckPointRoom"))
                {
                    if(templates.g.roomLists[this.gameObject.GetComponentsInChildren<RoomSpawner>()[i].GetComponentInParent<AddRoom>().lastIndex + i][j].gObject.CompareTag("KeyRoom") ||
                        templates.g.roomLists[this.gameObject.GetComponentsInChildren<RoomSpawner>()[i].GetComponentInParent<AddRoom>().lastIndex + i][j].gObject.CompareTag("CheckPointRoom") ||
                        templates.g.roomLists[this.gameObject.GetComponentsInChildren<RoomSpawner>()[i].GetComponentInParent<AddRoom>().lastIndex + i][j].gObject.CompareTag("TrapRoom") ||
                        templates.g.roomLists[this.gameObject.GetComponentsInChildren<RoomSpawner>()[i].GetComponentInParent<AddRoom>().lastIndex + i][j].gObject.CompareTag("FinishRoom"))
                    {
                        Destroy(templates.g.roomLists[this.gameObject.GetComponentsInChildren<RoomSpawner>()[i].GetComponentInParent<AddRoom>().lastIndex + i][j].gObject);
                        templates.SpecialRooms--;
                    }
                    else
                    {
                        Destroy(templates.g.roomLists[this.gameObject.GetComponentsInChildren<RoomSpawner>()[i].GetComponentInParent<AddRoom>().lastIndex + i][j].gObject);
                    }    
                }
            }
        }

        for (int i = 0; i < this.gameObject.GetComponentsInChildren<RoomSpawner>().Length - 1; i++)
        {
            templates.g.roomLists[this.gameObject.GetComponentsInChildren<RoomSpawner>()[i].GetComponentInParent<AddRoom>().lastIndex + i].RemoveRange(0, templates.g.roomLists[this.gameObject.GetComponentsInChildren<RoomSpawner>()[i].GetComponentInParent<AddRoom>().lastIndex + i].Count);
            Debug.Log(isFirstRoomsLength - i);
            templates.isFirstRoomsGenerated.RemoveAt(isFirstRoomsLength - i);
        }
        Debug.Log(templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Count);
        invokeSpawners();
    }

    private void invokeSpawners()
    {
        foreach (RoomSpawner rs in this.gameObject.GetComponentsInChildren<RoomSpawner>())
        {
            if(rs != occupiedSpawner)
            {
                rs.spawnedRoomsIndex.RemoveRange(0,rs.spawnedRoomsIndex.Count);
                templates.isFinishRoomSpawned = false;
                templates.isKeyRoomSpawned = false;
                templates.TrapRoomCounter = 0;
                templates.isCheckPointRoomSpawned = false;
                rs.isSpaceCheckedForSpecialRoom = false;
                rs.isEnoughSpaceForSpecialRoom = false;
                rs.isSpaceCheckedForRoom = false;
                rs.isEnoughSpaceForRoom = false;
                rs.isStepBack = false;
                rs.isSpawned = false;
                rs.Invoke("Spawn", 0.5f);
            }
            
        }
        enabled = false;
    }
}
