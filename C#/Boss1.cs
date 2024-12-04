using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Boss1 : MonoBehaviour{
    // ù��° �����Դϴ� (û�� �̹���)

    Rigidbody2D rb;

    [SerializeField]
    private float Speed = 3.75f;
    [SerializeField]
    private GameObject UI_Hp;
    [SerializeField]
    private TextMeshProUGUI Text_Hp;
    [SerializeField]
    private int MaxHp = 75;
    [SerializeField]
    public int Hp = 75;
    private GameObject[] Tag_Animal;
    private GameObject[] Tag_Animal_Stun;

    private int BS = 100;
    [SerializeField]
    private TextMeshProUGUI Text_BS;

    // Start is called before the first frame update
    // ������ ������ �� ��� "����" "����_������" �Ӽ����� ã��, ��Ȱ��ȭ ��ŵ�ϴ�(Blink() �Լ����� �ļ�)
    void Start(){
        Speed = 3.75f;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = (transform.right * -1) * Speed;
        MaxHp = 75;
        Hp = MaxHp;
        Text_Hp.SetText("Hp : " + Hp.ToString());
        BS = 100;
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
            transform.Translate(0.3f,0,0);
            UI_Hp.transform.Rotate(0, 180, 0);
            Text_Hp.transform.Rotate(0, 180, 0);
            Text_BS.transform.Rotate(0, 180, 0);
        }

        if(collision.tag == "Bullet"){
            Hp -= Bullet.Damage;
            UI_Hp.GetComponent<Image>().fillAmount = (Hp * 1f / MaxHp);
            Text_Hp.SetText("Hp : " + Hp.ToString());
            if (Hp <= 0) {
                Aim_Fire.FireRate = 0.4f;
                GameManager.Add_Bullet++;
                GameManager.instance.BulletCounter();
                Destroy(gameObject);
            }
        }
    }

    // �� �� �߰� ���� ��ġ�� ���ҵǸ�, BGM�� ����� �� ���� óġ���� ���� ��� �����˴ϴ�
    IEnumerator Tick(){
        while (true){
            if(BS > 0){
                BS--;
                Text_BS.SetText("Bonus : " + BS.ToString());
                //Debug.Log("Kill Bonus : " + BS);
            }
            if (BGM.BGM_Count == 4){
                BS = 0;
                Destroy(gameObject);
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
        if (BGM.BGM_Count == 3){
            BGM.BGM_Skip = true;
        }
        AnimalsSpawn.Bosses = false;
        BGM.BGMTimer = 203;
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