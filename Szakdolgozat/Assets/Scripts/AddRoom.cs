using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    private SpawnRoomSpawn SRS;
    private RoomTemplates templates;
    public Node newRoom;
    public int lastIndex;
    //private List<GameObject> checkedRooms = new List<GameObject>();
    public byte stage = 0;
    public GameObject nextRoom;

    //0 -> Zold oldal
    //1 -> Sarga oldal
    //2 -> Kek oldal
    //3 -> Piros oldal

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();        
        SRS = GameObject.FindGameObjectWithTag("Rooms").GetComponent<SpawnRoomSpawn>();
        Invoke("addRoom", 0.0f); //0.1f
    }

    void addRoom()
    {
        if (SRS.isSpawnRoomSpawned == false)
        {
            templates.g.rootNode = new Node(new Vector3(templates.SpawnRoom.transform.position.x,
                                                 templates.SpawnRoom.transform.position.y,
                                                 templates.SpawnRoom.transform.position.z),
                                                 templates.SpawnRoom);
            SRS.isSpawnRoomSpawned = true;
        }
        else if (templates.isFirstRoomsGenerated.Count < 4)
        {
            for (int i = 0; i < templates.g.roomLists.Count; i++)
            {
                if (templates.g.roomLists[i].Count == 0)
                {
                    lastIndex = i;
                    newRoom = new Node(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.gameObject, templates.g.rootNode, lastIndex);
                    templates.isFirstRoomsGenerated.Add(true);
                    templates.g.addEdge(newRoom, lastIndex);
                    break;
                }
                
            }
        }
        else if(templates.stageCounter == 2 && templates.isFirstRoomsGenerated.Count < 7)
        {
            if(!this.gameObject.CompareTag("KeyRoom") && !this.gameObject.CompareTag("TrapRoom"))
            {
                for (int i = 0; i < templates.g.roomLists.Count; i++)
                {
                    if (templates.g.roomLists[i].Count == 0)
                    {
                        lastIndex = i;
                        newRoom = new Node(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.gameObject, templates.g.rootNode, lastIndex);
                        templates.isFirstRoomsGenerated.Add(true);
                        templates.g.addEdge(newRoom, lastIndex);
                        break;
                    }

                }
            }
        }
        else if (templates.stageCounter == 3 && templates.isFirstRoomsGenerated.Count < 10)
        {
            if (!this.gameObject.CompareTag("KeyRoom") && !this.gameObject.CompareTag("TrapRoom"))
            {
                for (int i = 0; i < templates.g.roomLists.Count; i++)
                {
                    if (templates.g.roomLists[i].Count == 0)
                    {
                        lastIndex = i;
                        newRoom = new Node(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.gameObject, templates.g.rootNode, lastIndex);
                        templates.isFirstRoomsGenerated.Add(true);
                        templates.g.addEdge(newRoom, lastIndex);
                        break;
                    }

                }
            }
        }
        else
        {
            templates.g.addEdge(newRoom, lastIndex);
        }

        if(templates.stageCounter == 2 && templates.resetCounter == 0)
        {
            templates.TrapRoomCounter = 0;
            templates.isKeyRoomSpawned = false;
            templates.isCheckPointRoomSpawned = false;

            templates.resetCounter++;
        }
        else if(templates.stageCounter == 3 && templates.resetCounter == 1)
        {
            templates.TrapRoomCounter = 0;
            templates.isKeyRoomSpawned = false;
            templates.isCheckPointRoomSpawned = false;

            templates.resetCounter++;
        }
    }
}
