using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemsGenerate : MonoBehaviour
{
    [HideInInspector]
    public Vector2Int posIndex;
    [HideInInspector]
    public Board board;
    private Vector2 firstTouchPos;
    private Vector2 finalTouchPos;
    private bool mousePressed;
    private float swipeAngle =0;
    private GemsGenerate swipedGem;
    public enum GemType { blue,dark, green,  purple, red}
    public GemType type;
    public bool isMatched;
    
    private Vector2Int previousPos;
    public int scoreValue = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position,posIndex)>.01f){
            transform.position = Vector2.Lerp(transform.position,posIndex,board.gemSpeed*Time.deltaTime);
        }else{
            transform.position = new Vector3(posIndex.x,posIndex.y,0f);
            board.allGems[posIndex.x, posIndex.y] = this;
        }
           
        if(mousePressed && Input.GetMouseButtonUp(0)){
            mousePressed = false;
            if (board.currentState == Board.BoardState.move && board.roundMan.roundTime>0){
                finalTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            AngleCalculation();
            }
            
        }
    }
    public void GenerateGem(Vector2Int pos,Board theBoard){
        posIndex = pos;
        board = theBoard;
    }
    private void OnMouseDown(){

        if (board.currentState == Board.BoardState.move && board.roundMan.roundTime>0)
        {
        firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePressed = true;
        }
    }
    private void AngleCalculation(){
        swipeAngle =Mathf.Atan2(finalTouchPos.y-firstTouchPos.y,finalTouchPos.x-firstTouchPos.x);
        swipeAngle =swipeAngle * 180/Mathf.PI;
        if(Vector3.Distance(firstTouchPos,finalTouchPos)>.5f){
            moveGems();
        }
        
    }
    private void moveGems(){
        previousPos = posIndex;
        if(swipeAngle < 45 && swipeAngle > -45 && posIndex.x < board.width - 1)
        {
            swipedGem = board.allGems[posIndex.x + 1, posIndex.y];
            swipedGem.posIndex.x--;
            posIndex.x++;
        } else if (swipeAngle > 45 && swipeAngle <= 135 && posIndex.y < board.height - 1)
        {
            swipedGem = board.allGems[posIndex.x, posIndex.y + 1];
            swipedGem.posIndex.y--;
            posIndex.y++;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && posIndex.y > 0)
        {
            swipedGem = board.allGems[posIndex.x, posIndex.y - 1];
            swipedGem.posIndex.y++;
            posIndex.y--;
        } else if (swipeAngle > 135 || swipeAngle < -135 && posIndex.x > 0)
        {
            swipedGem = board.allGems[posIndex.x - 1, posIndex.y];
            swipedGem.posIndex.x++;
            posIndex.x--;
        }

        board.allGems[posIndex.x,posIndex.y] = this;
        board.allGems[swipedGem.posIndex.x,swipedGem.posIndex.y]= swipedGem;

        StartCoroutine(checkMove());
    }
    public IEnumerator checkMove(){
        board.currentState = Board.BoardState.wait;
        yield return new WaitForSeconds(.5f);
        board.match.findAllMatches();

        if(swipedGem != null){
            if(!isMatched && !swipedGem.isMatched){
                swipedGem.posIndex = posIndex;
                posIndex = previousPos;

                board.allGems[posIndex.x,posIndex.y] = this;
                board.allGems[swipedGem.posIndex.x,swipedGem.posIndex.y]= swipedGem;
                yield return new WaitForSeconds(.5f);
                board.currentState = Board.BoardState.move;
            }
            else{
                board.destroyMatches();
            }
        }
    }
}
