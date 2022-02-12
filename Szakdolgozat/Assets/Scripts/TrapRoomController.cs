using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRoomController : MonoBehaviour
{
    RoomTemplates templates;
    public GameObject TrapRoom;
    public bool isPressurePlateActivated = false;
    public bool isDoorsOpen = false;

    private void Start()
    {
        this.TrapRoom = this.gameObject;
    }
}
