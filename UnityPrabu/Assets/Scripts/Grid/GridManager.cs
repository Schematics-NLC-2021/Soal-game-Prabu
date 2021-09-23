using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//to import js  
using System.Runtime.InteropServices;

public class GridManager : MonoBehaviour
{

    [DllImport("__Internal")]
    private static extern void sendScore(int score);

    [Header("Configuration")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject Nirvana;
    [SerializeField] private GameObject InfoBox;
    private int rows = 7;
    private int cols = 7;
    [SerializeField] private float tileSize = 1.84375f;
    [SerializeField] private int totalBomb = 15;
    [SerializeField] private int totalFill = 6;

    private int[,] gameGrid = new int[6,6];
    private int[] bombRow = new int[6];
    private int[] bombCol = new int[6];
    private const int BOMB = 9;
    private TilesManager[] allChildren = new TilesManager[36];
    private int children_id = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        makeGameGrid();
        GenerateUIGrid();
        // Hello();
        // Debug.Log(AddNumbers(4, 7));
    }
    
    private static GridManager instance = null;
    void Awake(){
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }
        Destroy(this.gameObject);
    }

    void makeGameGrid(){
        for(int i = 0;i<rows-1;i++){            //1st phase, set all 0
            for(int j = 0; j < cols-1;j++){
                gameGrid[i, j] = 0;
            }
        }
        randomizeBomb();                        //2nd phase, set all bomb
        fixGrid();                              //3rd phase, fix 0 bomb tiles
        fillGrid();                             //fill tiles with fixed tile
        countColo();                            //calculate bomb col
        countRowo();                            //calculate bomb row
        // printGrid();
    }
    private void GenerateUIGrid()
    {
        
        for(int row = 0;row<rows;row++)
        {
            for(int col = 0;col<cols;col++)
            {
                //0, 0 tiles, the nirvana
                if(row == 0 && col == 0){
                    GameObject Voidy = (GameObject) Instantiate(Nirvana, transform);
                    float posX = col * tileSize;
                    float posY = row * -tileSize;
                    Voidy.transform.position = new Vector2(posX, posY);
                }else if(row == 0){
                    //info bomb of coloumn
                    GameObject InfoCol = (GameObject) Instantiate(InfoBox, transform);
                    InfoCol.name = $"Info: {row}, {col-1}";
                    TilesManager InfoCMng = InfoCol.GetComponent<TilesManager>();
                    InfoCMng.initValue = bombCol[col-1];
                    InfoCMng.isFixed = true;
                    float posX = col * tileSize;
                    float posY = row * -tileSize;
                    InfoCol.transform.position = new Vector2(posX, posY);
                }else if(col == 0){
                    //info bomb row
                    GameObject InfoRow = (GameObject) Instantiate(InfoBox, transform);
                    InfoRow.name = $"Info: {row-1}, {col}";
                    TilesManager InfoRMng = InfoRow.GetComponent<TilesManager>();
                    InfoRMng.initValue = bombRow[row-1];
                    InfoRMng.isFixed = true;
                    float posX = col * tileSize;
                    float posY = row * -tileSize;
                    InfoRow.transform.position = new Vector2(posX, posY);                   
                }else{
                    //creating the tile gameObject
                    GameObject tile = (GameObject) Instantiate(tilePrefab, transform);
                    tile.name = $"tile: {row-1}, {col-1}";
                    TilesManager tileMng = tile.GetComponent<TilesManager>();
                    tileMng.initValue = gameGrid[row-1,col-1];
                    //fixed pos
                    if(tileMng.initValue > 0 && tileMng.initValue < 9){
                        tileMng.isFixed = true;
                        tileMng.score.color = Color.blue;
                    }
                    //mark bomb
                    tileMng.isABomb = tileMng.initValue == BOMB;
                    //set the position
                    float posX = col * tileSize;
                    float posY = row * -tileSize;
                    //transform the tiles to its pos
                    tile.transform.position = new Vector2(posX, posY);
                    //add it to the children array
                    allChildren[children_id] = tileMng;
                    children_id++;
                }
            }
        }
        //center the grid
        float gridW = cols * tileSize;
        float gridH = rows * tileSize;
        float offX = -gridW/2 + tileSize/2;
        float offY = gridH/2 - tileSize/2;
        transform.position = new Vector2(offX, offY);
    }
