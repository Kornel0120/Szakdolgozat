using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    [Header("Parent GameObject")]
    public GameObject Parent;
    [Header("Rooms with bottom opening")]
    public GameObject[] bottomRooms;
    [Header("Rooms with top opening")]
    public GameObject[] topRooms;
    [Header("Rooms with left side opening")]
    public GameObject[] leftRooms;
    [Header("Rooms with right side opening")]
    public GameObject[] rightRooms;
    [Header("Spawn room")]
    public GameObject SpawnRoom;
    [Header("Key room")]
    public GameObject KeyRoom;
    [Header("Checkpoint room")]
    public GameObject CheckPointRoom;
    [Header("Trap room")]
    public GameObject TrapRoom;
    [Header("Spacecheck")]
    public GameObject spaceCheck;

    [Header("Room quantity per branches")]
    public int RoomQuantity = 15;

    [Header("Bools")]
    public bool isCheckPointRoomSpawned = false;
    public bool isKeyRoomSpawned = false;
    public bool SpecialRoomSpawn = false;

    [Header("Lists")]
    public List<bool> isFirstRoomsGenerated = new List<bool>();

    public int SpecialRooms = 0;

    [Header("Graph")]
    public Graph g = new Graph(4);
    public GameObject prevRoom;

}