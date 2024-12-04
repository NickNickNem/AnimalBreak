using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4_Shield : MonoBehaviour{
    // �׹�° ������ �ɷ� �ڵ��Դϴ�

    Rigidbody2D rb;

    private int Speed = 2;
    [SerializeField]
    private int Hp = 5;

    // Start is called before the first frame update
    // ���д� ���������� �ָ鼭 ���ư���, �ִ� ���� ���� �����մϴ�
    // ȭ�鿡�� ��������� �ð�(1.25��)�� ���� ���, �����մϴ�
    void Start(){
        Speed = 2;
        Hp = 5;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * Speed;
        StartCoroutine(SideMove());

        Destroy(gameObject, 1.25f);
    }

    // ���� �������ε� ���� ��� �߰��Ǿ� �����մϴ�
    IEnumerator SideMove(){
        while (true){
            rb.AddForce(transform.right * Speed * 5);
            yield return new WaitForSeconds(0.025f);
        }
    }

    // Update is called once per frame
    void Update(){
        
    }

    // "źȯ" �Ӽ��� �浹�� ������ ��� ���д� ü���� ���ҵǸ�, ü���� 0 ������ ��� �����˴ϴ�
    // (źȯ �ڵ� ����) ���п� �浹�� źȯ�� ���ŵ˴ϴ�
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Bullet"){
            Hp -= Bullet.Damage;
            if( Hp <= 0){
                Destroy(gameObject);
            }
        }
    }
}