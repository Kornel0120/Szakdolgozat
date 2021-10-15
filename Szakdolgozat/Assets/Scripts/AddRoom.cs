using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    private RoomTemplates templates;
    private RoomSpawner spawner;
    public int Index; //Ez mondja meg, hogy hany iranyba valt szet a folyoso, tehat ha ez 4 akkor 4 kulonbozo iranyba ment el a spawnroombol.

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Index = this.gameObject.GetComponentInChildren<RoomSpawner>().Index;
        RoomTemplates.Node temp1 = new RoomTemplates.Node(Index, this.gameObject);
        Index = this.gameObject.GetComponentInChildren<RoomSpawner>().Index;
        RoomTemplates.Node temp2 = new RoomTemplates.Node(Index, this.gameObject);

        templates.g.addEdge(temp1, temp2);
        Debug.Log(templates.g.toStr());
        //List<GameObject> temp = new List<GameObject>();
        //temp.Add(this.gameObject);
        //templates.rooms.Add(temp);
    }
}
