using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Animal : MonoBehaviour{
    // ���������� ���� �����鿡 ���� �ڵ��Դϴ�

    [SerializeField]
    public float Speed = 1;

    private int MaxHp = 2;
    private int Hp = 0;

    [SerializeField]
    private GameObject StunPrefab;

    // Start is called before the first frame update
    // �������� ������ �� �ʱ�ȭ �۾��� �ϴ� �ڵ��Դϴ�
    // �� ��, ������ �̸��� ���� ü��, �̵� �ӵ��� �޶��� �� �ֽ��ϴ�
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

    // ���� ��ġ�� ������ �����ϰ� 0.05�� �� �ش� ��ġ�� ���� �̹����� �����ϸ鼭 ���� �̹����� �����մϴ�
    IEnumerator Change(){
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        yield return new WaitForSeconds(0.05f);
        Instantiate(StunPrefab, position, rotation);
        Destroy(gameObject);
    }

    // �浹�� ������ ��, �ش� �Ӽ��� "źȯ" �� ���, źȯ�� ���ݷ¿� ���� ü���� �����մϴ�
    // ���� ���ҵ� ü���� 0 ���϶�� óġ�ǰ�, ������ ����ϴ�
    // 1�� �� ��쿡�� ������ ���� �̹����� ��ü�۾��� �����մϴ� (StartCoroutine(Change));)
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

    // ���� �����鳢�� �����ְ� �ȴٸ�, �����ִ� ���� y���� ���� ������ �Ʒ� �������� �о���ϴ�
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