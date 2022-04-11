using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateActivated : MonoBehaviour
{
    public TrapRoomController trapRoomController;
    public GameObject[] TrapRooms;
    public GameObject thisTrapRoom;
    private int rand;
    [SerializeField]
    LayerMask groundMask;
    private bool isNewCubeSpawned = false;
    [SerializeField]
    public GameObject Door;
    private List<string> inRangeGameObjects = new List<string>();

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
    }

    private void FixedUpdate()
    {
        PressurePlateActivate();
    }

    private bool cubeOnPressurePlate()
    {
        foreach (Collider collider in Physics.OverlapSphere(this.transform.position, 0.1f, groundMask))
        {
            inRangeGameObjects.Add(collider.gameObject.tag);
        }
        if (!inRangeGameObjects.Contains("Grabbable"))
            return false;
        else
            return true;
    }

    void PressurePlateActivate()
    {
        List<TrapRoomDoorsOpen> Doors = new List<TrapRoomDoorsOpen>();

        foreach (TrapRoomDoorsOpen TRDO in this.gameObject.GetComponentInParent<AddRoom>().GetComponentsInChildren<TrapRoomDoorsOpen>())
        {
            if (TRDO.gameObject.GetComponent<MeshRenderer>().enabled == true)
            {
                Doors.Add(TRDO);
            }
        }

        Debug.Log("cubeOnPressurePlate: " + cubeOnPressurePlate());
        if (cubeOnPressurePlate() == true)
        {
            if (this.Door == null)
            {
                this.GetComponentInParent<AddRoom>().GetComponentInChildren<CheckPointDoorOpen>().GetComponent<MeshRenderer>().enabled = true;
                this.GetComponentInParent<AddRoom>().GetComponentInChildren<CheckPointDoorOpen>().GetComponent<BoxCollider>().enabled = true;
            }

            if (Doors.Count > 2)
                rand = Random.Range(0, Doors.Count - 1);
            else
                rand = 0;
            foreach (TrapRoomDoorsOpen go in Doors)
            {
                Debug.Log("TrapRoomDoors: " + go.gameObject);
            }
            this.trapRoomController.isDoorsOpen = true;
            if(isNewCubeSpawned == false && Doors.Count > 0)
            {
                GetComponentInParent<AddRoom>().GetComponentInChildren<PuzzleCubeSawn>().spawnCube();
                Doors[rand].GetComponent<TrapRoomDoorsOpen>().isDoorOpen = true;
                Doors.Clear();
                isNewCubeSpawned = true;
            }
            else if(isNewCubeSpawned == false)
            {
                GetComponentInParent<AddRoom>().GetComponentInChildren<CheckPointDoorOpen>().enabled = true;
                isNewCubeSpawned = true;
            }
           
            this.GetComponent<PressurePlateActivated>().enabled = false;
        }
        else
        {
            this.trapRoomController.isDoorsOpen = false;
        }
            
    }
}
