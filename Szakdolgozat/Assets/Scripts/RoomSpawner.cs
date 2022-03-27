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
    public int ExtraRoomQuantity = 0;

    public List<int> spawnedRoomsIndex = new List<int>();

    [SerializeField] LayerMask groundMask;

    public NavMeshSurface surface;

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
        //if(!this.GetComponentInParent<AddRoom>().CompareTag("SpawnRoom") && this.GetComponentInParent<DeleteRoom>().isEnoughSpace.Count < this.GetComponentInParent<AddRoom>().GetComponentsInChildren<RoomSpawner>().Length)
        EnoughSpaceForRoom();

        if (isEnoughSpaceForRoom == true && isSpawned == false)
        {
            if (templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Count == templates.RoomQuantity && templates.isKeyRoomSpawned == false && templates.stageCounter == 1 && templates.TrapRoomCounter != 2)
            {
                EnoughShapceForSpecialRoom(1f);
            }
            else if(templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Count == templates.RoomQuantity && templates.isKeyRoomSpawned == false && (templates.stageCounter == 2 || templates.stageCounter == 3) && templates.TrapRoomCounter == 0)
            {
                EnoughShapceForSpecialRoom(1f);
            }
            else if(templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Count == templates.RoomQuantity)
            {
                EnoughShapceForSpecialRoom(3f);
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
                        //nextRoom.GetComponent<AddRoom>().stage = templates.stageCounter;
                    }
                    else if (templates.stageCounter == 2 && templates.TrapRoomCounter == 1)
                    {
                        templates.stageCounter++;
                        SpecialRoomSpawn(templates.CheckPointRoom);
                        templates.isCheckPointRoomSpawned = true;
                        //nextRoom.GetComponent<AddRoom>().stage = templates.stageCounter;
                    }
                    else if (templates.stageCounter == 3 && templates.TrapRoomCounter == 1)
                    {
                        SpecialRoomSpawn(templates.FinishRoom);
                        templates.isFinishRoomSpawned = true;
                        //nextRoom.GetComponent<AddRoom>().stage = templates.stageCounter;
                    }
                }
            }

            if (openingDirection == 1)
            {
                SelectRoom(templates.bottomRooms);
                //SpecialRoomRotationTest(templates.bottomRooms);
            }
            else if (openingDirection == 2)
            {
                SelectRoom(templates.topRooms);
                //SpecialRoomRotationTest(templates.topRooms);
            }
            else if (openingDirection == 3)
            {
                SelectRoom(templates.leftRooms);
                //SpecialRoomRotationTest(templates.leftRooms);
            }
            else if (openingDirection == 4)
            {
                SelectRoom(templates.rightRooms);
                //SpecialRoomRotationTest(templates.rightRooms);
            }
        }

        isSpawned = true;
        if (!this.GetComponentInParent<AddRoom>().gameObject.CompareTag("SpawnRoom") && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("CheckPointRoom")
            && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("KeyRoom") && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("TrapRoom"))
            this.GetComponentInParent<DeleteRoom>().Invoke("spaceCheck", 0.3f);


        //DestroyUnUsedSpawnPoint();
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
                Debug.Log("SelectRoom !this.spawnedRoomsIndex.Contains(rand) ág");
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
                Debug.Log("SelectRoom this.spawnedRoomsIndex.Contains(rand) && this.spawnedRoomsIndex.Count < roomList.Length ág");
                isSpawned = false;
                isEnoughSpaceForRoom = false;
                isSpaceCheckedForRoom = false;
                Invoke("Spawn", 0.5f);
            }
            else if (this.spawnedRoomsIndex.Contains(rand) && this.spawnedRoomsIndex.Count >= roomList.Length)
            {
                Debug.Log("SelectRoom this.spawnedRoomsIndex.Contains(rand) && this.spawnedRoomsIndex.Count >= roomList.Length ág");
                StepBack();
            }
        }
    }

    private void SpecialRoomRotationTest(GameObject[] roomList)
    {
        if (isEnoughSpaceForSpecialRoom == false && templates.SpecialRoomSpawn == false && templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Count < templates.RoomQuantity)
        {
            if (!spawnedRoomsIndex.Contains(0))
            {
                nextRoom = Instantiate(roomList[0], transform.position, roomList[0].transform.rotation, templates.Parent.transform);
                CreateNode(nextRoom);
                //Instantiate(nextRoom.gameObject, nextRoom.transform.position, nextRoom.transform.rotation, templates.Parent.transform);
                nextRoom.GetComponent<AddRoom>().stage = templates.stageCounter;
                foreach (RoomSpawner rs in nextRoom.GetComponentsInChildren<RoomSpawner>())
                {
                    rs.prevRoom = this.gameObject.GetComponentInParent<AddRoom>().gameObject;
                }
            }
            else if (spawnedRoomsIndex.Contains(0) && spawnedRoomsIndex.Count < roomList.Length)
            {
                Debug.Log("Már spawnolt!");
            }
            else if (spawnedRoomsIndex.Contains(0) && spawnedRoomsIndex.Count >= roomList.Length)
            {
                Debug.Log("StepBack");
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

    private GameObject CreateNode(GameObject nextGameObject)
    {
        nextGameObject.GetComponent<AddRoom>().lastIndex = this.gameObject.GetComponentInParent<AddRoom>().lastIndex;
        if(this.gameObject.GetComponentInParent<AddRoom>().gameObject.CompareTag("SpawnRoom"))
        {
            nextGameObject.GetComponent<AddRoom>().newRoom = new Node(new Vector3(nextGameObject.transform.position.x, nextGameObject.transform.position.y, nextGameObject.transform.position.z),
                                                               nextGameObject, this.templates.g.rootNode,
                                                               this.gameObject.GetComponentInParent<AddRoom>().lastIndex);
        }
        else
        {
            nextGameObject.GetComponent<AddRoom>().newRoom = new Node(new Vector3(nextGameObject.transform.position.x, nextGameObject.transform.position.y, nextGameObject.transform.position.z),
                                                                nextGameObject, this.gameObject.GetComponentInParent<AddRoom>().newRoom,
                                                                this.gameObject.GetComponentInParent<AddRoom>().lastIndex);
        }
        
        this.GetComponentInParent<AddRoom>().nextRoom = nextGameObject;

        return nextGameObject;
    }

    public Vector3 temp;


    private void EnoughSpaceForSlope(GameObject gameObject)
    {
        if(gameObject.name.Contains("Up"))
        {
            if (isSpaceCheckedForRoom == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.x < this.transform.position.x)
            {
                isEnoughSpaceForRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x + 0.25f, this.transform.position.y + 0.15f, this.transform.position.z), 0.5f, groundMask)
                    && !Physics.CheckSphere(new Vector3(this.transform.position.x + 0.15f, this.transform.position.y + 1.15f, this.transform.position.z), 0.5f, groundMask);
                temp = new Vector3(this.transform.position.x + 0.25f, this.transform.position.y + 0.15f, this.transform.position.z);
            }
            else if (isSpaceCheckedForRoom == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.x > this.transform.position.x)
            {
                isEnoughSpaceForRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x - 0.25f, this.transform.position.y + 0.15f, this.transform.position.z), 0.5f, groundMask)
                    && !Physics.CheckSphere(new Vector3(this.transform.position.x - 0.15f, this.transform.position.y + 1.15f, this.transform.position.z), 0.5f, groundMask);
                temp = new Vector3(this.transform.position.x - 0.25f, this.transform.position.y + 0.15f, this.transform.position.z);
            }
            else if (isSpaceCheckedForRoom == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.z < this.transform.position.z)
            {
                isEnoughSpaceForRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y + 0.15f, this.transform.position.z + 0.25f), 0.5f, groundMask)
                    && !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y + 1.15f, this.transform.position.z + 0.15f), 0.5f, groundMask);
                temp = new Vector3(this.transform.position.x, this.transform.position.y + 0.15f, this.transform.position.z + 0.25f);
            }
            else if (isSpaceCheckedForRoom == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.z > this.transform.position.z)
            {
                isEnoughSpaceForRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y + 0.15f, this.transform.position.z - 0.25f), 0.5f, groundMask)
                    && !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y + 1.15f, this.transform.position.z - 0.15f), 0.5f, groundMask);
                temp = new Vector3(this.transform.position.x, this.transform.position.y + 0.15f, this.transform.position.z - 0.25f);
            }
        }
        else
        {
            if (isSpaceCheckedForRoom == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.x < this.transform.position.x)
            {
                isEnoughSpaceForRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x + 0.25f, this.transform.position.y + 0.15f, this.transform.position.z), 0.5f, groundMask)
                    && !Physics.CheckSphere(new Vector3(this.transform.position.x + 0.25f, this.transform.position.y + 1.15f, this.transform.position.z), 0.5f, groundMask); 
                temp = new Vector3(this.transform.position.x + 0.25f, this.transform.position.y + 0.15f, this.transform.position.z);
            }
            else if (isSpaceCheckedForRoom == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.x > this.transform.position.x)
            {
                isEnoughSpaceForRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x - 0.25f, this.transform.position.y + 0.15f, this.transform.position.z), 0.5f, groundMask)
                    && !Physics.CheckSphere(new Vector3(this.transform.position.x - 0.25f, this.transform.position.y + 1.15f, this.transform.position.z), 0.5f, groundMask); 
                temp = new Vector3(this.transform.position.x - 0.25f, this.transform.position.y + 0.15f, this.transform.position.z);
            }
            else if (isSpaceCheckedForRoom == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.z < this.transform.position.z)
            {
                isEnoughSpaceForRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y + 0.15f, this.transform.position.z + 0.25f), 0.5f, groundMask)
                    && !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y + 1.15f, this.transform.position.z + 0.15f), 0.5f, groundMask);
                temp = new Vector3(this.transform.position.x, this.transform.position.y + 0.15f, this.transform.position.z + 0.25f);
            }
            else if (isSpaceCheckedForRoom == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.z > this.transform.position.z)
            {
                isEnoughSpaceForRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y + 0.15f, this.transform.position.z - 0.25f), 0.5f, groundMask) 
                    && !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y + 1.15f, this.transform.position.z - 0.15f), 0.5f, groundMask);
                temp = new Vector3(this.transform.position.x, this.transform.position.y + 0.15f, this.transform.position.z - 0.25f);
            }
        }

        if (!this.GetComponentInParent<AddRoom>().gameObject.CompareTag("SpawnRoom") && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("CheckPointRoom")
            && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("KeyRoom") && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("TrapRoom"))
            this.GetComponentInParent<DeleteRoom>().isEnoughSpace.Add(isEnoughSpaceForRoom);

        Debug.Log("EnoughSpaceForRoom: " + isEnoughSpaceForRoom + " Szoba: " + this.GetComponentInParent<AddRoom>().gameObject.name);

        isSpaceCheckedForRoom = true;
    }

    private void EnoughSpaceForRoom()
    {
        if (isSpaceCheckedForRoom == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.x < this.transform.position.x)
        {
            isEnoughSpaceForRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x + 0.25f, this.transform.position.y + 0.15f, this.transform.position.z), 0.5f, groundMask)
                && !Physics.CheckSphere(new Vector3(this.transform.position.x + 0.25f, this.transform.position.y + 1.15f, this.transform.position.z), 0.5f, groundMask)
                && !Physics.CheckSphere(new Vector3(this.transform.position.x + 0.25f, this.transform.position.y - 1.15f, this.transform.position.z), 0.5f, groundMask);
            temp = new Vector3(this.transform.position.x + 0.15f, this.transform.position.y + 0.15f, this.transform.position.z);
        }    
        else if (isSpaceCheckedForRoom == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.x > this.transform.position.x)
        {
            
            isEnoughSpaceForRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x - 0.25f, this.transform.position.y + 0.15f, this.transform.position.z), 0.5f, groundMask)
                && !Physics.CheckSphere(new Vector3(this.transform.position.x - 0.25f, this.transform.position.y + 1.15f, this.transform.position.z), 0.5f, groundMask)
                && !Physics.CheckSphere(new Vector3(this.transform.position.x - 0.25f, this.transform.position.y - 1.15f, this.transform.position.z), 0.5f, groundMask);
            temp = new Vector3(this.transform.position.x - 0.15f, this.transform.position.y + 0.15f, this.transform.position.z);
        }
        else if (isSpaceCheckedForRoom == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.z < this.transform.position.z)
        {
            isEnoughSpaceForRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y + 0.15f, this.transform.position.z + 0.25f), 0.5f, groundMask)
                && !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y + 1.15f, this.transform.position.z + 0.25f), 0.5f, groundMask)
                && !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y - 1.15f, this.transform.position.z + 0.25f), 0.5f, groundMask);
            temp = new Vector3(this.transform.position.x, this.transform.position.y + 0.15f, this.transform.position.z + 0.15f);
        }        
        else if (isSpaceCheckedForRoom == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.z > this.transform.position.z)
        {
            isEnoughSpaceForRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y + 0.15f, this.transform.position.z - 0.25f), 0.5f, groundMask)
                 && !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y + 1.15f, this.transform.position.z - 0.225f), 0.5f, groundMask)
                 && !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y - 1.15f, this.transform.position.z - 0.25f), 0.5f, groundMask);
            temp = new Vector3(this.transform.position.x, this.transform.position.y + 0.15f, this.transform.position.z - 0.15f);
        }
            

        if(!this.GetComponentInParent<AddRoom>().gameObject.CompareTag("SpawnRoom") && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("CheckPointRoom")
            && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("KeyRoom") && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("TrapRoom"))
            this.GetComponentInParent<DeleteRoom>().isEnoughSpace.Add(isEnoughSpaceForRoom);

       
        Debug.Log("EnoughSpaceForRoom: " + isEnoughSpaceForRoom + " Szoba: " + this.GetComponentInParent<AddRoom>().gameObject.name);

        isSpaceCheckedForRoom = true;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(temp, 0.45f);
    }

    private void EnoughShapceForSpecialRoom(float radius)
    {
        if (isSpaceCheckedForSpecialRoom == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.x < this.transform.position.x)
            isEnoughSpaceForSpecialRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x + radius, this.transform.position.y, this.transform.position.z), radius, groundMask);
        else if (isSpaceCheckedForSpecialRoom == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.x > this.transform.position.x)
            isEnoughSpaceForSpecialRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x - radius, this.transform.position.y, this.transform.position.z), radius, groundMask);
        else if (isSpaceCheckedForSpecialRoom == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.z < this.transform.position.z)
            isEnoughSpaceForSpecialRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + radius), radius, groundMask);
        else if (isSpaceCheckedForSpecialRoom == false && this.GetComponentInParent<AddRoom>().gameObject.transform.position.z > this.transform.position.z)
            isEnoughSpaceForSpecialRoom = !Physics.CheckSphere(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - radius), radius, groundMask);
        if (isEnoughSpaceForSpecialRoom == false || this.GetComponentInParent<AddRoom>().name.Contains("Slope"))
        {
            isEnoughSpaceForSpecialRoom = false;
            isEnoughSpaceForSpecialRoom = false;
            templates.RoomQuantity += 1;
            Invoke("Spawn", 0.5f);
        }

        isSpaceCheckedForSpecialRoom = true;
    }

    private void StepBackCollider(Collider other)
    {
        if (other.CompareTag("Wall") /*|| other.CompareTag("Ground"))*/ && this.nextRoom == null)
        {
            GameObject lastAddedRoom = this.prevRoom;
            //GameObject lastAddedRoom = templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex][templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Count - 1].gObject;
            RoomSpawner[] roomSpawner = lastAddedRoom.GetComponentsInChildren<RoomSpawner>();
            foreach (RoomSpawner rs in roomSpawner)
            {
                if (rs.nextRoom == null)
                    rs.isSpawned = true;
                else
                {
                    rs.isSpawned = false;
                    rs.isStepBack = true;
                    rs.Invoke("DeleteNextRoom", 0.5f);
                    //Destroy(rs.nextRoom);
                }
            }
        }
    }

    public void DeleteNextRoom()
    {

        //if(!this.prevRoom.CompareTag("SpawnRoom") && this.prevRoom.CompareTag("CheckPointRoom"))
        //{
        //    templates.g.roomLists[this.GetComponentInParent<AddRoom>().lastIndex].Clear();
        //    templates.isFirstRoomsGenerated.RemoveAt(this.GetComponentInParent<AddRoom>().lastIndex);
        //}

        if (!this.GetComponentInParent<AddRoom>().gameObject.CompareTag("SpawnRoom") && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("CheckPointRoom")
            && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("KeyRoom") && !this.GetComponentInParent<AddRoom>().gameObject.CompareTag("TrapRoom"))
        {
            this.GetComponentInParent<DeleteRoom>().isEnoughSpace.Clear();
        }
            
        if (isStepBack == true)
        {
            templates.g.roomLists[this.gameObject.GetComponentInParent<AddRoom>().lastIndex].Remove(this.nextRoom.GetComponent<AddRoom>().newRoom);
            Destroy(this.nextRoom);
            Debug.Log(templates.g.toStr());
            isSpawned = false;
            isStepBack = false;
            Invoke("Spawn", 0.5f);
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
                    rs.Invoke("DeleteNextRoom", 0.1f);
                    //Destroy(this.GetComponentInParent<AddRoom>().gameObject);
                }
            }
        }
        else
        {
            lastAddedRoom.GetComponent<DeleteStageRooms>().enabled = true;
        }
        //foreach (RoomSpawner rs in roomSpawner)
        //{
        //    if (rs.isSpawned == false)
        //        rs.Invoke("Spawn", 0.5f);
        //}
    }

    void DestroyUnUsedSpawnPoint()
    {
        if(!this.gameObject.GetComponentInParent<AddRoom>().gameObject.CompareTag("SpawnRoom"))
        {
            if(!this.prevRoom.CompareTag("SpawnRoom"))
            {
                RoomSpawner[] prevRoomSpawnPoints = this.prevRoom.GetComponentsInChildren<RoomSpawner>();
                foreach (RoomSpawner rs in prevRoomSpawnPoints)
                {
                    if (rs.nextRoom == null)
                        Destroy(rs.gameObject);
                }
            } 
        }
        
    }
}