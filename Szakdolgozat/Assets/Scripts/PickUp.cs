using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform GrabbedObjectPosition;
    GameObject thisTrapRoom;
    GameObject[] TrapRooms;
    PressurePlateActivated firstPressurePlate;

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

        foreach (PressurePlateActivated PPA in this.GetComponentInParent<AddRoom>().GetComponentsInChildren<PressurePlateActivated>())
        {
            if (PPA.Door == null)
            {
                firstPressurePlate = PPA;
            }
        }
    }

    public void OnMouseDown()
    {
        firstPressurePlate.enabled = true;
        foreach (GameObject tr in TrapRooms)
        {
            if (tr.GetComponent<TrapRoomController>().TrapRoom == this.gameObject.GetComponentInParent<TrapRoomController>().TrapRoom)
            {
                thisTrapRoom = tr;
            }
        }
        GetComponent<Rigidbody>().useGravity = false;
        this.transform.position = GrabbedObjectPosition.position;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        this.transform.parent = GameObject.Find("GrabbedObjectPosition").transform;
    }

    public void OnMouseUp()
    {
        this.transform.parent = thisTrapRoom.GetComponentInChildren<PuzzleCubeSawn>().transform;
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    public void OnCollisionEnter(Collision collision)
    {
        this.transform.parent = thisTrapRoom.GetComponentInChildren<PuzzleCubeSawn>().transform;
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
}
