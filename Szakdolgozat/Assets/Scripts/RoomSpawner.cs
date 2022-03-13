using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    // 1 --> Alsó ajtó kell RoomSpawnPoint
    // 2 --> felsõ ajtó kell RoomSpawnPoint (2)
    // 3 --> bal aljtó kell RoomSpawnPoint (3)
    // 4 --> jobb ajtó kell RoomSpawnPoint (1)

    private RoomTemplates templates;
    private int rand;
    public int ExtraRoomQuantity = 0;

    [SerializeField] LayerMask groundMask;

    public NavMeshSurface surface;

    [Header("Bools")]
    public bool isSpawned = false;
    public bool isEnoughSpace = false;
    public bool isSpaceChecked = false;

    [Header("Gameobjects")]
    public GameObject nextRoom;
    public GameObject prevRoom;
    public GameObject spaceChecktemp;

    [Header("Delay for destroying used spawnpoints")]
    public float waitTime = 1000f;

    void Start()
    {          
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        //if(templates.isFirstRoomsGenerated.Count == 4)
        //{
        //    prevRoom = templates.g.roomLists[this.GetComponentInParent<AddRoom>().lastIndex][templates.g.roomLists[this.GetComponentInParent<AddRoom>().lastIndex].Count - 1].gObject;
        //    RoomSpawner[] roomSpawner = prevRoom.GetComponentsInChildren<RoomSpawner>();
        //    foreach (RoomSpawner rs in roomSpawner)
        //    {
        //        if (rs.nextRoom = this.GetComponentInParent<AddRoom>().gameObject)
        //        {
        //            ExtraRoomQuantity = rs.ExtraRoomQuantity;
        //        }
        //    }
        //}
        Invoke("Spawn", 0.5f);
        //Destroy(gameObject, waitTime);  
    }

    public void Spawn()
    {
        if (isSpawned == false)
        {
            if(templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Count == templates.RoomQuantity)
            {
                //if (isSpaceChecked == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.x < this.transform.position.x)
                //    nextRoom = Instantiate(templates.spaceCheck, new Vector3(this.transform.position.x + 1.05f, this.transform.position.y, this.transform.position.z), new Quaternion(0, 0, 0, 0));
                //else if (isSpaceChecked == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.x > this.transform.position.x)
                //    nextRoom = Instantiate(templates.spaceCheck, new Vector3(this.transform.position.x - 1.05f, this.transform.position.y, this.transform.position.z), new Quaternion(0,180f,0,0));
                //else if (isSpaceChecked == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.z < this.transform.position.z)
                //    nextRoom = Instantiate(templates.spaceCheck, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1.05f), this.gameObject.transform.rotation * Quaternion.Euler(0, 270, 0));
                //else if (isSpaceChecked == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.z > this.transform.position.z)
                //    nextRoom = Instantiate(templates.spaceCheck, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 1.05f), this.gameObject.transform.rotation * Quaternion.Euler(0, 90, 0));
                EnoughShapce();
            }

            if (isEnoughSpace == true && templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Count == templates.RoomQuantity + ExtraRoomQuantity)
            {
                if(templates.stageCounter == 1 && templates.TrapRoomCounter != 2)
                {
                    SpecialRoomSpawn(templates.TrapRoom);
                    templates.TrapRoomCounter++;
                }
                else if((templates.stageCounter == 2 || templates.stageCounter == 3) && templates.TrapRoomCounter == 0)
                {
                    SpecialRoomSpawn(templates.TrapRoom);
                    templates.TrapRoomCounter++;
                }
                else if(templates.isKeyRoomSpawned == false)
                {
                    SpecialRoomSpawn(templates.KeyRoom);
                    templates.isKeyRoomSpawned = true;
                }
                else if(templates.isKeyRoomSpawned == true)
                {
                    if(templates.stageCounter == 1 && templates.TrapRoomCounter == 2)
                    {
                        templates.stageCounter++;
                        SpecialRoomSpawn(templates.CheckPointRoom);
                        templates.isCheckPointRoomSpawned = true;
                        //nextRoom.GetComponent<AddRoom>().stage = templates.stageCounter;
                    }
                    else if (templates.stageCounter == 2 && templates.TrapRoomCounter == 1)
                    {
                        templates.stageCounter++;
                        SpecialRoomSpawn(templates.CheckPointRoom);
                        templates.isCheckPointRoomSpawned = true;
                        //nextRoom.GetComponent<AddRoom>().stage = templates.stageCounter;
                    }
                    else if(templates.stageCounter == 3 && templates.TrapRoomCounter == 1)
                    {
                        SpecialRoomSpawn(templates.FinishRoom);
                        templates.isFinishRoomSpawned = true;
                        //nextRoom.GetComponent<AddRoom>().stage = templates.stageCounter;
                    }
                }
            }

            if (openingDirection == 1)
            {
                //SelectRoom(templates.bottomRooms);
                SpecialRoomRotationTest(templates.bottomRooms);
            }
            else if (openingDirection == 2)
            {
                //SelectRoom(templates.topRooms);
                SpecialRoomRotationTest(templates.topRooms);
            }
            else if (openingDirection == 3)
            {
                //SelectRoom(templates.leftRooms);
                SpecialRoomRotationTest(templates.leftRooms);
            }
            else if (openingDirection == 4)
            {
                //SelectRoom(templates.rightRooms);
                SpecialRoomRotationTest(templates.rightRooms);
            }
        }

        isSpawned = true;
    }

    private void SelectRoom(GameObject[] roomList)
    {
        if (isEnoughSpace == false && templates.SpecialRoomSpawn == false && templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Count < templates.RoomQuantity + ExtraRoomQuantity)
        {
            rand = Random.Range(0, roomList.Length - 1);
            nextRoom = Instantiate(roomList[rand], transform.position, roomList[rand].transform.rotation, templates.Parent.transform);
            CreateNode();
        }
    }

    private void SpecialRoomRotationTest(GameObject[] roomList)
    {
        if (isEnoughSpace == false && templates.SpecialRoomSpawn == false && templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Count < templates.RoomQuantity)
        {
            nextRoom = Instantiate(roomList[0], transform.position, roomList[0].transform.rotation, templates.Parent.transform);
            CreateNode();
            nextRoom.GetComponent<AddRoom>().stage = templates.stageCounter;
        }
    }

    private void SpecialRoomSpawn(GameObject specialRoom)
    {
        if(isEnoughSpace == true && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("CheckPointRoom"))
        {
            if(!specialRoom.CompareTag("CheckPointRoom"))
            {
                if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.x < this.transform.position.x)
                {
                    nextRoom = Instantiate(specialRoom, new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z), new Quaternion(0, 0, 0, 0), templates.Parent.transform);
                }               
                else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.x > this.transform.position.x)
                {
                    nextRoom = Instantiate(specialRoom, new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z), new Quaternion(0, 180, 0, 0), templates.Parent.transform); //180
                }               
                else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.z < this.transform.position.z)
                {
                    nextRoom = Instantiate(specialRoom, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1), this.gameObject.transform.rotation * Quaternion.Euler(0, 270, 0), templates.Parent.transform); //270
                }      
                else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.z > this.transform.position.z)
                {
                    nextRoom = Instantiate(specialRoom, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 1), this.gameObject.transform.rotation * Quaternion.Euler(0, 90, 0), templates.Parent.transform); //90f
                }
            }
            else
            {
                if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.x < this.transform.position.x)
                {
                    nextRoom = Instantiate(specialRoom, new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z), new Quaternion(0, 0, 0, 0), templates.Parent.transform);
                }
                else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.x > this.transform.position.x)
                {
                    nextRoom = Instantiate(specialRoom, new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z), new Quaternion(0, 0, 0, 0), templates.Parent.transform); //180
                }
                else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.z < this.transform.position.z)
                {
                    nextRoom = Instantiate(specialRoom, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1), this.gameObject.transform.rotation * Quaternion.Euler(0, 0, 0), templates.Parent.transform); //270
                }
                else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.z > this.transform.position.z)
                {
                    nextRoom = Instantiate(specialRoom, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 1), this.gameObject.transform.rotation * Quaternion.Euler(0, 0, 0), templates.Parent.transform); //90f
                }
            }

            nextRoom.GetComponent<AddRoom>().stage = templates.stageCounter;
            CreateNode();
            templates.SpecialRooms++;
            if (templates.SpecialRooms == 4 || templates.SpecialRooms == 7)
                templates.RoomQuantity = templates.RoomQuantity + 10;
        }
    }

    private void CreateNode()
    {
        nextRoom.GetComponent<AddRoom>().lastIndex = this.gameObject.GetComponentInParent<AddRoom>().lastIndex;
        nextRoom.GetComponent<AddRoom>().newRoom = new Node(new Vector3(nextRoom.transform.position.x, nextRoom.transform.position.y, nextRoom.transform.position.z),
            nextRoom.gameObject, this.gameObject.GetComponentInParent<AddRoom>().newRoom,
            this.gameObject.GetComponentInParent<AddRoom>().lastIndex);
    }

    private void EnoughShapce()
    {
        if (isSpaceChecked == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.x < this.transform.position.x)
            isEnoughSpace = !Physics.CheckSphere(new Vector3(this.transform.position.x + 1f, this.transform.position.y, this.transform.position.z), 1f, groundMask);
        else if (isSpaceChecked == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.x > this.transform.position.x)
            isEnoughSpace = !Physics.CheckSphere(new Vector3(this.transform.position.x - 1f, this.transform.position.y, this.transform.position.z), 1f, groundMask);
        else if (isSpaceChecked == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.z < this.transform.position.z)
            isEnoughSpace = !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1f), 1f, groundMask);
        else if (isSpaceChecked == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.z > this.transform.position.z)
            isEnoughSpace = !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 1f), 1f, groundMask);
        if (isEnoughSpace == false)
        {
            templates.RoomQuantity += 1;
            Invoke("SelectRoom", 0.5f);
        }
            
        isSpaceChecked = true;
    }

    private void StepBack(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Ground") && this.nextRoom == null)
        {
            GameObject lastAddedRoom = templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex][templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Count - 1].gObject;
            RoomSpawner[] roomSpawner = lastAddedRoom.GetComponentsInChildren<RoomSpawner>();
            foreach (RoomSpawner rs in roomSpawner)
            {
                if (rs.nextRoom == null)
                    rs.isSpawned = true;
                else
                {
                    Destroy(rs.nextRoom);
                    rs.isSpawned = false;
                }
            }
            foreach (RoomSpawner rs in roomSpawner)
            {
                if (rs.isSpawned == false)
                    rs.Invoke("Spawn", 0.5f);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Guard"))
        {
            //if(this.gameObject.GetComponentInParent<AddRoom>().gameObject.name.Contains("SlopeUp"))
            //{
            //    StepBack(other);
            //}

            //if (other.gameObject.GetComponentInParent<AddRoom>().gameObject.name.Contains("SlopeUp"))
            //{
            //    StepBack(other);
            //}

            if (other.CompareTag("Wall") || other.gameObject.GetComponentInParent<AddRoom>().gameObject.name.Contains("SlopeUp") || this.gameObject.GetComponentInParent<AddRoom>().gameObject.name.Contains("SlopeUp"))
            {
                 StepBack(other);
            }
        }
        

        isSpawned = true;
    }
}
