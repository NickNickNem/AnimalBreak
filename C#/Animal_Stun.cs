using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal_Stun : MonoBehaviour{
    // 빈사 상태의 동물에 대한 코드입니다

    [SerializeField]
    private int Hp = 1;

    [SerializeField]
    public float Speed = 0.5f;

    // Start is called before the first frame update
    // Animal.cs 에서 기절된 동물로 교체하는 조건이 Hp == 1 이므로 1로 설정합니다
    void Start(){
        Hp = 1;
        Speed = 0.5f;
    }

    // Update is called once per frame
    void Update(){
        
    }

    // 충돌이 감지될 때, 해당 속성이 "탄환" 인 경우, 탄환의 공격력에 따라서 체력이 감소합니다
    // 만약 감소된 체력이 0 이하라면 처치되고, 점수를 얻습니다
    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Bullet"){
            Hp -= Bullet.Damage;

            if (Hp <= 0){
                GameManager.ScoreBouns++;
                //Debug.Log(GameManager.ScoreBouns);
                GameManager.instance.BreakScore();
                Destroy(gameObject);
            }
        }
    }

    // 만약 동물들끼리 겹쳐있게 된다면, 겹쳐있는 동안 y축이 낮은 동물을 아래 방향으로 밀어냅니다
    private void OnTriggerStay2D(Collider2D collision){
        if (collision.gameObject.tag == "Animals" || collision.gameObject.tag == "Animal_Stun"){
            Vector3 A = transform.position;
            Vector3 B = collision.transform.position;
            if (A.y > B.y){
                collision.transform.Translate(Vector3.down * 0.05f);
            }
        }
    }
}
