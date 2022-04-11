using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRoomDoorsOpen : MonoBehaviour
{
    TrapRoomController trapRoomController;
    public bool isDoorOpen = false;
    [SerializeField]
    public GameObject pressurePlate;

    private void FixedUpdate()
    {
        trapRoomController = this.gameObject.GetComponentInParent<TrapRoomController>();
        if (isDoorOpen == true)
            Invoke("DoorOpen", 0.1f);
        else
            Invoke("DoorClose", 0.1f);
    }
    void DoorOpen()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
        if (pressurePlate.GetComponent<PressurePlateActivated>().Door = this.gameObject)
            pressurePlate.GetComponent<PressurePlateActivated>().enabled = true;
    }

    void DoorClose()
    {
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<Collider>().enabled = true;
    }
}
