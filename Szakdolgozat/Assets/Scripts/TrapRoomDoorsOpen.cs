using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRoomDoorsOpen : MonoBehaviour
{
    TrapRoomController trapRoomController;
    public bool isDoorOpen = false;

    private void FixedUpdate()
    {
        trapRoomController = this.gameObject.GetComponentInParent<TrapRoomController>();
        if (trapRoomController.isDoorsOpen == true)
            Invoke("DoorOpen", 0.1f);
        else
            Invoke("DoorClose", 0.1f);
    }
    void DoorOpen()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
        //Destroy(this.gameObject);
    }

    void DoorClose()
    {
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<Collider>().enabled = true;
    }
}
