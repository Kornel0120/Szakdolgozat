using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform GrabbedObjectPosition;
    GameObject thisTrapRoom;
    GameObject[] TrapRooms;

    private void Start()
    {
        TrapRooms = GameObject.FindGameObjectsWithTag("TrapRoom");
        foreach (GameObject tr in TrapRooms)
        {
            if (tr.GetComponent<TrapRoomController>().TrapRoom == this.gameObject.GetComponentInParent<TrapRoomController>().TrapRoom)
            {
                thisTrapRoom = tr;
            }
        }
        GrabbedObjectPosition = GameObject.FindGameObjectWithTag("GrabbedObjectPosition").transform;
    }

    public void OnMouseDown()
    {
        foreach (GameObject tr in TrapRooms)
        {
            if (tr.GetComponent<TrapRoomController>().TrapRoom == this.gameObject.GetComponentInParent<TrapRoomController>().TrapRoom)
            {
                thisTrapRoom = tr;
            }
        }
        GetComponent<Rigidbody>().useGravity = false;
        this.transform.position = GrabbedObjectPosition.position;
        this.transform.parent = GameObject.Find("GrabbedObjectPosition").transform;
    }

    public void OnMouseUp()
    {
        this.transform.parent = thisTrapRoom.transform;
        this.GetComponent<Rigidbody>().useGravity = true;
    }
}
