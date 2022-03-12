using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCheck : MonoBehaviour
{
    public int listIndex;
    public GameObject lastAddedRoom;
    private RoomTemplates templates;
    private RoomSpawner correctRoomSpawner;
    public int CollideCount = 0;
    public List<Collider> Colliders = new List<Collider>();

    [SerializeField] LayerMask groundMask;

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("findSpawnPoint", 0.1f);
    }

    public bool isSpawnable()
    {
        if (Colliders.Count == 0)
        {
            Debug.Log("----------Number of collisions == " + Colliders.Count);
            return true;
        } 
        else
        {
            Debug.Log("----------Number of collisions == " + Colliders.Count);
            return false;
        }
    }

    public void findSpawnPoint()
    {
        for (int i = 0; i < templates.g.roomLists.Count; i++)
        {
            for (int j = 0; j < templates.g.roomLists[i].Count; j++)
            {
                if (templates.g.roomLists[i][j].gObject.GetComponentInChildren<RoomSpawner>().nextRoom.gameObject == this.gameObject)
                {
                    lastAddedRoom = templates.g.roomLists[i][j].gObject;
                    RoomSpawner[] roomSpawner = lastAddedRoom.GetComponentsInChildren<RoomSpawner>();
                    foreach (RoomSpawner rs in roomSpawner)
                    {
                        if (rs.nextRoom != null && rs.nextRoom.gameObject.tag == "SpaceCheck")
                        {
                            rs.isEnoughSpace = Physics.CheckSphere(this.transform.position, 2f, groundMask);
                            rs.isSpawned = false;
                            if(rs.isEnoughSpace == false)
                                rs.ExtraRoomQuantity += 1;
                            rs.Spawn();
                            this.gameObject.GetComponent<SpaceCheck>().enabled = false;
                        }
                    }
                    break;
                }
            }
        }
    }

    private void SpawnCall()
    {
        Debug.Log("isEnoughSpace = " + correctRoomSpawner.isEnoughSpace + " isSpawned = " + correctRoomSpawner.isSpawned + " GameObject: " + correctRoomSpawner.GetComponentInParent<AddRoom>().gameObject);
        correctRoomSpawner.Spawn();
    }

    public void InvokeSpawnMethod()
    {
        if(isSpawnable() == true)
        {
            //findSpawnPoint(isSpawnable());
        }
        else
        {
            //findSpawnPoint(isSpawnable());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Colliders.Add(other);
        //if(!other.CompareTag("RoomSpawnPoint"))
        //{
        //    findSpawnPoint(false);
        //}
    }
}
