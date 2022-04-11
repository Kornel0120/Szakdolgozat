using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    private RoomTemplates templates;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    }

    void KeyPickedUpByPlayer()
    {
        GameObject checkPointRoom = null;

        if(templates.KeyPickedUp < 2)
        {
            foreach (GameObject checkPointRooms in GameObject.FindGameObjectsWithTag("CheckPointRoom"))
            {
                if (checkPointRooms.GetComponent<AddRoom>().stage == this.GetComponentInParent<AddRoom>().stage + 1)
                {
                    checkPointRoom = checkPointRooms;
                }
            }
        } 
        else
        {
            checkPointRoom = GameObject.FindGameObjectWithTag("FinishRoom");
        }

        if(checkPointRoom != null)
        {
            if (templates.KeyPickedUp < 2)
                checkPointRoom.GetComponent<GuardSpawn>().Invoke("SpawnGuard", 0.1f);
            CheckPointDoorOpen[] Doors = checkPointRoom.GetComponentsInChildren<CheckPointDoorOpen>();
            foreach (CheckPointDoorOpen CDO in Doors)
            {
                CDO.enabled = true;
            }
            templates.KeyPickedUp++;
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            KeyPickedUpByPlayer();
        }
    }

}
