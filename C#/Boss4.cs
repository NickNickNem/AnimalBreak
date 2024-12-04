using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss4 : MonoBehaviour{
    // 세번째 보스입니다 (주작 이미지)

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
    // 보스가 생성될 때 모든 "동물" "동물_기절됨" 속성들을 찾고, 비활성화 시킵니다(Blink() 함수에서 후술)
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

    // 충돌이 감지된게 양 끝 벽일 경우, 보스의 이동 방향 및 UI를 좌우반전 시킵니다
    // 충돌이 감지된게 "탄환" 속성일 경우, 피해를 받습니다
    // 피해를 받아서 체력이 0 이하일 경우, 게임 승리 화면으로 전환합니다
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

    // 매 초 추가 점수 수치가 감소되며, BGM이 종료될 때 까지 처치하지 못할 경우 생략됩니다
    // 매 초 방패를 발사하여 탄환을 막으려고 시도하며, 체력이 절반 이하일 경우 방패 개수가 증가합니다
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

    // 보스가 사라질 때 추가 점수(변수 BS)가 1 이상일 경우 추가 점수를 얻습니다
    // 보스가 사라질 때, 아직 보스 테마곡이 진행중일 경우 생략합니다
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

    // "동물" 속성과 "동물_기절됨" 속성의 수량만큼 반복하면서 활성화/비활성화 상태를 반전시킵니다
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