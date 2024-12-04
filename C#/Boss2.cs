using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss2 : MonoBehaviour{
    // �ι�° �����Դϴ� (���� �̹���)

    Rigidbody2D rb;

    [SerializeField]
    private float Speed = 2.5f;
    [SerializeField]
    private int MaxHp = 125;
    [SerializeField]
    public int Hp = 125;
    [SerializeField]
    private GameObject UI_Hp;
    [SerializeField]
    private TextMeshProUGUI Text_Hp;
    GameObject[] Tag_Animal;
    GameObject[] Tag_Animal_Stun;

    private int BS = 200;
    [SerializeField]
    private TextMeshProUGUI Text_BS;
    private int Refill = 0;

    // Start is called before the first frame update
    // ������ ������ �� ��� "����" "����_������" �Ӽ����� ã��, ��Ȱ��ȭ ��ŵ�ϴ�(Blink() �Լ����� �ļ�)
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = (transform.right * -1) * Speed;
        MaxHp = 125;
        Hp = MaxHp;
        Text_Hp.SetText("Hp : " + Hp.ToString());
        BS = 200;
        Text_BS.SetText("Bonus : " + BS.ToString());
        Refill = 0;
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
        if (collision.tag == "Side"){
            Vector3 Recochet = Vector3.Reflect(rb.velocity, Vector3.right);
            rb.velocity = Recochet;
            transform.Rotate(0, 180, 0);
            UI_Hp.transform.Rotate(0, 180, 0);
            Text_Hp.transform.Rotate(0, 180, 0);
            Text_BS.transform.Rotate(0, 180, 0);
        }

        if (collision.tag == "Bullet"){
            Hp -= Bullet.Damage;
            UI_Hp.GetComponent<Image>().fillAmount = (Hp * 1f / MaxHp);
            Text_Hp.SetText("Hp : " + Hp.ToString());
            if (Hp <= 0){
                Aim_Fire.FireRate = 0.25f;
                BossSpawn.Advantage += 2;
                Destroy(gameObject);
            }
        }
    }

    // �� �� �߰� ���� ��ġ�� ���ҵǸ�, BGM�� ����� �� ���� óġ���� ���� ��� �����˴ϴ�
    // ���� �浹�� ���� �Ǿ��ٸ� Refill ��ġ�� 1�� �����մϴ�
    // Refill ��ġ�� 5 �̻�(5�� �̻�) �� ���, ���� �������մϴ�
    IEnumerator Tick(){
        while (true){
            if (BS > 0){
                BS--;
                Text_BS.SetText("Bonus : " + BS.ToString());
                //Debug.Log("Kill Bonus : " + BS);
            }
            if (BGM.BGM_Count == 7){
                BS = 0;
                Destroy(gameObject);
            }
            if(Boss2_Armor.hit == true){
                Refill++;
                //Debug.Log(Refill);
                if(Refill >= 5){
                    Refill = 0;
                    Boss2_Armor.a.SetActive(true);
                    BS--;
                    Text_BS.SetText("Bonus : " + BS.ToString());
                }
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
        if (BGM.BGM_Count == 6){
            BGM.BGM_Skip = true;
        }
        AnimalsSpawn.Bosses = false;
        BGM.BGMTimer = 179;
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