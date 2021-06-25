using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject Nirvana;
    [SerializeField] private GameObject InfoBox;
    private int rows = 7;
    private int cols = 7;
    [SerializeField] private float tileSize = 1;
    [SerializeField] private int totalBomb = 15;
    [SerializeField] private int totalFill = 6;

    private int[,] gameGrid = new int[6,6];
    private int[] bombRow = new int[6];
    private int[] bombCol = new int[6];
    private const int BOMB = 9;
    
    // Start is called before the first frame update
    void Start()
    {
        makeGameGrid();
        GenerateUIGrid();
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
    }
    private void GenerateUIGrid()
    {
        for(int row = 0;row<rows;row++)
        {
            for(int col = 0;col<cols;col++)
            {
                if(row == 0 && col == 0){
                    //tbc
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
                        tileMng.score.color = Color.red;
                    }
                    //mark bomb
                    tileMng.isABomb = tileMng.initValue == BOMB;
                    //set the position
                    float posX = col * tileSize;
                    float posY = row * -tileSize;
                    //transform the tiles to its pos
                    tile.transform.position = new Vector2(posX, posY);
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
        print(total); 
    }
    int calculateBomb(int posi, int posj){
        int total = 0;
        // atas
        if(posi > 0){
            for(int j = posj-1;j<=posj+1;j++){
                if(j >= 0 && j < cols-1){
                    if(gameGrid[posi-1,j] == BOMB)
                        total++;
                } 
            }
        }
        //bawah
        if(posi < rows -2){
            for(int j = posj-1;j<=posj+1;j++){
                if(j >= 0 && j < cols-2){
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
        if(posj < cols - 2){
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
                        totalFill--;
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
}