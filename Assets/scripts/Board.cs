using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject bgPrefab;
    public GemsGenerate[] gems;

    public GemsGenerate[,] allGems;
    public float gemSpeed;
    [HideInInspector]
    public Match match;
    public enum BoardState {  wait, move}
    public BoardState currentState = BoardState.move;
    [HideInInspector]
    public GameManager roundMan;
    void Start()
    {
        allGems =new GemsGenerate[width,height];
        Generate();
        
    }
    private void Awake(){
        match = FindObjectOfType<Match>();
        roundMan = FindObjectOfType<GameManager>();
    }
   
    private void Generate(){

        for(int x =0;x<width;x++){
            for(int y=0;y<height;y++){
                Vector2 pos = new Vector2(x,y);
                GameObject bgTitle = Instantiate(bgPrefab,pos,Quaternion.identity);
                bgTitle.transform.parent = transform;
                bgTitle.name="bg-"+x+"-"+y;
                int gemChecked = Random.Range(0,gems.Length);
                int interations =0;
                while(matchAt(new Vector2Int(x,y),gems[gemChecked]) && interations <100){
                    gemChecked = Random.Range(0,gems.Length);
                    interations++;
                    
                }
                SpawnGem(new Vector2Int(x,y),gems[gemChecked]);

            }
            
        }
    }
    private void SpawnGem(Vector2Int pos,GemsGenerate gemSpawn){
        GemsGenerate gem = Instantiate(gemSpawn,new Vector3(pos.x,pos.y + height,0f),Quaternion.identity);
        gem.transform.parent = transform;
        gem.name ="gem"+pos.x+"-"+pos.y;
        allGems[pos.x,pos.y]=gem;
        gem.GenerateGem(pos,this);
    }
    bool matchAt(Vector2Int posToCheck,GemsGenerate gemToCheck){
        if(posToCheck.x >1){
            if(allGems[posToCheck.x-1,posToCheck.y].type == gemToCheck.type && allGems[posToCheck.x-2,posToCheck.y].type == gemToCheck.type){
                return true;
            }
        }
        if(posToCheck.y >1){
            if(allGems[posToCheck.x,posToCheck.y-1].type == gemToCheck.type && allGems[posToCheck.x,posToCheck.y-2].type == gemToCheck.type){
                return true;
            }
        }
         return false;
    }
    private void destroyMatchedAt(Vector2Int pos){
        if(allGems[pos.x,pos.y] != null){
            if(allGems[pos.x,pos.y].isMatched){
                Destroy(allGems[pos.x,pos.y].gameObject);
                allGems[pos.x,pos.y] = null;
            }
            
        }
    }
    public void destroyMatches(){
        for(int i=0;i<match.currentMatches.Count;i++){
            if(match.currentMatches[i] !=null){
                scoreCheck(match.currentMatches[i]);
                destroyMatchedAt(match.currentMatches[i].posIndex);

            }
        }
        StartCoroutine(DecreaseRow());
    }

    private IEnumerator DecreaseRow(){
        yield return new WaitForSeconds(.2f);
        int nullCounter = 0;
        for(int x =0;x<width;x++){
            for(int y=0;y<height;y++){
                if(allGems[x,y]==null){
                    nullCounter++;
                }else if(nullCounter>0){
                    allGems[x,y].posIndex.y -= nullCounter;
                    allGems[x,y - nullCounter] = allGems[x,y];
                    allGems[x,y] = null;
                }
            }   
            nullCounter = 0;
        }
        StartCoroutine(FillBoard());
        
    }

    private IEnumerator FillBoard(){
        yield return new WaitForSeconds(.5f);
        ReFillBoard();
        yield return new WaitForSeconds(.5f);
        match.findAllMatches();
        if(match.currentMatches.Count >0){
            yield return new WaitForSeconds(1f);
            destroyMatches();
        }else{
            yield return new WaitForSeconds(.5f);
            currentState =BoardState.move;
        }
    }
    private void ReFillBoard(){
        for(int x =0;x<width;x++){
            for(int y=0;y<height;y++){
                if(allGems[x,y] == null){
                    int gemChecked= Random.Range(0,gems.Length);
                    SpawnGem(new Vector2Int(x,y),gems[gemChecked]);
                }
                
            }
        }
        checkMisplacedGems();
    }
    private void checkMisplacedGems(){
        List<GemsGenerate> foundGems = new List<GemsGenerate>();

        foundGems.AddRange(FindObjectsOfType<GemsGenerate>());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(foundGems.Contains(allGems[x,y]))
                {
                    foundGems.Remove(allGems[x, y]);
                }
            }
        }

        foreach(GemsGenerate g in foundGems)
        {
            Destroy(g.gameObject);
        }
    }

    public void scoreCheck(GemsGenerate gemToCheck){
        roundMan.currentScore += gemToCheck.scoreValue;
        
    }
}
