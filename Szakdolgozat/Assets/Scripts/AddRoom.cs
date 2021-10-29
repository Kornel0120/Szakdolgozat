using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    private SpawnRoomSpawn SRS;
    private RoomTemplates templates;
    public int lastIndex; //Ez mondja meg, hogy hany iranyba valt szet a folyoso, tehat ha ez 4 akkor 4 kulonbozo iranyba ment el a spawnroombol.
    //private List<bool> isFirstRoomsGenerated = new List<bool>();

    //0 -> Zold oldal
    //1 -> Sarga oldal
    //2 -> Kek oldal
    //3 -> Piros oldal

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        lastIndex = templates.lastIndex;
        SRS = GameObject.FindGameObjectWithTag("Rooms").GetComponent<SpawnRoomSpawn>();


        //Index = this.gameObject.GetComponentInChildren<RoomSpawner>().Index;
        if(SRS.isSpawnRoomSpawned == false)
        {
            templates.g.rootNode = new Node(new Vector3(templates.SpawnRoom.transform.position.x,
                                                 templates.SpawnRoom.transform.position.y,
                                                 templates.SpawnRoom.transform.position.z),
                                                 templates.SpawnRoom);
            //templates.g.rootNode.roomPos.x = templates.SpawnRoom.transform.position.x;
            //templates.g.rootNode.roomPos.y = templates.SpawnRoom.transform.position.y;
            //templates.g.rootNode.roomPos.z = templates.SpawnRoom.transform.position.z;
            //templates.g.rootNode.gObject = templates.SpawnRoom;
            SRS.isSpawnRoomSpawned = true;
        }
        else if (templates.isFirstRoomsGenerated.Count < 4)
        {
            for (int i = 0; i < templates.g.roomLists.Count; i++)
            {
                if (templates.g.roomLists[i].Count == 0)
                {
                    Node temp = new Node(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.gameObject, templates.g.rootNode);
                    templates.nodes.Add(temp);
                    templates.isFirstRoomsGenerated.Add(true);
                    templates.g.addEdge(temp);
                    break;
                }
            }
        }
        else
        {
            Node temp = new Node(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.gameObject, templates.nodes[0]);
            templates.nodes.RemoveAt(0);
            templates.nodes.Add(temp);
            templates.g.addEdge(temp);
            for (int i = 0; i < templates.isFirstRoomsGenerated.Count; i++)
            {
                Debug.Log(templates.isFirstRoomsGenerated[i] + ", " + i);
            }
            for (int i = 0; i < templates.nodes.Count; i++)
            {
                Debug.Log(templates.nodes[i].gObject);
            }
        }

        //if(lastIndex < 3)
        //{
        //    lastIndex++;
        //    templates.lastIndex = lastIndex;
        //}
        //else
        //{
        //    lastIndex = 0;
        //    templates.lastIndex = lastIndex;
        //}

        //Node prevTemp = templates.g.findLast(temp1.listIndex);
        //Node temp1 = new Node(new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z), this.gameObject);
        
        //Index = this.gameObject.GetComponentInChildren<RoomSpawner>().Index;
        //RoomTemplates.Node temp2 = new RoomTemplates.Node(Index, this.gameObject);

        //templates.g.addEdge(temp1/*, temp2*/);
        Debug.Log(templates.g.toStr());

        //List<GameObject> temp = new List<GameObject>();
        //temp.Add(this.gameObject);
        //templates.rooms.Add(temp);
    }
}
