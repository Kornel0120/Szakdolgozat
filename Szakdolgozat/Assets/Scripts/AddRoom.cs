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

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();        
        SRS = GameObject.FindGameObjectWithTag("Rooms").GetComponent<SpawnRoomSpawn>();
        Invoke("addRoom", 0.1f);
    }

    void addRoom()
    {
        //Index = this.gameObject.GetComponentInChildren<RoomSpawner>().Index;
        lastIndex = templates.lastIndex;
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
            if(templates.nodes.Count != 0)
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
        Debug.Log(templates.g.toStr());

        
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
        
        

        //List<GameObject> temp = new List<GameObject>();
        //temp.Add(this.gameObject);
        //templates.rooms.Add(temp);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "Room" && other.gameObject.tag == "Wall" && other.gameObject.tag != "RoomSpawnPoint")
        {
            Node temp;
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!COLLIDE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + gameObject.name);
            foreach (var node in templates.nodes)
            {
                Debug.Log("Nodes lista: "+ node.gObject);
            }
            
            Destroy(this.gameObject);
            if(templates.nodes.Count != 0)
                templates.nodes.RemoveAt(templates.nodes.Count-1);
            //for (int i = 0; i < templates.nodes.Count; i++)
            //{
            //        temp = templates.nodes[i];
            //        Debug.Log("Törölt elem: " + temp.gObject);
            //        templates.nodes.Remove(temp);
            //        break;
            //        Debug.Log("Nodes Lista: " + templates.nodes[i].gObject);
            //}
            //Destroy(gameObject);

            //foreach(Node node in templates.nodes)
            //{
            //    if (node.gObject == gameObject)
            //    {
            //        temp = node;
            //        Debug.Log("Törölt elem: {0}", temp.gObject);
            //        templates.nodes.Remove(temp);
            //        break;
            //    }
            //    Debug.Log("Nodes Lista: {0}", node.gObject);
            //}



        }
    }
}
