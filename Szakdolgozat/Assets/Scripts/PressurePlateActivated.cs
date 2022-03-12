using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateActivated : MonoBehaviour
{
    public TrapRoomController trapRoomController;
    public GameObject[] TrapRooms;
    public GameObject thisTrapRoom;
    private TrapRoomDoorsOpen[] Doors;
    private int rand;

    private void Start()
    {
        trapRoomController = this.gameObject.GetComponentInParent<TrapRoomController>();
        TrapRooms = GameObject.FindGameObjectsWithTag("TrapRoom");   
        foreach (GameObject tr in TrapRooms)
        {
            if (tr.GetComponent<TrapRoomController>().TrapRoom == this.gameObject.GetComponentInParent<TrapRoomController>().TrapRoom)
            {
                thisTrapRoom = tr;
            }
        }
        TrapRoomDoorsOpen[] Doors = thisTrapRoom.GetComponentsInChildren<TrapRoomDoorsOpen>();  
        rand = Random.Range(0, Doors.Length - 1);
    }

    void PressurePlateActivate()
    {
        if(trapRoomController.isPressurePlateActivated == true)
        {
            this.trapRoomController.isDoorsOpen = true;
            //Doors[rand].GetComponent<TrapRoomDoorsOpen>().isDoorOpen = true;
        }
        else
        {
            this.trapRoomController.isDoorsOpen = false;
            //Doors[rand].GetComponent<TrapRoomDoorsOpen>().isDoorOpen = false;
        }
            
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grabbable"))
        {
            //foreach (GameObject tr in TrapRooms)
            //{
            //    if (tr.GetComponent<TrapRoomController>().TrapRoom == this.gameObject.GetComponentInParent<TrapRoomController>().TrapRoom)
            //    {
                thisTrapRoom.GetComponent<TrapRoomController>().isPressurePlateActivated = true;
                PressurePlateActivate();
            //    }    
            //}

        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grabbable"))
        {
            thisTrapRoom.GetComponent<TrapRoomController>().isPressurePlateActivated = false;
            PressurePlateActivate();
        }
    }
}
