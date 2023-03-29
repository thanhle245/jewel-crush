using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class menu : MonoBehaviour
{
    public string level;
    public void startGame(){
        SceneManager.LoadScene(level);
    }
    public void quitGame(){
        Application.Quit();
    }
}
