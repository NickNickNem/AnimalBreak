using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3_Shield : MonoBehaviour{
    // 세번째 보스의 능력 코드입니다

    Rigidbody2D rb;

    private float Speed = 2.5f;

    // Start is called before the first frame update
    // 방패는 한 방향으로 날아갑니다
    // 화면에서 벗어날정도의 시간(2.5초)이 지날 경우, 삭제합니다
    void Start(){
        Speed = 2.5f;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * Speed;

        Destroy(gameObject, 2.5f);
    }

    // Update is called once per frame
    void Update(){
        transform.Rotate(0, 0, 5);
    }

    // "탄환" 속성에 충돌이 감지될 경우, 방패는 사라집니다
    // (탄환 코드 참고) 방패에 충돌된 탄환은 제거됩니다
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Bullet"){
            Destroy(gameObject);
        }
    }
}