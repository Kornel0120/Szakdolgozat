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
    //public GameObject BottomStairsRoom;
    //public GameObject TopStairsRoom;
    //public GameObject LeftStairsRoom;
    //public GameObject RightStairsRoom;
    //public GameObject intersectionRoom;
    //public GameObject closedRoom;

    public List<List<GameObject>> rooms = new List<List<GameObject>>();
    //public List<GameObject> rooms;
    //public List<Vector3> targetPostitions = new List<Vector3>();
    public List<Vector3> positions = new List<Vector3>();
    public Graph g = new Graph(4);

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

        public void addEdge(Node v, Node w)
        {
            int tempi = v.getIndex();
            GameObject tempg = w.getgObject();
            adj[v.getIndex()].Add(w.getgObject());   // Add w to v`s list
            adj[w.getIndex()].Add(v.getgObject());   // add v to w`s list
            E++;
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

        //public int setIndex() { return Index++; }
        
        public GameObject getgObject() { return gObject; }

        //public GameObject setgObject(GameObject gObj) {
        //    gObject = gObj;
        //    return gObject; }

    }

}


