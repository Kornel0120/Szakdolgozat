using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckInSpawnRoom : MonoBehaviour
{
    [SerializeField] LayerMask groundMask;
    private RoomTemplates templates;
    private List<string> inRangeGameObjects = new List<string>();

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    }

    void FixedUpdate()
    {
        if (playerOutsideSpawnRoom() == true && templates.isFinishRoomSpawned == true)
        {
            this.gameObject.GetComponentInParent<GuardSpawn>().Invoke("SpawnGuard", 0.1f);
            this.gameObject.GetComponentInParent<PlayerCheckInSpawnRoom>().enabled = false;
        }
        inRangeGameObjects.Clear();
    }

    private bool playerOutsideSpawnRoom()
    {
        if (this.GetComponentInChildren<PlayerSpawn>().isPlayerSpawned == false)
            return false;

        foreach (Collider collider in Physics.OverlapSphere(this.transform.position, 5, groundMask))
        {
            inRangeGameObjects.Add(collider.gameObject.tag);
        } 
        if (inRangeGameObjects.Contains("Player"))
            return false;
        else
            return true;
    }
}
