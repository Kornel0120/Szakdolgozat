using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSpawn : MonoBehaviour
{
    public GameObject guard;
    public List<GameObject> targetObjects = new List<GameObject>();

    void SpawnGuard()
    {
        GameObject temp = Instantiate(guard, this.transform.position, this.transform.rotation);
        temp.GetComponent<GuardMovement>().stage = this.GetComponent<AddRoom>().stage;
    }
}
