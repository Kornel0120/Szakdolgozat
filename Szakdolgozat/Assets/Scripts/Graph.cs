using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    public RoomTemplates templates;
    public Node rootNode;
    public List<List<Node>> roomLists = new List<List<Node>>();
    public List<bool> visited = new List<bool>();
    private int V;
    private int E;
        
    public Graph(int V)
    {
        this.V = V;
        this.E = 0;
        for (int i = 0; i < V; i++)
        {
            roomLists.Add(new List<Node>());
        }
    }

    public int getV() { return V; }

    public int getE() { return E; }

    public void addEdge(Node newNode, int index)
    {
        if(newNode.gObject.tag != "SpawnRoom")
            roomLists[index].Add(newNode);
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
}

public class Node
{
    public int listIndex;
    public Vector3 roomPos;
    public GameObject gObject;
    public Node prevNode;

    public Node(Vector3 roomPos, GameObject gObject)
    {
        this.roomPos = roomPos;
        this.gObject = gObject;
    }

    public Node( Vector3 roomPos ,GameObject gObject, Node prevNode, int listIndex)
    {
        this.roomPos = roomPos;
        this.gObject = gObject;
        this.prevNode = prevNode;
        this.listIndex = prevNode.listIndex;
    }

    public int getListIndex() { return listIndex; }

    public GameObject getgObject() { return gObject; }

}