using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    //[SerializeField] float waitTime = 2f;
    // Start is called before the first frame update
    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        
    }
    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }


}
