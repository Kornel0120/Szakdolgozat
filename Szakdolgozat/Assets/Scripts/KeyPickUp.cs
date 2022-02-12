using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    void KeyPickedUpByPlayer()
    {
        GameObject[] checkPointRooms = GameObject.FindGameObjectsWithTag("CheckPointRoom");
        foreach (GameObject checkPointRoom in checkPointRooms)
        {
            CheckPointDoorOpen[] Doors = checkPointRoom.GetComponentsInChildren<CheckPointDoorOpen>();
            foreach (CheckPointDoorOpen CDO in Doors)
            {
                CDO.enabled = true;
            }
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            KeyPickedUpByPlayer();
        }
    }

}