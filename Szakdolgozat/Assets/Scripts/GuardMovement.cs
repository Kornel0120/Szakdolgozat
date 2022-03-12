using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;
    RaycastHit playerHit;

    private Vector3 PatrolStartLocation;
    private Vector3 PatrolEndLocation;
    private bool agentAtStartLocation = false;
    private bool agentAtEndLocation = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PatrolStartLocation = this.transform.position;
        agentAtStartLocation = true;
        PatrolEndLocation = FindEndLocation(this.transform.position, 5f);
    }

    void Update()
    {
        if (this.transform.position == PatrolStartLocation)
        {
            agentAtStartLocation = true;
            agentAtEndLocation = false;
        }
        else if (this.transform.position == PatrolEndLocation)
        {
            agentAtEndLocation = true;
            agentAtStartLocation = false;
        }

        if (PlayerHit(this.transform.position, 1f) == true)
            agent.SetDestination(player.transform.position);
        else if (agentAtStartLocation == true)
            agent.SetDestination(PatrolEndLocation);
        else if (agentAtEndLocation == false)
            agent.SetDestination(PatrolStartLocation);
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

    private Vector3 FindEndLocation(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        float FurthestDistance = radius;
        Vector3 EndPosition = Vector3.zero;


        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Room"))
            {
                float Distance = Vector3.Distance(this.transform.position, collider.gameObject.transform.position);
                if (Distance > FurthestDistance)
                {
                    EndPosition = collider.transform.position;
                    FurthestDistance = Distance;
                }
            }
        }

        return EndPosition;
    }
}
