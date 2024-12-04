using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Event : MonoBehaviour{

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){

    }

    // 충돌이 감지된게 "탄환" 속성일 경우, 삭제되며 강화 포인트를 얻습니다
    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Bullet"){
            GameManager.Upgrade++;
            Destroy(gameObject);
        }
    }
}