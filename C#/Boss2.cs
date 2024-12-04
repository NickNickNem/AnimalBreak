using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss2 : MonoBehaviour{
    // 두번째 보스입니다 (현무 이미지)

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
    // 보스가 생성될 때 모든 "동물" "동물_기절됨" 속성들을 찾고, 비활성화 시킵니다(Blink() 함수에서 후술)
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

    // 충돌이 감지된게 양 끝 벽일 경우, 보스의 이동 방향 및 UI를 좌우반전 시킵니다
    // 충돌이 감지된게 "탄환" 속성일 경우, 피해를 받습니다
    // 피해를 받아서 체력이 0 이하일 경우, 처치 보상으로 이용자의 대포를 강화시킵니다
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

    // 매 초 추가 점수 수치가 감소되며, BGM이 종료될 때 까지 처치하지 못할 경우 생략됩니다
    // 방어막의 충돌이 감지 되었다면 Refill 수치를 1씩 증가합니다
    // Refill 수치가 5 이상(5초 이상) 될 경우, 방어막을 재충전합니다
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

    // 보스가 사라질 때 추가 점수(변수 BS)가 1 이상일 경우 추가 점수를 얻습니다
    // 보스가 사라질 때, 아직 보스 테마곡이 진행중일 경우 생략합니다
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