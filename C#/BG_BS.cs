using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_BS : MonoBehaviour{
    // ������ �����̴� �޹�� �ڵ��Դϴ�

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start(){
        
    }

    // ���� �������� ����� �����Դϴ�
    private void OnEnable(){
        StartCoroutine(Move());
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * -1 * 3f;
    }

    // 2�ʸ��� ������ ��ġ�� �ٽ� ���ư��ϴ�
    IEnumerator Move(){
        while (true){
            gameObject.transform.position = new Vector3(3, 1.25f, 0);
            yield return new WaitForSeconds(2f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}