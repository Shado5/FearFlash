using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    //loads tutorial scene
    public void Tutorial()
    {
        SceneManager.LoadScene("Cutscene");
    }

    //quits game
    public void Quit()
    {
        Application.Quit();
    }

    //loads level 2
    public void Level2()
    {
        SceneManager.LoadScene("Level2");
    }
    
}

