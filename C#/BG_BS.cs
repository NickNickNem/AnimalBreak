using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_BS : MonoBehaviour{
    // 보스의 움직이는 뒷배경 코드입니다

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start(){
        
    }

    // 왼쪽 방향으로 배경이 움직입니다
    private void OnEnable(){
        StartCoroutine(Move());
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * -1 * 3f;
    }

    // 2초마다 고정된 위치로 다시 돌아갑니다
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