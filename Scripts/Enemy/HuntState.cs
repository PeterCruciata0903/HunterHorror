using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HuntState : INPCState
{
    //Chase Sequence, play hunting music, and teleport if player manages to get far enough away.
    public INPCState DoState(NPCSearch_ClassBased npc)
    {
        if (npc.navAgent == null)
            npc.navAgent = npc.GetComponent<NavMeshAgent>();
        attackCritter(npc);

        if (npc.targetDistance > 100)
        {
            npc.RandomTeleport(npc.self, npc.patrolDistance, false);
            return npc.patrolState;
        }
        else
            return npc.huntState;
    }
    
    private void attackCritter(NPCSearch_ClassBased npc)
    {
        if(npc.navAgent.destination != npc.player.transform.position)
        {
            npc.navAgent.SetDestination(npc.player.transform.position);
            EnemyAudio.Instance.aggressiveNoise();
        }
        npc.navAgent.speed = npc.huntSpeed;
        npc.enemySpeed = npc.huntSpeed;
    }
}
