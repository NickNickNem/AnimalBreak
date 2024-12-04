using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour{
    // 게임 패배 조건 코드입니다 (빨간 선에 들어가는 코드)
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    // 충돌이 감지된게 {동물, 동물_기절됨} 속성일 경우 게임 패배 화면으로 전환합니다
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Animals" ||  collision.tag == "Animal_Stun"){
            SceneManager.LoadScene("GameOver");
        }
    }
}