using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    
    public TMP_Text timeText;
    public TMP_Text scoreText;
    private Board theBoard;
    public GameObject roundOverScreen;
    public TMP_Text winScore;
    public TMP_Text winText;
    public GameObject pauseScreen;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PauseUnpause()
    {
        if(!pauseScreen.activeInHierarchy)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        } else
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }
    }

   

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("mainMenu");
    }


    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }
}
