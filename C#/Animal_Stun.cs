using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal_Stun : MonoBehaviour{
    // ��� ������ ������ ���� �ڵ��Դϴ�

    [SerializeField]
    private int Hp = 1;

    [SerializeField]
    public float Speed = 0.5f;

    // Start is called before the first frame update
    // Animal.cs ���� ������ ������ ��ü�ϴ� ������ Hp == 1 �̹Ƿ� 1�� �����մϴ�
    void Start(){
        Hp = 1;
        Speed = 0.5f;
    }

    // Update is called once per frame
    void Update(){
        
    }

    // �浹�� ������ ��, �ش� �Ӽ��� "źȯ" �� ���, źȯ�� ���ݷ¿� ���� ü���� �����մϴ�
    // ���� ���ҵ� ü���� 0 ���϶�� óġ�ǰ�, ������ ����ϴ�
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
