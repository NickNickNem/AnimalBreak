using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Animal : MonoBehaviour{
    // 기절당하지 않은 동물들에 대한 코드입니다

    [SerializeField]
    public float Speed = 1;

    private int MaxHp = 2;
    private int Hp = 0;

    [SerializeField]
    private GameObject StunPrefab;

    // Start is called before the first frame update
    // 동물들이 생성될 때 초기화 작업을 하는 코드입니다
    // 이 때, 동물의 이름에 따라서 체력, 이동 속도가 달라질 수 있습니다
    void Start(){
        MaxHp = 2;
        GameObject Type = GameObject.Find(gameObject.name);
        //Debug.Log(Type);
        if (Type.name == "Rat(Clone)" || Type.name == "Rabbit(Clone)" || Type.name == "Horse(Clone)"){
            Speed += 0.5f;
        }else if (Type.name == "Ox(Clone)" || Type.name == "Pig(Clone)"){
            MaxHp += 2;
            Speed -= 0.2f;
        }else if (Type.name == "Dragon(Clone)"){
            MaxHp += 1;
            Speed += 0.25f;
        }
        MaxHp = MaxHp * (BossSpawn.BC + 1) - BossSpawn.Advantage;
        Hp = MaxHp;
        Speed = Speed + (Speed * BossSpawn.BC * 1f / 6);
        //Debug.Log(Speed);
        //Debug.Log(Hp);
    }

    // Update is called once per frame
    void Update(){

    }

    // 현재 위치와 각도를 저장하고 0.05초 후 해당 위치에 기절 이미지를 생성하면서 기존 이미지를 삭제합니다
    IEnumerator Change(){
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        yield return new WaitForSeconds(0.05f);
        Instantiate(StunPrefab, position, rotation);
        Destroy(gameObject);
    }

    // 충돌이 감지될 때, 해당 속성이 "탄환" 인 경우, 탄환의 공격력에 따라서 체력이 감소합니다
    // 만약 감소된 체력이 0 이하라면 처치되고, 점수를 얻습니다
    // 1이 될 경우에는 기절한 동물 이미지로 교체작업을 진행합니다 (StartCoroutine(Change));)
    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Bullet"){
            Hp -= Bullet.Damage;

            if( Hp <= 0) {
                GameManager.ScoreBouns++;
                //Debug.Log(GameManager.ScoreBouns);
                GameManager.instance.BreakScore();
                Destroy(gameObject);
            }else if(Hp == 1){
                StartCoroutine(Change());
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