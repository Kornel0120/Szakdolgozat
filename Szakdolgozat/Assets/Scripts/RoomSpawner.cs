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

    public int Index;


    private RoomTemplates templates;
    private int rand;
    public bool isSpawned = false;
    public bool isSpawnRoomSpawned = false;
    public int roomsCount = 0;

    public float waitTime = 10f;

    void Start()
    {          
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f); 
        Destroy(gameObject, waitTime);  
    }

    void Spawn()
    {
        
        if (isSpawned == false /*&& templates.rooms.Count != roomsCount*/)
        {      
            if (openingDirection == 1)
            {
                rand = Random.Range(0, templates.bottomRooms.Length - 1);
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
                roomsCount++;
            }
            else if (openingDirection == 2)
            {
                rand = Random.Range(0, templates.topRooms.Length - 1);
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
                roomsCount++;
            }
            else if (openingDirection == 3)
            {
                rand = Random.Range(0, templates.leftRooms.Length - 1);
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
                roomsCount++;
            }
            else if (openingDirection == 4)
            {
                rand = Random.Range(0, templates.rightRooms.Length - 1);
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
                roomsCount++;
            }               
        }
        isSpawned = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoomSpawnPoint"))
        {
            if (other.GetComponent<RoomSpawner>().isSpawned == false && isSpawned == true)
            {
                //GameObject last = templates.g.FindLast(this.gameObject.GetComponent<RoomSpawner>().Index);
                //Destroy(last);
                //templates.g.deleteLast(this.gameObject.GetComponent<RoomSpawner>().Index);
                //last = templates.g.FindLast(this.gameObject.GetComponent<RoomSpawner>().Index);
                isSpawned = false;
            }
        }
        else
        {
            isSpawned = true; //ez allitja meg a szobak egymasba spawnolasat.
        }

        

    }
}
