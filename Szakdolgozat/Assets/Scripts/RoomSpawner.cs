using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    // 1 --> Alsó ajtó kell RoomSpawnPoint
    // 2 --> felsõ ajtó kell RoomSpawnPoint (2)
    // 3 --> bal aljtó kell RoomSpawnPoint (3)
    // 4 --> jobb ajtó kell RoomSpawnPoint (1)

    private RoomTemplates templates;
    private int rand;

    [Header("Bools")]
    public bool isSpawned = false;
    public bool isEnoughSpace = false;
    public bool isSpaceChecked = false;

    [Header("Gameobjects")]
    public GameObject nextRoom;
    public GameObject spaceChecktemp;

    [Header("Delay for destroying used spawnpoints")]
    public float waitTime = 1000f;

    void Start()
    {          
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.5f); 
        //Destroy(gameObject, waitTime);  
    }

    public void Spawn()
    {
        if (isSpawned == false)
        {
            if (templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Count == 15)
            {
                if(templates.isCheckPointRoomSpawned == false)
                {
                    SpecialRoomSpawn(templates.CheckPointRoom);
                    templates.isCheckPointRoomSpawned = true;
                }
                else if(templates.isKeyRoomSpawned == false)
                {
                    SpecialRoomSpawn(templates.KeyRoom);
                    templates.isKeyRoomSpawned = true;
                }
                else
                {
                    SpecialRoomSpawn(templates.TrapRoom);
                }
            }

            if (openingDirection == 1)
            {
                SelectRoom(templates.bottomRooms);
            }
            else if (openingDirection == 2)
            {
                SelectRoom(templates.topRooms);
            }
            else if (openingDirection == 3)
            {
                SelectRoom(templates.leftRooms);
            }
            else if (openingDirection == 4)
            {
                SelectRoom(templates.rightRooms);
            }
        }
        isSpawned = true;
    }

    private void SelectRoom(GameObject[] roomList)
    {
        if (templates.SpecialRoomSpawn == false && templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Count < 15)
        {
            rand = Random.Range(0, roomList.Length - 1);
            nextRoom = Instantiate(roomList[rand], transform.position, roomList[rand].transform.rotation);
            nextRoom.GetComponent<AddRoom>().lastIndex = this.gameObject.GetComponentInParent<AddRoom>().lastIndex;
            nextRoom.GetComponent<AddRoom>().newRoom = new Node(new Vector3(nextRoom.transform.position.x, nextRoom.transform.position.y, nextRoom.transform.position.z), nextRoom.gameObject, this.gameObject.GetComponentInParent<AddRoom>().newRoom, this.gameObject.GetComponentInParent<AddRoom>().lastIndex);
        }
    }

    private void SpecialRoomSpawn(GameObject specialRoom)
    {
        if (templates.prevRoom.transform.position.x < this.transform.position.x)
            Instantiate(specialRoom, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), specialRoom.transform.rotation);
        else if (templates.prevRoom.transform.position.x > this.transform.position.x)
            Instantiate(specialRoom, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), specialRoom.transform.rotation);
        else if (templates.transform.position.z < this.transform.position.z)
            Instantiate(specialRoom, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), specialRoom.transform.rotation);
        else if (templates.transform.position.z > this.transform.position.z)
            Instantiate(specialRoom, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), specialRoom.transform.rotation);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wall"))
        {
            GameObject lastAddedRoom = templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex][templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Count - 1].gObject;
            RoomSpawner[] roomSpawner = lastAddedRoom.GetComponentsInChildren<RoomSpawner>();
            foreach (RoomSpawner rs in roomSpawner)
            {
                if (rs.nextRoom == null)
                    rs.isSpawned = true;
                else
                {
                    rs.isSpawned = false;
                    Destroy(rs.nextRoom);
                }       
            }
            foreach (RoomSpawner rs in roomSpawner)
            {
                if (rs.isSpawned == false)
                    rs.Spawn();
            }
        }
        //if (other.CompareTag("RoomSpawnPoint"))
        //{
        //    if (other.GetComponent<RoomSpawner>().isSpawned == false && isSpawned == true)
        //    {
        //        isSpawned = true;
        //    }
        //}
        //else
        //{
        //    isSpawned = true; //ez allitja meg a szobak egymasba spawnolasat.
        //}
        isSpawned = true;
        //if(other.gameObject.tag == "SpaceCheck" && templates.g.roomLists[0].Count >= 25)
        //{
        //    isEnoughSpace = false;
        //    Debug.Log("!!!!!!!!!SpaceCheck!!!!!!!! " + isEnoughSpace);
        //    Destroy(spaceChecktemp);
        //    isSpawned = false;
        //    Invoke("Spawn", 0.5f);
        //}
        //else if(templates.g.roomLists[0].Count >= 25)
        //{
        //    isEnoughSpace = true;
        //    Debug.Log("!!!!!!!!!SpaceCheck!!!!!!!! " + isEnoughSpace);
        //    //Destroy(spaceChecktemp);
        //}
        //isSpaceChecked = true;
    }
}
