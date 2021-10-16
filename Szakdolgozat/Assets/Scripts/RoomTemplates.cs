using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject SpawnRoom;
    public GameObject KeyRoom;
    public GameObject CheckPointRoom;
    public GameObject TrapRoom;

    public List<List<GameObject>> rooms = new List<List<GameObject>>();
    //public List<GameObject> rooms;
    //public List<Vector3> targetPostitions = new List<Vector3>();
    public List<Vector3> positions = new List<Vector3>();
    public Graph g = new Graph(4);
    public int Index = 0;

    public class Graph
    {
        private int V;
        private int E;
        private List<List<GameObject>> adj = new List<List<GameObject>>();

        public Graph(int V0)
        {
            V = V0;
            E = 0;
            for (int v = 0; v < V0; v++)
                adj.Add(new List<GameObject>());
        }

        public int getV() { return V; }
        public int getE() { return E; }

        public void addEdge(Node v/*, Node w*/)
        {
            int tempi = v.getIndex();
            //GameObject tempg = w.getgObject();
            adj[v.getIndex()].Add(v.getgObject()); 
            /*adj[w.getIndex()].Add(v.getgObject());*/
            E++;
        }

        public GameObject FindLast(int Index)
        {
            GameObject last = adj[Index][adj[Index].Count];
            return last;
        }

        public void deleteLast(int Index)
        {
            adj[Index].RemoveAt(adj[Index].Count);
        }

        public string toStr()
        {
            string s = V.ToString() + "vertices, " + E.ToString() + "edges\n";
            for (int v = 0; v < V; v++)
            {
                s += v + ":";
                foreach (GameObject w in this.adj[v])
                    s += w.ToString() + ", ";
                s += '\n';
            }
            return s;
        }
    }

    public class Node
    {
        private int Index;
        private GameObject gObject;

        public Node(int Index, GameObject gObject)
        {
            this.Index = Index;
            this.gObject = gObject;
        }


        public int getIndex() { return Index; }
        
        public GameObject getgObject() { return gObject; }

    }

}


