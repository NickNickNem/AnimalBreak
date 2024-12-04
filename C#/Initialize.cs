using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Initialize : MonoBehaviour{
    // 게임 승리, 패배 화면에 들어가는 점수 UI 코드입니다

    [SerializeField]
    private TextMeshProUGUI ScoreUI;
    private int Score = 0;

    public void InitScene(){
        SceneManager.LoadScene("First");
        //Debug.Log("Start!");
    }



    // Start is called before the first frame update
    void Start(){
        Score = GameManager.UI_BreakCount;
        ScoreUI.SetText("Score : " + Score.ToString());
    }

    // Update is called once per frame
    void Update(){
        
    }
}