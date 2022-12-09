using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene("Maze");
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void QuitGame()
    {
        Debug.Log("I am quitting this game");
        Application.Quit();
    }

    public void Victory()
    {
        SceneManager.LoadScene("VictoryScene");
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("Enemy"));
        Destroy(GameObject.Find("GameRecord"));
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Lose()
    {
        SceneManager.LoadScene("LoseScene");
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("Enemy"));
        Destroy(GameObject.Find("GameRecord"));
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}
