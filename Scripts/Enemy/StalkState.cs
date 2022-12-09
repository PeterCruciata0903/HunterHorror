using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class StalkState : INPCState
{
    //Randomly moves between local dead ends. If Player sprints, then it will go to the last place the player moved. If it gets too close, then it will hunt.
    List<Collider> stalkPoints = new List<Collider>();
    int val;
    GameObject stalk; // what transform to stalk towards
    public INPCState DoState(NPCSearch_ClassBased npc)
    {
       // Debug.Log("StalkState");

        if (npc.navAgent == null)
            npc.navAgent = npc.GetComponent<NavMeshAgent>();

        stalk = RandomStalkMove(npc, npc.stalkDistance);

        //If player is sprinting, the hunter can notice and change behavior. Else, it will browse the local endpoints randomly.
        if (PlayerMovement.Instance.playerSpeed > PlayerMovement.Instance.walkSpeed+1)
        {
            //Samples the last place the player Sprinted and moves towards it.
            EnemyAudio.Instance.stalkingNoise();
            npc.navAgent.destination = npc.player.transform.position;
            npc.navAgent.speed = npc.stalkSpeed;
            npc.enemySpeed = npc.stalkSpeed;

        }
        else
        {
            EnemyAudio.Instance.clickingNoise();
            npc.navAgent.destination = stalk.transform.position;
            npc.navAgent.speed = npc.huntSpeed;
            npc.enemySpeed = npc.huntSpeed;
        }

        if ((npc.targetDistance > npc.stalkDistance))
            return npc.patrolState;
        else if (npc.targetDistance < npc.huntDistance)
            return npc.huntState;
        else
            return npc.stalkState;
    }
    public GameObject RandomStalkMove(NPCSearch_ClassBased npc, float radius) //returns a random Spawnpoint gameObject
    {
        if (stalkPoints == null)
        {
            Collider[] colliders = Physics.OverlapSphere(npc.transform.position, radius, npc.enemyPoints, QueryTriggerInteraction.Collide); //take in all trigger colliders
            foreach (Collider interactive in colliders)
                if (interactive.tag == "SpawnPoint") //gameObject must be tagged SpawnPoint
                    stalkPoints.Add(interactive);
            val = Random.Range(0, stalkPoints.Count-1);
            stalk = stalkPoints[val].gameObject; //gameObject of randomStalkPoint
            if (Vector3.Distance(npc.self.transform.position,stalkPoints[val].gameObject.transform.position) < 0.5f)
            {
                //If we are overlapping with the patrol objective, reset the patrol
                EnemyAudio.Instance.stalkingNoise(); // every time it reaches a patrol location, play the stalking Noise
                RandomStalkMove(npc, radius);
            } 
            return stalk;
        }
        else
            return npc.player.gameObject; //If there are no Spawnpoints in the radius, then return the player GameObject
    }
}
