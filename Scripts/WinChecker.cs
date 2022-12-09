using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinChecker : MonoBehaviour
{

    
    public GameObject[] Locker;
    public Scene scene;
    // Start is called before the first frame update
    public int cLocked = 5;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            List<GameObject> toRemove = new List<GameObject>();
            foreach (GameObject b in Player.Instance.bag)
            {
                if (b.tag == "Key")
                {
                    toRemove.Add(b);
                }
            }
            foreach (GameObject b in toRemove)
            {
                Player.Instance.bag.Remove(b);
                Locker[cLocked - 1].SetActive(true);
                cLocked -= 1;
            }

            if (cLocked == 0)
            {
                scene.Victory();
                Debug.Log("win!!!");
            }
        }
    }

}
