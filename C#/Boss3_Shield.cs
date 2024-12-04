using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3_Shield : MonoBehaviour{
    // ����° ������ �ɷ� �ڵ��Դϴ�

    Rigidbody2D rb;

    private float Speed = 2.5f;

    // Start is called before the first frame update
    // ���д� �� �������� ���ư��ϴ�
    // ȭ�鿡�� ��������� �ð�(2.5��)�� ���� ���, �����մϴ�
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

    // "źȯ" �Ӽ��� �浹�� ������ ���, ���д� ������ϴ�
    // (źȯ �ڵ� ����) ���п� �浹�� źȯ�� ���ŵ˴ϴ�
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Bullet"){
            Destroy(gameObject);
        }
    }
}