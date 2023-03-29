using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float roundTime = 120f;
    private UIManager uiMan;

    private Board board;

    public int currentScore;
    private bool endRound =false;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        uiMan = FindObjectOfType<UIManager>();
        board = FindObjectOfType<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        if(roundTime > 0)
        {
            roundTime -= Time.deltaTime;

            if(roundTime <= 0)
            {
                roundTime = 0;
                endRound = true;
            }
        }
        if(endRound && board.currentState == Board.BoardState.move){
            winCheck();
            endRound = false;
        }
        uiMan.timeText.text = roundTime.ToString("0.0") + "s";
        uiMan.scoreText.text = currentScore.ToString();
    }
    private void winCheck(){
        uiMan.roundOverScreen.SetActive(true);
        uiMan.winScore.text = currentScore.ToString();
    }
}
