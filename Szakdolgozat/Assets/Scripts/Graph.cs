using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    public RoomTemplates templates;
    //public static GameObject gObject;
    //private static Vector3 roomPos;
    public Node rootNode;/*= new Node(roomPos, gObject);*/
    public List<List<Node>> roomLists = new List<List<Node>>();
    public List<bool> visited = new List<bool>();
    private int V;
    private int E;
//templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        
    public Graph(int V)
    {
        
        //this.rootNode = new Node(new Vector3(templates.SpawnRoom.transform.position.x,
        //                                     templates.SpawnRoom.transform.position.y,
        //                                     templates.SpawnRoom.transform.position.z),
        //                                     templates.SpawnRoom);
        this.V = V;
        this.E = 0;
        for (int i = 0; i < V; i++)
        {
            roomLists.Add(new List<Node>());
        }
    }

    public int getV() { return V; }

    public int getE() { return E; }

    public void addEdge(Node newNode)
    {
        if(newNode.gObject.tag != "SpawnRoom")
            roomLists[newNode.listIndex].Add(newNode);
        E++;
    }

    public Node DFS(int V, GameObject g)
    {
        if (visited[V] != true)
        {
            for (int i = 0; i < roomLists.Count; i++)
            {
                for (int j = 0; j < roomLists[i].Count; j++)
                {
                    if (roomLists[i][j].gObject == g)
                    { 
                        visited.Clear();
                        return roomLists[i][j]; 
                    }
                        
                }
            }
        }
        visited[V] = true;
        if (V < templates.g.roomLists.Count)
            return DFS(V + 1, g);
        else
        {
            visited.Clear();
            return null;
        }
            
    }


    public void DeleteFromGraph(int V,Node g)
    {
        if (visited[V] != true)
        {
            for (int i = 0; i < roomLists.Count; i++)
            {
                for (int j = 0; j < roomLists[i].Count; j++)
                {
                    if (roomLists[i][j] == g)
                       roomLists[i].Remove(roomLists[i][j]);
                }
            }
        }
        visited[V] = true;
        if (V < templates.g.roomLists.Count)
            DeleteFromGraph(V + 1, g);
        else
            Debug.Log("Nincs a gráfban!");
    }

    public Node findLast(int Index)
    {
        return roomLists[Index][roomLists[Index].Count];
    }

    public string toStr()
    {
        string s = V.ToString() + "vertices, " + E.ToString() + "edges\n";
        for (int v = 0; v < V; v++)
        {
            s += v + ":";
            foreach (Node w in this.roomLists[v])
                s += w.gObject.ToString() + ", ";
            s += "Rooms: " + roomLists[v].Count;
            s += '\n';
            
        }
        return s;
    }

    #region netes
    //private int V;
    //private int E;
    //private List<List<GameObject>> adj = new List<List<GameObject>>();

    //public Graph(int V0)
    //{
    //    V = V0;
    //    E = 0;
    //    for (int v = 0; v < V0; v++)
    //        adj.Add(new List<GameObject>());
    //}

    //public int getV() { return V; }
    //public int getE() { return E; }

    //public void addEdge(Node v/*, Node w*/)
    //{
    //    int tempi = v.getIndex();
    //    //GameObject tempg = w.getgObject();
    //    adj[v.getIndex()].Add(v.getgObject());
    //    /*adj[w.getIndex()].Add(v.getgObject());*/
    //    E++;
    //}

    //public GameObject FindLast(int Index)
    //{
    //    GameObject last = adj[Index][adj[Index].Count];
    //    return last;
    //}

    //public void deleteLast(int Index)
    //{
    //    adj[Index].RemoveAt(adj[Index].Count);
    //}

    //public string toStr()
    //{
    //    string s = V.ToString() + "vertices, " + E.ToString() + "edges\n";
    //    for (int v = 0; v < V; v++)
    //    {
    //        s += v + ":";
    //        foreach (GameObject w in this.adj[v])
    //            s += w.ToString() + ", ";
    //        s += '\n';
    //    }
    //    return s;
    //}
    #endregion
}

public class Node
{
    RoomTemplates templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    public int listIndex;
    public Vector3 roomPos;
    public GameObject gObject;
    private Node prevNode;

    public Node(Vector3 roomPos, GameObject gObject)
    {
        this.roomPos = roomPos;
        this.gObject = gObject;
    }

    public Node( Vector3 roomPos ,GameObject gObject, Node prevNode)
    {
        this.roomPos = roomPos;
        this.gObject = gObject;
        this.prevNode = prevNode;
        if (this.prevNode.gObject.tag == "SpawnRoom")
            listIndex = templates.isFirstRoomsGenerated.Count;
        else if(!templates.invalidIndexes.Contains(listIndex))
            listIndex = prevNode.listIndex;
    }

    public int getListIndex() { return listIndex; }

    public GameObject getgObject() { return gObject; }

}