using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    private RoomTemplates templates;
    public GameObject RoutePref;
    public Vector3 targetLocation;
    public Vector3 startLocation;
    public int step;


    // Start is called before the first frame update
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("OnTriggerEnter", 1f);
        Invoke("routeCalc", 1f);
    }

    // Update is called once per frame
    void routeCalc()
    {
        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        targetLocation = GameObject.Find("TargetPoint").transform.position;
        startLocation = this.gameObject.transform.position;
        Debug.LogFormat("targetLoc: {0}, startLoc: {1}", targetLocation, startLocation);
        if (targetLocation.x < startLocation.x)
        {
            startLocation.x -= step;
            Instantiate(RoutePref, startLocation, Quaternion.identity);
        }
        else if (targetLocation.z < startLocation.z)
        {
            startLocation.z -= step;
            Instantiate(RoutePref, startLocation, Quaternion.identity);
        }
        else if (targetLocation.x > startLocation.x)
        {
            startLocation.x += step;
            Instantiate(RoutePref, startLocation, Quaternion.identity);
        }
        else if (targetLocation.z > startLocation.z)
        {
            startLocation.z += step;
            Instantiate(RoutePref, startLocation, Quaternion.identity);
        }
        templates.positions.Add(startLocation);
    }
}
