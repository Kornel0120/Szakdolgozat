using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialRoomSpawner : MonoBehaviour
{
    private RoomTemplates templates;
    public int radius;
    private int angle;

    public bool isCheckPointRoomSpawned = false, isKeyRoomSpawned = false, isTrapRoomSpawned = false;
    private int TrapRoomCount = 0;

    public int segments = 180;

    void Start()
    {
        angle = Random.Range(0, 360);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("SpawnSpecialRoom", 0.1f);

        //templates.targetPostitions.Add(GameObject.Find("TargetPoint").transform.position);
    }

    void SpawnSpecialRoom()
    {
        if(isCheckPointRoomSpawned == false)
        {
            Instantiate(templates.CheckPointRoom, roomLocation(), Quaternion.identity);
            isCheckPointRoomSpawned = true;
        }
        if (isKeyRoomSpawned == false)
        {
            Instantiate(templates.KeyRoom, roomLocation(), Quaternion.identity);
            isKeyRoomSpawned = true;
        }
        while (isTrapRoomSpawned == false)
        {
            Instantiate(templates.TrapRoom, roomLocation(), Quaternion.identity);
            TrapRoomCount++;
            if (TrapRoomCount == 2)
                isTrapRoomSpawned = true;
        }
    }

    public Vector3 roomLocation()
    {
        float x = (int)(Mathf.Sin(Mathf.Deg2Rad * angle) * radius) +0.5f;
        float y = 0;
        float z = (int)(Mathf.Cos(Mathf.Deg2Rad * angle) * radius) +0.5f;

        angle += (360 / segments);
        return new Vector3(x, y, z);
    }
}
