using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : INPCState
{
    public INPCState DoState(NPCSearch_ClassBased npc)
    {
        Debug.Log("PatrolState");

        if (npc.navAgent == null)
            npc.navAgent = npc.GetComponent<NavMeshAgent>();

        RandomPatrolMove(npc, npc.patrolDistance);

        if (npc.targetDistance > npc.stalkDistance) //patrol range
            return npc.patrolState;
        else if (npc.targetDistance < npc.stalkDistance && npc.targetDistance > npc.huntDistance) // stalking interval
            return npc.stalkState;
        else //hunt range
            return npc.huntState;
    }
    void RandomPatrolMove(NPCSearch_ClassBased npc, float radius)
    {
        List<Collider> patrolPoints = new List<Collider>();
        int val;
        Transform patrol;

        Debug.Log("RandomPatrolMove");

        if (patrolPoints == null)
        {
            Collider[] colliders = Physics.OverlapSphere(npc.transform.position, radius, npc.enemyPoints, QueryTriggerInteraction.Collide);
            foreach (Collider interactive in colliders)
                if (interactive.tag == "PatrolPoint") //game object must be named PatrolPoint
                    patrolPoints.Add(interactive);

            val = Random.Range(0, patrolPoints.Count - 1);
            patrol = patrolPoints[val].transform;

            npc.navAgent.destination = patrol.position;
            npc.navAgent.speed = npc.patrolSpeed;
            npc.enemySpeed = npc.patrolSpeed;

            if (Vector3.Distance(npc.self.transform.position,patrolPoints[val].gameObject.transform.position) < 0.5f)
                RandomPatrolMove(npc, radius); //If we are overlapping with the patrol objective, reset the patrol
        }
    }
}
