using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject SpawnRoom;
    public GameObject KeyRoom;
    public GameObject CheckPointRoom;
    public GameObject TrapRoom;

    public List<List<GameObject>> rooms = new List<List<GameObject>>();
    //public List<GameObject> rooms;
    //public List<Vector3> targetPostitions = new List<Vector3>();
    public List<Vector3> positions = new List<Vector3>();

    public List<Node> nodes = new List<Node>();
    public int lastIndex = 0;
    //public static GameObject spawnRoom = GameObject.FindGameObjectWithTag("SpawnRoom");
    public List<bool> isFirstRoomsGenerated = new List<bool>();
    public List<int> invalidIndexes = new List<int>();
    public Graph g = new Graph(4);
    

    //public static GameObject spawnRoom;

    //private void Start()
    //{
    //    spawnRoom = GameObject.FindGameObjectWithTag("SpawnRoom");
    //}

    //public Graph g = new Graph(new Node(new Vector3(spawnRoom.transform.position.x,
    //                                                spawnRoom.transform.position.y,
    //                                                spawnRoom.transform.position.z),
    //                                                spawnRoom), 4);

}