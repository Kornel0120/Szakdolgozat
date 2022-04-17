using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteStageRoomSpawners : MonoBehaviour
{
    void Start()
    {
        Invoke("DeletePrevStageSpawners", 0.0f);
    }

    void DeletePrevStageSpawners()
    {
        foreach (GameObject spawner in GameObject.FindGameObjectsWithTag("RoomSpawnPoint"))
        {
            if (spawner.GetComponentInParent<AddRoom>().stage == this.gameObject.GetComponent<AddRoom>().stage - 1)
                Destroy(spawner);
            else if (this.gameObject.CompareTag("FinishRoom") == true)
                Destroy(spawner);
        }
    }
}
