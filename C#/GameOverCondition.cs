using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour{
    // ���� �й� ���� �ڵ��Դϴ� (���� ���� ���� �ڵ�)
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    // �浹�� �����Ȱ� {����, ����_������} �Ӽ��� ��� ���� �й� ȭ������ ��ȯ�մϴ�
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Animals" ||  collision.tag == "Animal_Stun"){
            SceneManager.LoadScene("GameOver");
        }
    }
}