using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4_Shield : MonoBehaviour{
    // 네번째 보스의 능력 코드입니다

    Rigidbody2D rb;

    private int Speed = 2;
    [SerializeField]
    private int Hp = 5;

    // Start is called before the first frame update
    // 방패는 오른쪽으로 휘면서 날아가며, 휘는 힘이 점차 증가합니다
    // 화면에서 벗어날정도의 시간(1.25초)이 지날 경우, 삭제합니다
    void Start(){
        Speed = 2;
        Hp = 5;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * Speed;
        StartCoroutine(SideMove());

        Destroy(gameObject, 1.25f);
    }

    // 우측 방향으로도 힘이 계속 추가되어 가속합니다
    IEnumerator SideMove(){
        while (true){
            rb.AddForce(transform.right * Speed * 5);
            yield return new WaitForSeconds(0.025f);
        }
    }

    // Update is called once per frame
    void Update(){
        
    }

    // "탄환" 속성에 충돌이 감지될 경우 방패는 체력이 감소되며, 체력이 0 이하일 경우 삭제됩니다
    // (탄환 코드 참고) 방패에 충돌된 탄환은 제거됩니다
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Bullet"){
            Hp -= Bullet.Damage;
            if( Hp <= 0){
                Destroy(gameObject);
            }
        }
    }
}