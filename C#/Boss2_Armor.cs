using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_Armor : MonoBehaviour{
    // �ι�° ������ �� �ڵ��Դϴ�

    public static bool hit = false;
    public static GameObject a;

    // Start is called before the first frame update
    void Start(){
        a = this.gameObject;
    }

    // Update is called once per frame
    void Update(){

    }

    // "źȯ" �Ӽ��� �浹�� ������ ���, ���� ��Ȱ��ȭ �˴ϴ�
    // (źȯ �ڵ� ����) ���� �浹�� źȯ�� ���ŵ˴ϴ�
    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Bullet"){
            Debug.Log("Collied");
            hit = true;
            gameObject.SetActive(false);
        }
    }

    private void OnEnable(){
        hit = false;
    }
}
