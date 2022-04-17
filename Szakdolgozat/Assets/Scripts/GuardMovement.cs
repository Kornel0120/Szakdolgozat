using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardMovement : MonoBehaviour
{
    private RoomTemplates templates;

    public NavMeshAgent agent;
    public GameObject player;

    public byte stage;

    public List<Vector3> PatrolLocations = new List<Vector3>();

    public int Index = 0;
    public Vector3 currentLocation;

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        player = GameObject.FindGameObjectWithTag("Player");
        PatrolLocations = FindEndLocation(stage);
        agent.SetDestination(PatrolLocations[Index]);
        currentLocation = PatrolLocations[Index];
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(agent.transform.position, PatrolLocations[Index]) < 1f)
        {
            if (Index == PatrolLocations.Count - 1)
            {
                Index = 0;
                currentLocation = PatrolLocations[Index];
                agent.SetDestination(PatrolLocations[Index]);
            }
            else
            {
                Index++;
                currentLocation = PatrolLocations[Index];
                agent.SetDestination(PatrolLocations[Index]);
            }
        }

        else if(PlayerHit(this.gameObject.transform.position, 1f) == true)
        {
            agent.SetDestination(player.transform.position);
        }
        else if(PlayerHit(this.gameObject.transform.position, 1f) == false)
        {
            agent.SetDestination(PatrolLocations[Index]);
        }
    }

    private bool PlayerHit(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);

        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Player"))
                return true;
        }
        return false;
    }

    private List<Vector3> FindEndLocation(byte stage)
    {
        List<Vector3> agentPatrolLocations = new List<Vector3>();
        List<GameObject> TargetLocations = new List<GameObject>(GameObject.FindGameObjectsWithTag("TrapRoom"));
        TargetLocations.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("KeyRoom")));
        TargetLocations.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("CheckPointRoom")));
        TargetLocations.Add(GameObject.FindGameObjectWithTag("FinishRoom"));
        TargetLocations.Add(GameObject.FindGameObjectWithTag("SpawnRoom"));

        if (stage == 1)
        {
            foreach (GameObject gameObject in TargetLocations)
            {
                if (gameObject.GetComponent<AddRoom>().stage == stage)
                    agentPatrolLocations.Add(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.05000001f, gameObject.transform.position.z));
            }
        }
        else if(stage == 2)
        {
            foreach (GameObject gameObject in TargetLocations)
            {
                if (gameObject.GetComponent<AddRoom>().stage == stage)
                    agentPatrolLocations.Add(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.05000001f, gameObject.transform.position.z));
            }
        }
        else if(stage == 3)
        {
            foreach (GameObject gameObject in TargetLocations)
            {
                if (gameObject.GetComponent<AddRoom>().stage == stage)
                    agentPatrolLocations.Add(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.05000001f, gameObject.transform.position.z));
            }
        }

        return agentPatrolLocations;
    }
}
