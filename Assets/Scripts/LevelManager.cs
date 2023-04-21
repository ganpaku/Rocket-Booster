using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    CollisionHandler m_collisionHandler;

    void Start()
    {
        m_collisionHandler = FindObjectOfType<CollisionHandler>();
    }

    void Update()
    {
        QuickReload();
        QuickLoadNextLevel();
        LoadMenu();

    }

    void QuickReload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            m_collisionHandler.ReloadLevel();
        }
    }
    
    void QuickLoadNextLevel()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            m_collisionHandler.LoadNextLevel();
        }
    }


    void LoadMenu()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Go to menu");
            SceneManager.LoadScene(0);
        }
    }

}
