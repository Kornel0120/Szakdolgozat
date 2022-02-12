using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(gameObject.tag == "Room" && other.gameObject.tag == "Wall")
        {
            //Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!COLLIDE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + gameObject.name);
        }  
    }
}
