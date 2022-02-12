using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    private SpawnRoomSpawn SRS;
    private RoomTemplates templates;
    public Node newRoom;
    public int lastIndex; 

    //0 -> Zold oldal
    //1 -> Sarga oldal
    //2 -> Kek oldal
    //3 -> Piros oldal

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();        
        SRS = GameObject.FindGameObjectWithTag("Rooms").GetComponent<SpawnRoomSpawn>();
        Invoke("addRoom", 0.1f);
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
            Debug.Log("--------Spawn room ág---------------");
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
                    Debug.Log("------------Kisebb mint 4 ág-----------");
                    break;
                }
                
            }
        }
        else
        {
            templates.g.addEdge(newRoom, lastIndex);
            Debug.Log("------------Else ág-----------");
            for (int i = 0; i < templates.isFirstRoomsGenerated.Count; i++)
            {
                Debug.Log(templates.isFirstRoomsGenerated[i] + ", " + i);
            }
            Debug.Log("this node: " + newRoom.gObject + "prevnode: "  + newRoom.prevNode.gObject);
        }
        Debug.Log(templates.g.toStr());
        
        //lastIndex = templates.lastIndex;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("RoomSpawnPoint") && !other.CompareTag("Player"))
        {
            if (other.CompareTag("Wall") || other.CompareTag("Ground"))
            {
                GameObject prevRoom = templates.g.roomLists[lastIndex][templates.g.roomLists[lastIndex].Count - 1].gObject;
                RoomSpawner[] roomSpawner = prevRoom.GetComponentsInChildren<RoomSpawner>();
                foreach (RoomSpawner rs in roomSpawner)
                {
                    if (rs.nextRoom != gameObject)
                        rs.isSpawned = true;
                    else
                        rs.isSpawned = false;
                }
                Destroy(this.gameObject);
                foreach (RoomSpawner rs in roomSpawner)
                {
                    if (rs.isSpawned == false)
                        rs.Spawn();
                }
            }
        }
    }
}
