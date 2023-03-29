using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Match : MonoBehaviour
{
    private Board board;
    public List<GemsGenerate> currentMatches = new List<GemsGenerate>();
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake(){
        board = FindObjectOfType<Board>();
    }
    public void findAllMatches(){
         currentMatches.Clear();
        for(int x =0;x<board.width;x++){
            for(int y=0;y<board.height;y++){
                GemsGenerate currentGem = board.allGems[x,y];
                if(currentGem !=null){
                    if(x>0 && x<board.width -1){
                        GemsGenerate leftGem = board.allGems[x-1,y];
                        GemsGenerate rightGem = board.allGems[x+1,y];
                        if(leftGem !=null && rightGem !=null){
                            if(leftGem.type == currentGem.type && rightGem.type == currentGem.type){
                                currentGem.isMatched = true;
                                leftGem.isMatched = true;
                                rightGem.isMatched = true;

                                currentMatches.Add(currentGem);
                                currentMatches.Add(leftGem);
                                currentMatches.Add(rightGem);
                            }
                        }
                    }
                    if(y>0 && y<board.height -1){
                        GemsGenerate topGem = board.allGems[x,y+1];
                        GemsGenerate downGem = board.allGems[x,y-1];
                        if(topGem !=null && downGem !=null){
                            if(topGem.type == currentGem.type && downGem.type == currentGem.type){
                                currentGem.isMatched = true;
                                topGem.isMatched = true;
                                downGem.isMatched = true;

                                currentMatches.Add(currentGem);
                                currentMatches.Add(topGem);
                                currentMatches.Add(downGem);
                            }
                        }
                    }
                }
            }
        }
        if(currentMatches.Count >0){
            currentMatches =currentMatches.Distinct().ToList();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
