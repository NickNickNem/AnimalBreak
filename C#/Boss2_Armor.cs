using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_Armor : MonoBehaviour{
    // 두번째 보스의 방어막 코드입니다

    public static bool hit = false;
    public static GameObject a;

    // Start is called before the first frame update
    void Start(){
        a = this.gameObject;
    }

    // Update is called once per frame
    void Update(){

    }

    // "탄환" 속성에 충돌이 감지될 경우, 방어막이 비활성화 됩니다
    // (탄환 코드 참고) 방어막에 충돌된 탄환은 제거됩니다
    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Bullet"){
            Debug.Log("Collied");
            hit = true;
            gameObject.SetActive(false);
        }
    }

    private void OnEnable(){
        hit = false;
    }
}