//-------------------------------------------------------Game Grid Utils--------------------------------------//
    void randomizeBomb(){
        int row, col;
        int total = 0;
        //guaranteed at least one generator
        //row phase
        bool isDone = false;
        for(int i = 0;i<rows-1;i++){
            isDone = false;
            while(!isDone){
                col = Random.Range(0, cols-1);
                if(gameGrid[i,col] == 0){
                    gameGrid[i,col] = BOMB;
                    total++;
                    isDone = true;
                }
            }
        }
        //col phase
        for(int j = 0;j<cols-1;j++){
            isDone = false;
            while(!isDone){
                row = Random.Range(0, rows-1);
                if(gameGrid[row,j] == 0){
                    gameGrid[row,j] = BOMB;
                    total++;
                    isDone = true;
                }
            }
        }
        //random sisa bombu
        while(total < totalBomb){
            row = Random.Range(0, rows-1);
            col = Random.Range(0, cols-1);
            if(gameGrid[row,col] == 0){
                gameGrid[row,col] = BOMB;
                total++;
            }            
        }
    }
    //i = 0, j = 4 
    int calculateBomb(int posi, int posj){
        int total = 0;
        // atas
        if(posi > 0){
            for(int j = posj-1;j<=posj+1;j++){
                if(j >= 0 && j <= 5){
                    if(gameGrid[posi-1,j] == BOMB)
                        total++;
                } 
            }
        }
        //bawah, row-3 == 4
        if(posi <= 4){
            for(int j = posj-1;j<=posj+1;j++){
                if(j >= 0 && j <= 5){
                    if(gameGrid[posi+1,j] == BOMB)
                        total++;
                } 
            }
        }
        //kiri
        if(posj > 0){
            if(gameGrid[posi,posj-1] == BOMB)
                total++;
        }
        //kanan
        if(posj <= 4){
            if(gameGrid[posi,posj+1] == BOMB)
                total++;
        }
        return total;
    }
    void fixGrid(){
        for(int i = 0;i<rows-1;i++){
            for(int j = 0;j<cols-1;j++){
                if(gameGrid[i, j] == 0){
                    //if the bomb around it is 0
                    if(calculateBomb(i, j) == 0){
                        //set it to be a bomb
                        gameGrid[i, j] = BOMB;
                    }
                }
            }
        }
    }
    void fillGrid(){
        int row, col;
        int total = 0;
        while(total < totalFill){
            row = Random.Range(0, rows-1);
            col = Random.Range(0, cols-1);
            if(gameGrid[row,col] == 0){
                gameGrid[row,col] = calculateBomb(row, col);
                total++;
            }
        }
    }
    void countRowo(){
        for(int i = 0;i<rows-1;i++){
            bombRow[i] = 0;
            for(int j = 0;j<cols-1;j++){
                if(gameGrid[i,j] == BOMB)
                    bombRow[i]++;
            }
        }
    }
    void countColo(){
        for(int i = 0;i<cols-1;i++){
            bombCol[i] = 0;
            for(int j = 0;j<rows-1;j++){
                if(gameGrid[j,i] == BOMB)
                    bombCol[i]++;
            }
        }
    }

    public void CalculateScore(){
        // Debug.Log(transform.childCount);
        // Debug.Log(children_id);
        // Debug.Log(totalFill);
        int userScore = totalFill;
        for(int i = 0;i<children_id;i++){
            int relativeRow = i/(rows-1);
            int relativeCol = i%(cols-1);
            if(!allChildren[i].isFixed && !allChildren[i].isABomb){

                // var msg = string.Format("Row : {0}, Col : {1}", relativeRow, relativeCol);
                // Debug.Log(msg);
                int actual_val = calculateBomb(relativeRow, relativeCol);
                if(actual_val == allChildren[i].count){
                    userScore++;
                    // var msg1 = string.Format("Row : {0}, Col : {1}", relativeRow, relativeCol);
                    // Debug.Log(msg1);
                }
            }
            else if(allChildren[i].isABomb && allChildren[i].count == BOMB){
                userScore++;
                // var msg2 = string.Format("Row : {0}, Col : {1}", relativeRow, relativeCol);
                // Debug.Log(msg2);
            }
        }
        var msg3 = string.Format("Your Score : {0}", userScore);
        // Debug.Log(msg3);
        // Debug.Log("Finding score manager");
        FindObjectOfType<ScoreManager>().ShowResult(msg3);
        sendScore(userScore);
    }
    void printGrid(){
        for(int i = 0;i<rows-1;i++){
            for(int j = 0;j<cols-1;j++){
                var msg4 = string.Format("Isi {0}", gameGrid[i, j]);
                Debug.Log(msg4);
            }
            Debug.Log("\n");
        }
    }
}