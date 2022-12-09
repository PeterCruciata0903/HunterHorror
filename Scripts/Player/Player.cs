using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Singleton<Player>
{
    public delegate void playerDeathDelegate(Player p);
    public event playerDeathDelegate playerDeathEvent;
    public event playerDeathDelegate playerKeyEvent;
    public event playerDeathDelegate enemyWarpEvent;

    public int maxHealth = 5;
    public int currentHealth;
    public Text keys;

    public HealthScript healthScript;

    public List<GameObject> bag;
    [SerializeField]
    LayerMask enemy;

    Ray ray;
    public Camera camera;

    //void enemyCollision()
    //{
    //    Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position,2,enemy);
    //    foreach (Collider collider in colliders)
    //        if (collider.tag == "Enemy")
    //            getHurt();
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.tag == "Enemy")
    //    {
    //        getHurt();
    //        Debug.Log("Ow your hurting me");
    //    }
    //}
    //void getHurt()
    //{
    //    playerDeathEvent(this); //Calls for the enemy to teleport away from the player
    //    currentHealth -= 1;
    //    PlayerAudio.Instance.playerHurt();
    //}

    void enemyCollision()
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position,2,enemy);
        foreach (Collider collider in colliders)
            if (collider.tag == "Enemy")
                Die();
    }


    void Die()
    {
        Debug.Log("MotherFucker OW");
        playerDeathEvent(this); //Calls for the enemy to teleport away from the player
        currentHealth -= 1;
        PlayerAudio.Instance.playerHurt();
        
    }

    public void Grab()
    {
        if (Input.GetKeyDown(KeyCode.E)) //Now the e key is the grab trigger
        {
            ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); //center of the screen is the location of the ray.
            grabObject();
            return;
        }
    }

    void grabObject()
    {
        

        RaycastHit hit;
        //check if there is a hit between object and ray
        if (Physics.Raycast(ray, out hit, 6))
        {
            if (hit.collider.gameObject.tag == "Key") //Keys are tagged
            {
                playerKeyEvent(this);//Connects to the player key event which causes the enemy to react.
                bag.Add(hit.collider.gameObject);
                GameRecord.Instance.notesObtained += 1;
                hit.collider.gameObject.SetActive(false);
                return;
            }
            if(hit.collider.gameObject.tag == "Magic_Stone")
            {
                enemyWarpEvent(this);
                bag.Add(hit.collider.gameObject);
                hit.collider.gameObject.SetActive(false);
                return;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        ray = camera.ScreenPointToRay(Input.mousePosition);

        Grab();
    }
    

    void Start()
    {
        currentHealth = maxHealth;
        healthScript.SetMaxHealth(maxHealth);

        
        GameObject scoreGO = GameObject.Find("KeyCounter"); //Note: Weir feels uncomfortable by this
        //Get the Text Component of that Game Object
        keys = scoreGO.GetComponent<Text>();
        //Set the starting number of points to 0
        keys.text = "0";
    }
}
