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
    private int rand = -1;
    private int deletedRoomIndex = -1;
    public int ExtraRoomQuantity = 0;

    public List<int> spawnedRoomsIndex = new List<int>();

    [SerializeField] LayerMask groundMask;

    //public NavMeshSurface surface;

    [Header("Bools")]
    public bool isSpawned = false;
    public bool isEnoughSpaceForSpecialRoom = false;
    public bool isEnoughSpaceForRoom = false;
    public bool isSpaceCheckedForSpecialRoom = false;
    public bool isSpaceCheckedForRoom = false;
    public bool isStepBack = false;

    [Header("Gameobjects")]
    public GameObject nextRoom;
    public GameObject prevRoom = null;

    [Header("Delay for destroying used spawnpoints")]
    public float waitTime = 1000f;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.0f); //0.5f 
    }

    public void Spawn()
    {
        EnoughSpaceForRoom();

        if (isEnoughSpaceForRoom == true && isSpawned == false)
        {
            if(templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Count == templates.RoomQuantity && 
                (templates.isKeyRoomSpawned == true && templates.stageCounter == 1 && templates.TrapRoomCounter == 2 || templates.isKeyRoomSpawned == true && 
                (templates.stageCounter == 2 || templates.stageCounter == 3) && templates.TrapRoomCounter == 1))
            {
                EnoughShapceForSpecialRoom(5f, true);
            }
            else if (templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Count == templates.RoomQuantity)
            {
                EnoughShapceForSpecialRoom(1f, false);
            }

            if (isEnoughSpaceForSpecialRoom == true && templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Count == templates.RoomQuantity + ExtraRoomQuantity)
            {
                if (templates.stageCounter == 1 && templates.TrapRoomCounter != 2)
                {
                    SpecialRoomSpawn(templates.TrapRoom);
                    templates.TrapRoomCounter++;
                }
                else if ((templates.stageCounter == 2 || templates.stageCounter == 3) && templates.TrapRoomCounter == 0)
                {
                    SpecialRoomSpawn(templates.TrapRoom);
                    templates.TrapRoomCounter++;
                }
                else if (templates.isKeyRoomSpawned == false)
                {
                    SpecialRoomSpawn(templates.KeyRoom);
                    templates.isKeyRoomSpawned = true;
                }
                else if (templates.isKeyRoomSpawned == true)
                {
                    if (templates.stageCounter == 1 && templates.TrapRoomCounter == 2)
                    {
                        templates.stageCounter++;
                        SpecialRoomSpawn(templates.CheckPointRoom);
                        templates.isCheckPointRoomSpawned = true;
                    }
                    else if (templates.stageCounter == 2 && templates.TrapRoomCounter == 1)
                    {
                        templates.stageCounter++;
                        SpecialRoomSpawn(templates.CheckPointRoom);
                        templates.isCheckPointRoomSpawned = true;
                    }
                    else if (templates.stageCounter == 3 && templates.TrapRoomCounter == 1)
                    {
                        SpecialRoomSpawn(templates.FinishRoom);
                        templates.isFinishRoomSpawned = true;
                    }
                }
            }


            switch (openingDirection)
            {
                case 1:
                    SelectRoom(templates.bottomRooms);
                    break;
                case 2:
                    SelectRoom(templates.topRooms);
                    break;
                case 3:
                    SelectRoom(templates.leftRooms);
                    break;
                case 4:
                    SelectRoom(templates.rightRooms);
                    break;
            }
        }

        isSpawned = true;
        if (!this.GetComponentInParent<AddRoom>().gameObject.CompareTag("SpawnRoom") && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("CheckPointRoom")
            && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("KeyRoom") && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("TrapRoom"))
            this.GetComponentInParent<DeleteRoom>().Invoke("spaceCheck", 0.0f); //0.3f
    }

    private void SelectRoom(GameObject[] roomList)
    {
        if(!this.gameObject.GetComponentInParent<AddRoom>().gameObject.CompareTag("CheckPointRoom") && !this.gameObject.GetComponentInParent<AddRoom>().gameObject.CompareTag("SpawnRoom"))
        {
            GameObject room = this.gameObject.GetComponentInParent<AddRoom>().gameObject;
            RoomSpawner[] spawners = room.GetComponentsInChildren<RoomSpawner>();
            foreach (RoomSpawner rs in spawners)
            {
                if (rs.gameObject != this.gameObject)
                    Destroy(rs.gameObject);
            }
        }
        
        if (isEnoughSpaceForSpecialRoom == false && templates.SpecialRoomSpawn == false && templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Count < templates.RoomQuantity + ExtraRoomQuantity)
        {
            if(rand == -1)
            {
                rand = Random.Range(0, roomList.Length - 1);
            }  
            else
            {
                for (int i = 0; i < roomList.Length; i++)
                {
                    if (!spawnedRoomsIndex.Contains(i))
                    {
                        List<string> temp = new List<string>();
                        for (int j = 0; j < spawnedRoomsIndex.Count-1; j++)
                        {
                            temp.Add(roomList[spawnedRoomsIndex[j]].name);
                        }
                        if (!temp.Contains(roomList[i].gameObject.name))
                            rand = i;
                        else
                            spawnedRoomsIndex.Add(i);
                    }        
                }
            }

            if (!this.spawnedRoomsIndex.Contains(rand))
            {
                nextRoom = Instantiate(roomList[rand], transform.position, roomList[rand].transform.rotation, templates.Parent.transform);
                CreateNode(nextRoom);
                this.spawnedRoomsIndex.Add(rand);
                nextRoom.GetComponent<AddRoom>().stage = templates.stageCounter;
                foreach (RoomSpawner rs in nextRoom.GetComponentsInChildren<RoomSpawner>())
                {
                    rs.prevRoom = this.gameObject.GetComponentInParent<AddRoom>().gameObject;
                }
            }
            else if (this.spawnedRoomsIndex.Contains(rand) && this.spawnedRoomsIndex.Count < roomList.Length)
            {
                isSpawned = false;
                isEnoughSpaceForRoom = false;
                isSpaceCheckedForRoom = false;
                Invoke("Spawn", 0.0f); //0.5f
            }
            else if (this.spawnedRoomsIndex.Contains(rand) && this.spawnedRoomsIndex.Count >= roomList.Length)
            {
                StepBack();
            }
        }
    }

    private void SpecialRoomSpawn(GameObject specialRoom)
    {
        if (isEnoughSpaceForSpecialRoom == true && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("CheckPointRoom"))
        {
            if (!specialRoom.CompareTag("CheckPointRoom"))
            {
                if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.x < this.transform.position.x)
                    nextRoom = Instantiate(specialRoom, new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z), new Quaternion(0, 0, 0, 0), templates.Parent.transform);
                else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.x > this.transform.position.x)
                    nextRoom = Instantiate(specialRoom, new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z), new Quaternion(0, 180, 0, 0), templates.Parent.transform); //180
                else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.z < this.transform.position.z)
                    nextRoom = Instantiate(specialRoom, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1), this.gameObject.transform.rotation * Quaternion.Euler(0, 270, 0), templates.Parent.transform); //270
                else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.z > this.transform.position.z)
                    nextRoom = Instantiate(specialRoom, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 1), this.gameObject.transform.rotation * Quaternion.Euler(0, 90, 0), templates.Parent.transform); //90f
            }
            else
            {
                if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.x < this.transform.position.x)
                    nextRoom = Instantiate(specialRoom, new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z), new Quaternion(0, 0, 0, 0), templates.Parent.transform);
                else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.x > this.transform.position.x)
                    nextRoom = Instantiate(specialRoom, new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z), new Quaternion(0, 0, 0, 0), templates.Parent.transform); //180
                else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.z < this.transform.position.z)
                    nextRoom = Instantiate(specialRoom, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1), this.gameObject.transform.rotation * Quaternion.Euler(0, 0, 0), templates.Parent.transform); //270
                else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.z > this.transform.position.z) 
                    nextRoom = Instantiate(specialRoom, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 1), this.gameObject.transform.rotation * Quaternion.Euler(0, 0, 0), templates.Parent.transform); //90f
            }

            nextRoom.GetComponent<AddRoom>().stage = templates.stageCounter;
            CreateNode(nextRoom);
            templates.SpecialRooms++;
            if (templates.SpecialRooms == 4 || templates.SpecialRooms == 7)
                templates.RoomQuantity = templates.RoomQuantity + 10;
        }
    }

    private void CreateNode(GameObject nextGameObject)
    {
        nextGameObject.GetComponent<AddRoom>().lastIndex = this.gameObject.GetComponentInParent<AddRoom>().lastIndex;
        if(this.gameObject.GetComponentInParent<AddRoom>().gameObject.CompareTag("SpawnRoom"))
        {
            if(deletedRoomIndex == -1)
                nextGameObject.GetComponent<AddRoom>().newRoom = new Node(new Vector3(nextGameObject.transform.position.x, nextGameObject.transform.position.y, nextGameObject.transform.position.z),
                                                               nextGameObject, this.templates.g.rootNode,
                                                               this.gameObject.GetComponentInParent<AddRoom>().lastIndex);
            else
                nextGameObject.GetComponent<AddRoom>().newRoom = new Node(new Vector3(nextGameObject.transform.position.x, nextGameObject.transform.position.y, nextGameObject.transform.position.z),
                                                               nextGameObject, this.templates.g.rootNode,
                                                               deletedRoomIndex);
        }
        else
        {
            if (deletedRoomIndex == -1)
                nextGameObject.GetComponent<AddRoom>().newRoom = new Node(new Vector3(nextGameObject.transform.position.x, nextGameObject.transform.position.y, nextGameObject.transform.position.z),
                                                                nextGameObject, this.gameObject.GetComponentInParent<AddRoom>().newRoom,
                                                                this.gameObject.GetComponentInParent<AddRoom>().lastIndex);
            else
                nextGameObject.GetComponent<AddRoom>().newRoom = new Node(new Vector3(nextGameObject.transform.position.x, nextGameObject.transform.position.y, nextGameObject.transform.position.z),
                                                                nextGameObject, this.gameObject.GetComponentInParent<AddRoom>().newRoom,
                                                                deletedRoomIndex);
        }
        
        this.GetComponentInParent<AddRoom>().nextRoom = nextGameObject;
    }

    public Vector3 temp;

    private void EnoughSpaceForRoom()
    {
        if(isSpaceCheckedForRoom == false)
        {
            if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.x < this.transform.position.x)
            {
                isEnoughSpaceForRoom = CheckSphere(0.25f, 0);
                temp = new Vector3(this.transform.position.x + 0.15f, this.transform.position.y + 0.15f, this.transform.position.z);
            }
            else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.x > this.transform.position.x)
            {
                isEnoughSpaceForRoom = CheckSphere(-0.25f, 0);
                temp = new Vector3(this.transform.position.x - 0.15f, this.transform.position.y + 0.15f, this.transform.position.z);
            }
            else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.z < this.transform.position.z)
            {
                isEnoughSpaceForRoom = CheckSphere(0, 0.25f);
                temp = new Vector3(this.transform.position.x, this.transform.position.y + 0.15f, this.transform.position.z + 0.15f);
            }
            else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.z > this.transform.position.z)
            {
                isEnoughSpaceForRoom = CheckSphere(0, -0.25f);
                temp = new Vector3(this.transform.position.x, this.transform.position.y + 0.15f, this.transform.position.z - 0.15f);
            }
        }

        if (!this.GetComponentInParent<AddRoom>().gameObject.CompareTag("SpawnRoom") && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("CheckPointRoom")
            && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("KeyRoom") && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("TrapRoom"))
            this.GetComponentInParent<DeleteRoom>().isEnoughSpace.Add(isEnoughSpaceForRoom);

        isSpaceCheckedForRoom = true;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(temp, 0.45f);
    }

    private void EnoughShapceForSpecialRoom(float radius, bool isCheckPointRoom)
    {
        if(isCheckPointRoom == false)
        {
            if(isSpaceCheckedForSpecialRoom == false)
            {
                if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.x < this.transform.position.x)
                    isEnoughSpaceForSpecialRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x + radius, this.transform.position.y, this.transform.position.z), radius, groundMask);
                else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.x > this.transform.position.x)
                    isEnoughSpaceForSpecialRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x - radius, this.transform.position.y, this.transform.position.z), radius, groundMask);
                else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.z < this.transform.position.z)
                    isEnoughSpaceForSpecialRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + radius), radius, groundMask);
                else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.z > this.transform.position.z)
                    isEnoughSpaceForSpecialRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - radius), radius, groundMask);
            }
        }
        else
        {
            if (isSpaceCheckedForSpecialRoom == false)
            {
                if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.x < this.transform.position.x)
                    isEnoughSpaceForSpecialRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x + radius, this.transform.position.y, this.transform.position.z), radius, groundMask) &&
                        !Physics.CheckSphere(new Vector3(this.transform.position.x + radius, this.transform.position.y, this.transform.position.z + 2), radius, groundMask) &&
                        !Physics.CheckSphere(new Vector3(this.transform.position.x + radius, this.transform.position.y, this.transform.position.z - 2), radius, groundMask) &&
                        !Physics.CheckSphere(new Vector3(this.transform.position.x + radius + 1, this.transform.position.y, this.transform.position.z), radius, groundMask);
                else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.x > this.transform.position.x)
                    isEnoughSpaceForSpecialRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x - radius, this.transform.position.y, this.transform.position.z), radius, groundMask) &&
                        !Physics.CheckSphere(new Vector3(this.transform.position.x - radius, this.transform.position.y, this.transform.position.z + 2), radius, groundMask) &&
                        !Physics.CheckSphere(new Vector3(this.transform.position.x - radius, this.transform.position.y, this.transform.position.z - 2), radius, groundMask) &&
                        !Physics.CheckSphere(new Vector3(this.transform.position.x - radius - 1, this.transform.position.y, this.transform.position.z), radius, groundMask);
                else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.z < this.transform.position.z)
                    isEnoughSpaceForSpecialRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + radius), radius, groundMask) &&
                        !Physics.CheckSphere(new Vector3(this.transform.position.x + 2, this.transform.position.y, this.transform.position.z + radius), radius, groundMask) &&
                        !Physics.CheckSphere(new Vector3(this.transform.position.x - 2, this.transform.position.y, this.transform.position.z + radius), radius, groundMask) &&
                        !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + radius + 1), radius, groundMask);
                else if (this.GetComponentInParent<AddRoom>().gameObject.transform.position.z > this.transform.position.z)
                    isEnoughSpaceForSpecialRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - radius), radius, groundMask) &&
                        !Physics.CheckSphere(new Vector3(this.transform.position.x + 2, this.transform.position.y, this.transform.position.z - radius), radius, groundMask) &&
                        !Physics.CheckSphere(new Vector3(this.transform.position.x - 2, this.transform.position.y, this.transform.position.z - radius), radius, groundMask) &&
                        !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - radius - 1), radius, groundMask);
            }
        }

        if (isEnoughSpaceForSpecialRoom == false || this.GetComponentInParent<AddRoom>().name.Contains("Slope"))
        {
            isEnoughSpaceForSpecialRoom = false;
            isEnoughSpaceForSpecialRoom = false;
            templates.RoomQuantity += 1;
            Invoke("Spawn", 0.0f); //0.5f
        }

        isSpaceCheckedForSpecialRoom = true;
    }

    private bool CheckSphere(float offsetOnXCord, float offsetOnZCord)
    {
        return !Physics.CheckSphere(new Vector3(this.transform.position.x + offsetOnXCord, this.transform.position.y + 0.15f, this.transform.position.z + offsetOnZCord), 0.5f, groundMask)
                && !Physics.CheckSphere(new Vector3(this.transform.position.x + offsetOnXCord, this.transform.position.y + 1.15f, this.transform.position.z + offsetOnZCord), 0.5f, groundMask)
                && !Physics.CheckSphere(new Vector3(this.transform.position.x + offsetOnXCord, this.transform.position.y - 1.15f, this.transform.position.z + offsetOnZCord), 0.5f, groundMask);
    }

    public void DeleteNextRoom()
    {
        if (this.GetComponentInParent<AddRoom>().gameObject.CompareTag("CheckPointRoom") || this.GetComponentInParent<AddRoom>().gameObject.CompareTag("SpawnRoom"))
            deletedRoomIndex = this.nextRoom.GetComponent<AddRoom>().newRoom.listIndex;

        if (!this.GetComponentInParent<AddRoom>().gameObject.CompareTag("SpawnRoom") && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("CheckPointRoom")
            && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("KeyRoom") && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("TrapRoom"))
        {
            this.GetComponentInParent<DeleteRoom>().isEnoughSpace.Clear();
        }
            
        if (isStepBack == true)
        {
            templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Remove(this.nextRoom.GetComponent<AddRoom>().newRoom);
            Destroy(this.nextRoom);
            isSpawned = false;
            isStepBack = false;
            Invoke("Spawn", 0.0f); //0.5f
        }
    }

    private void StepBack()
    {
        GameObject lastAddedRoom = this.prevRoom;
        RoomSpawner[] roomSpawner = lastAddedRoom.GetComponentsInChildren<RoomSpawner>();
        if (!lastAddedRoom.CompareTag("CheckPointRoom"))
        {
            foreach (RoomSpawner rs in roomSpawner)
            {
                if (rs.nextRoom == null)
                    rs.isSpawned = true;
                else
                {
                    rs.isStepBack = true;
                    rs.Invoke("DeleteNextRoom", 0.0f); //0.1f
                }
            }
        }
        else
        {
            lastAddedRoom.GetComponent<DeleteStageRooms>().enabled = true;
        }
    }
}