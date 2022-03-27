using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteRoom : MonoBehaviour
{
    public RoomSpawner[] prevSpawners;
    public bool isSpawn = false;
    public List<bool> isEnoughSpace = new List<bool>();
    public byte falseSpace = 0;
    public GameObject prevRoomGameObject;

    private void Start()
    {
        if(prevSpawners != null)
        { 
            prevRoomGameObject = this.GetComponentInChildren<RoomSpawner>().prevRoom;
            prevSpawners = prevRoomGameObject.GetComponentsInChildren<RoomSpawner>();
        }
    }

    public void spaceCheck()
    {
        //foreach (bool space in isEnoughSpace)
        //{
        //    if (space == false)
        //        falseSpace++;
        //}

            if (!isEnoughSpace.Contains(true))
                invokeDeleteNextRoomScript();
    }

    private void invokeDeleteNextRoomScript()
    {
        foreach (RoomSpawner rs in prevSpawners)
        {
            if(rs.nextRoom == this.gameObject)
            {
                rs.isStepBack = true;
                rs.isSpaceCheckedForRoom = false;
                rs.isSpaceCheckedForSpecialRoom = false;
                rs.Invoke("DeleteNextRoom", 0.5f);
            }
        }
    }
}
