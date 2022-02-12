using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerableDirection : MonoBehaviour
{
    public bool Generable = true;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("GenerableCheck", 0.5f);
    }

    void GenerableCheck()
    {
        //if(Generable == true)
        //{
        //    RoomSpawner rs = this.gameObject.GetComponent<RoomSpawner>();
        //    rs.enabled = true;
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        Generable = false;
    }
}
