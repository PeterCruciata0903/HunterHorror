using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCSearch_ClassBased : Singleton<NPCSearch_ClassBased>
{
    [SerializeField]
    //public static NPCSearch_ClassBased Instance { get; private set; }
    public string currentStateName;
    private INPCState currentState;
    
    [Header("States")]
    public PatrolState patrolState = new PatrolState();
    public float patrolDistance = 2000f;
    public float patrolSpeed = 15f;
    public StalkState stalkState = new StalkState();
    public float stalkDistance = 100f;
    public float stalkSpeed = 5f;
    public HuntState huntState = new HuntState();
    public float huntDistance = 25f;
    public float huntSpeed = 10f;

    private float Speed;

    [SerializeField]
    public NavMeshAgent navAgent;
    public GameObject self; //references self

    [Header("NextLocation")]
    public Vector3 nextLocation;
    public float nextLocationDistance;

    //What is the gameObject of the nextTarget
    public GameObject nextTarget;

    public Player player; //player gameObject

    [Header("Layers")]
    public LayerMask enemyPoints;

    public float attackDistance = 0.2f;
    public float targetDistance; //updated distance between object and player

    private void OnEnable()
    {

        self = gameObject;
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        currentState = patrolState;
        //player = Player.Instance.gameObject;
        RandomTeleport(self, patrolDistance, false);

        player.playerDeathEvent += playerDies; //Event Subscription
        player.playerKeyEvent += playerGetsKey;
        player.enemyWarpEvent += playerActivatesWarp;
    }

    public float enemySpeed //grab the enemy speed
    {
        get { return Speed; }
        set { Speed = value; }
    }
    void Update()
    {
        //Speed should scale with the number of notes that the player finds
        patrolSpeed = 15f / 5f * ((float)GameRecord.Instance.notesObtained + 1f);
        stalkSpeed = 5f / 5f * ((float)GameRecord.Instance.notesObtained + 1f);
        huntSpeed = 10f / 5f * ((float)GameRecord.Instance.notesObtained + 1f);

        currentState = currentState.DoState(this);
        currentStateName = currentState.ToString();
        targetDistance = Vector3.Distance(self.transform.position, player.transform.position);
    }

    public void playerDies(Player player) //If the subscribed event is sent?
    {
        RandomTeleport(self, patrolDistance, false);
        EnemyAudio.Instance.eatingNoise();
    }
    public void playerGetsKey(Player player) //If the player gets the key, put the player in danger.
    {
        RandomTeleport(self, stalkDistance, true);
        EnemyAudio.Instance.aggressiveNoise();
    }
    public void playerActivatesWarp(Player player)
    {
        RandomTeleport(self, patrolDistance, false);
        EnemyAudio.Instance.aggressiveNoise();
    }
    public void RandomTeleport(GameObject self, float radius, bool close) //randomly teleport to any PatrolPoint in a particular radius
    {
        Debug.Log("RandomTeleportCalled");
        //Index through all enemyPoints
        List<Collider> teleportPoints = new List<Collider>();
        Collider[] colliders = Physics.OverlapSphere(self.transform.position, radius, enemyPoints, QueryTriggerInteraction.Collide);
        foreach (Collider interactive in colliders)
        {
            if (interactive.tag == "PatrolPoint" && close == false)
                teleportPoints.Add(interactive);
            if (interactive.tag == "SpawnPoint" && close == true)
                teleportPoints.Add(interactive);
        }
            

        int val = Random.Range(0, teleportPoints.Count-1);
        Transform teleport = teleportPoints[val].transform;
        self.transform.position = teleport.position;
    }
}
