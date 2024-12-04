using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class Boss3 : MonoBehaviour{
    // ����° �����Դϴ� (��ȣ �̹���)
    
    Rigidbody2D rb;

    [SerializeField]
    private float Speed = 2.5f;
    [SerializeField]
    private int MaxHp = 200;
    [SerializeField]
    public int Hp = 200;
    [SerializeField]
    private GameObject UI_Hp;
    [SerializeField]
    private TextMeshProUGUI Text_Hp;
    GameObject[] Tag_Animal;
    GameObject[] Tag_Animal_Stun;

    private int BS = 375;
    [SerializeField]
    private TextMeshProUGUI Text_BS;
    [SerializeField]
    private GameObject Shield;
    [SerializeField]
    private Transform Spawner;


    // Start is called before the first frame update
    // ������ ������ �� ��� "����" "����_������" �Ӽ����� ã��, ��Ȱ��ȭ ��ŵ�ϴ�(Blink() �Լ����� �ļ�)
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = (transform.right * -1) * Speed;
        MaxHp = 200;
        Hp = MaxHp;
        Text_Hp.SetText("Hp : " + Hp.ToString());
        BS = 375;
        Text_BS.SetText("Bonus : " + BS.ToString());
        Tag_Animal = GameObject.FindGameObjectsWithTag("Animals");
        Tag_Animal_Stun = GameObject.FindGameObjectsWithTag("Animal_Stun");
        Blink();
        StartCoroutine(Tick());
    }

    // Update is called once per frame
    void Update(){
        
    }

    // �浹�� �����Ȱ� �� �� ���� ���, ������ �̵� ���� �� UI�� �¿���� ��ŵ�ϴ�
    // �浹�� �����Ȱ� "źȯ" �Ӽ��� ���, ���ظ� �޽��ϴ�
    // ���ظ� �޾Ƽ� ü���� 0 ������ ���, óġ �������� �̿����� ������ ��ȭ��ŵ�ϴ�
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Side"){
            Vector3 Recochet = Vector3.Reflect(rb.velocity, Vector3.right);
            rb.velocity = Recochet;
            transform.Rotate(0, 180, 0);
            transform.Translate(2, 0, 0);
            UI_Hp.transform.Rotate(0, 180, 0);
            Text_Hp.transform.Rotate(0, 180, 0);
            Text_BS.transform.Rotate(0, 180, 0);
        }

        if(collision.tag == "Bullet"){
            Hp -= Bullet.Damage;
            UI_Hp.GetComponent<Image>().fillAmount = (Hp * 1f / MaxHp);
            Text_Hp.SetText("Hp : " + Hp.ToString());
            if ( Hp <= 0) {
                Aim_Fire.FireRate = 0.2f;
                GameManager.UI_BulletCount--;
                BossSpawn.DealBuff++;
                GameManager.instance.BulletCounter();
                Destroy(gameObject);
            }
        }
    }

    // �� �� �߰� ���� ��ġ�� ���ҵǸ�, BGM�� ����� �� ���� óġ���� ���� ��� �����˴ϴ�
    // �� �� Fire�� 1�� �����ϸ� 3�̻��� �� ���(3��) ���и� �߻��Ͽ� źȯ�� �������� �õ��մϴ�
    IEnumerator Tick(){
        int Fire = 0;
        while (true){
            if(BS > 0){
                BS--;
                Text_BS.SetText("Bonus : " + BS.ToString());
                //Debug.Log("Kill Bonus : " + BS);
            }
            if (BGM.BGM_Count == 10){
                BS = 0;
                Destroy(gameObject);
            }
            if (Fire >=3){
                //Instantiate(Bullet, BulletTransform.position, transform.rotation);
                for(int i = 0; i<4; i++) {
                    Instantiate(Shield, Spawner.position, Quaternion.Euler(0, 0, i*90));
                }
                Fire = 0;
                BS--;
                Text_BS.SetText("Bonus : " + BS.ToString());
            }
            else{
                Fire++;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    // ������ ����� �� �߰� ����(���� BS)�� 1 �̻��� ��� �߰� ������ ����ϴ�
    // ������ ����� ��, ���� ���� �׸����� �������� ��� �����մϴ�
    private void OnDestroy(){
        if (BS > 0){
            for (int i = 1; i <= BS; i++){
                GameManager.instance.BreakScore();
            }
        }
        if(BGM.BGM_Count == 9){
            BGM.BGM_Skip = true;
        }
        AnimalsSpawn.Bosses = false;
        BGM.BGMTimer = 193;
        Blink();
    }

    // "����" �Ӽ��� "����_������" �Ӽ��� ������ŭ �ݺ��ϸ鼭 Ȱ��ȭ/��Ȱ��ȭ ���¸� ������ŵ�ϴ�
    void Blink(){
        foreach (GameObject AnimalsPos in Tag_Animal){
            if (AnimalsPos.activeSelf == true){
                AnimalsPos.SetActive(false);
            }else{
                AnimalsPos.SetActive(true);
            }
        }

        foreach (GameObject AnimalsPos in Tag_Animal_Stun){
            if (AnimalsPos.activeSelf == true){
                AnimalsPos.SetActive(false);
            }else{
                AnimalsPos.SetActive(true);
            }
        }
    }
}