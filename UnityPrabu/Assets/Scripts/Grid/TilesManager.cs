using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TilesManager : MonoBehaviour
{
    //for readable code
    private const int ADD = 1;
    private const int SUB = 0;
    
    public int count = 0;                               //tiles score
    private int saveCount = 0;                          //save mode
    public int initValue = 0;
    public bool isABomb = false;                        //is a bomb, so uh, count could change
    public bool isFixed = false;                        //the show count can not change
    public bool scoreChanged = false;

    public Text score;                                  //to show score
    // Start is called before the first frame update
    void Start()
    {
        if(isFixed){
            count = initValue;
        }
        showScore();
    }

    // Update is called once per frame
    void Update()
    {
        scoreChanged = false;
        if(!isFixed){
            StateUpdate();
            ScoreUpdate();
        }
        if(scoreChanged)
            showScore();
    }

    void StateUpdate(){
        if(Input.GetKey("s")){                          //check if it wants to save the current state
            saveCount = count;
        }else if(Input.GetKey("r")){                    //check if it wants to restart
            count = 0;
            scoreChanged = true;
        }else if(Input.GetKey("b")){                    //go back to prev saved state
            count = saveCount;
            scoreChanged = true;
        }
    }

    //to update the score according to the user input
    void ScoreUpdate(){
        //jika klik kiri
        if(Input.GetMouseButtonDown(0)){
            if(isClicked()){
                countUpdate(ADD);
                scoreChanged = true;
                FindObjectOfType<AudioManager>().Play("ClickFX");
            }
        }
        //jika klik kanan
        else if(Input.GetMouseButtonDown(1)){
            if(isClicked()){
                scoreChanged = true;
                //jika di klik dengan shift key, kita reset isinya
                if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
                    count = 0;
                }
                //klo ga teken shift
                else if(count > 0){
                    count--;
                }else{
                    //jadi bintang klo count == 0
                    count = 9;
                }
                FindObjectOfType<AudioManager>().Play("ClickFX");
            }
        }
    }

    void showScore(){
        if(count > 8){
            count = 9;
            score.text = "*";
            return;
        }
        if(count <= 0){
            count = 0;
            score.text = "";
            return;
        }
        score.text = count.ToString();        
    }

    void countUpdate(int mode){
        if(mode == 0)
            count--;
        if(mode == 1)
            count++;
    }

    bool isClicked(){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 100.0f))
        {
            if(hit.transform != null){  
                return hit.transform.gameObject.name == this.name;
            }
        }
        return false;    
    }
}
