using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointDoorOpen : MonoBehaviour
{
    void Start()
    {
        Invoke("DoorOpen", 0.1f);
    }

    void DoorOpen()
    {
        Destroy(this.gameObject);
    }
}
