using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss4 : MonoBehaviour{
    // ����° �����Դϴ� (���� �̹���)

    Rigidbody2D rb;

    [SerializeField]
    private float Speed = 2.5f;
    [SerializeField]
    private int MaxHp = 250;
    [SerializeField]
    public int Hp = 250;
    [SerializeField]
    private GameObject UI_Hp;
    [SerializeField]
    private TextMeshProUGUI Text_Hp;
    GameObject[] Tag_Animal;
    GameObject[] Tag_Animal_Stun;

    private int BS = 500;
    [SerializeField]
    private TextMeshProUGUI Text_BS;
    [SerializeField]
    private GameObject Shield;
    [SerializeField]
    private Transform Spawner;
    private float RS = 0.5f;

    // Start is called before the first frame update
    // ������ ������ �� ��� "����" "����_������" �Ӽ����� ã��, ��Ȱ��ȭ ��ŵ�ϴ�(Blink() �Լ����� �ļ�)
    void Start(){
        Speed = 2.5f;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = (transform.right * -1) * Speed;
        MaxHp = 250;
        Hp = MaxHp;
        Text_Hp.SetText("Hp : " + Hp.ToString());
        BS = 500;
        Text_BS.SetText("Bonus : " + BS.ToString());
        Tag_Animal = GameObject.FindGameObjectsWithTag("Animals");
        Tag_Animal_Stun = GameObject.FindGameObjectsWithTag("Animal_Stun");
        RS = 0.5f;
        Blink();
        StartCoroutine(Tick());
    }

    // Update is called once per frame
    void Update(){
        Spawner.Rotate(0, 0, RS);
    }

    // �浹�� �����Ȱ� �� �� ���� ���, ������ �̵� ���� �� UI�� �¿���� ��ŵ�ϴ�
    // �浹�� �����Ȱ� "źȯ" �Ӽ��� ���, ���ظ� �޽��ϴ�
    // ���ظ� �޾Ƽ� ü���� 0 ������ ���, ���� �¸� ȭ������ ��ȯ�մϴ�
    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.tag == "Side"){
            Vector3 Recochet = Vector3.Reflect(rb.velocity, Vector3.right);
            rb.velocity = Recochet;
            transform.Rotate(0, 180, 0);
            transform.Translate(1.25f, 0, 0);
            UI_Hp.transform.Rotate(0, 180, 0);
            Text_Hp.transform.Rotate(0, 180, 0);
            Text_BS.transform.Rotate(0, 180, 0);
            RS = RS * -1;
        }

        if (collision.tag == "Bullet"){
            Hp -= Bullet.Damage;
            UI_Hp.GetComponent<Image>().fillAmount = (Hp * 1f / MaxHp);
            Text_Hp.SetText("Hp : " + Hp.ToString());
            if (Hp <= MaxHp/2){
                Speed = 5;
                rb.velocity = (transform.right * -1) * Speed;
            }
            if (Hp <= 0){
                Destroy(gameObject);
                SceneManager.LoadScene("Clear");
            }
        }
    }

    // �� �� �߰� ���� ��ġ�� ���ҵǸ�, BGM�� ����� �� ���� óġ���� ���� ��� �����˴ϴ�
    // �� �� ���и� �߻��Ͽ� źȯ�� �������� �õ��ϸ�, ü���� ���� ������ ��� ���� ������ �����մϴ�
    IEnumerator Tick(){
        while (true){
            if (BS > 0){
                BS -= 2;
                Text_BS.SetText("Bonus : " + BS.ToString());
                //Debug.Log("Kill Bonus : " + BS);
            }
            if (BGM.BGM_Count == 1){
                BS = 0;
                Destroy(gameObject);
                SceneManager.LoadScene("GameOver");
            }
            if (Hp >= MaxHp / 2) {
                for (int i = 0; i < 3; i++){
                    Instantiate(Shield, Spawner.position, Spawner.rotation * Quaternion.Euler(0, 0, i * 120));
                }
            }else{
                for (int i = 0; i < 5; i++){
                    Instantiate(Shield, Spawner.position, Spawner.rotation * Quaternion.Euler(0, 0, i * 72));
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
        if (BGM.BGM_Count == 0){
            BGM.BGM_Skip = true;
        }
        AnimalsSpawn.Bosses = false;
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