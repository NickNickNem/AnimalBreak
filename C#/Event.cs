using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Event : MonoBehaviour{

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){

    }

    // �浹�� �����Ȱ� "źȯ" �Ӽ��� ���, �����Ǹ� ��ȭ ����Ʈ�� ����ϴ�
    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Bullet"){
            GameManager.Upgrade++;
            Destroy(gameObject);
        }
    }
}