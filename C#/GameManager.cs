using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{
    // 전반적인 게임 흐름에 관련된 코드입니다

    public static GameManager instance = null;

    [SerializeField]
    private TextMeshProUGUI BreakUI;

    [SerializeField]
    private TextMeshProUGUI BulletUI;

    private int Phase = 0;

    public static int UI_BreakCount = 0;
    public static int UI_BulletCount = 1;

    public static int Add_Bullet = 0;

    public static int BulletCount = 0;

    public static int ScoreBouns = -2;
    [SerializeField]
    private GameObject StreakUI;
    [SerializeField]
    private TextMeshProUGUI KS;
    [SerializeField]
    private TextMeshProUGUI BS;

    [SerializeField]
    private GameObject UpgradeUI;
    [SerializeField]
    private TextMeshProUGUI UPUI;
    [SerializeField]
    private GameObject PowerUI;
    public static int Upgrade = 0;

    [SerializeField]
    private AudioSource Play_SFX;
    [SerializeField]
    private AudioClip[] SFX;

    void Awake(){
        if (instance == null){
            instance = this;
        }
        UI_BreakCount = 0;
        UI_BulletCount = 1;
        BulletCount = 0;
        ScoreBouns = -2;
        Upgrade = 0;
    }

    private void Start(){
        StreakUI.SetActive(false);
        UpgradeUI.SetActive(false);
        Play_SFX = GetComponent<AudioSource>();
        StartCoroutine(Optimization());
    }

    IEnumerator Optimization(){
        // 0.05초마다 무한 반복합니다 (yield return new WaitForSeconds(0.05f);)
        while (true){
            Phase = Aim_Fire.Ready;

            /*
            탄환이 모두 제거되었을 경우, 추가 점수가 있는지를 확인합니다
            그 후 모든 {동물, 동물_기절됨, 이벤트} 속성들의 Speed 변수에 따라 내려갑니다
            1초 후 {동물, 동물_기절됨, 이벤트} 속성들의 이동 속도를 0으로 만들어서 정지시킵니다
            */
            if (Phase == -2){
                if (ScoreBouns > 0){
                    int bonus = 0;
                    Play_SFX.clip = SFX[0];
                    Play_SFX.Play();
                    for (int i = 0; i <= ScoreBouns; i++){
                        bonus += i;
                    }
                    StreakUI.SetActive(true);
                    KS.SetText((ScoreBouns + 2).ToString() + " Kill\nCombo!");
                    BS.SetText("Get " + bonus + " Bonus Score!");
                    for (int i = 0; i < bonus; i++){
                        instance.BreakScore();
                    }
                }
                ScoreBouns = -2;

                GameObject[] Tag_Animal = GameObject.FindGameObjectsWithTag("Animals");
                GameObject[] Tag_Animal_Stun = GameObject.FindGameObjectsWithTag("Animal_Stun");
                GameObject[] Tag_Events = GameObject.FindGameObjectsWithTag("Events");

                foreach (GameObject AnimalsPos in Tag_Animal){
                    Animal animalScript = AnimalsPos.GetComponent<Animal>();
                    Rigidbody2D rb = AnimalsPos.GetComponent<Rigidbody2D>();
                    rb.velocity = new Vector2(0, -animalScript.Speed);
                    //AnimalsPos.transform.Translate(Vector3.down * animalScript.Speed);
                    //Debug.Log(Tag_Animal);
                }
                foreach (GameObject AnimalsPos in Tag_Animal_Stun){
                    Animal_Stun animalScript = AnimalsPos.GetComponent<Animal_Stun>();
                    Rigidbody2D rb = AnimalsPos.GetComponent<Rigidbody2D>();
                    rb.velocity = new Vector2(0, -animalScript.Speed);
                }

                yield return new WaitForSeconds(1f);

                foreach (GameObject AnimalsPos in Tag_Animal){
                    Rigidbody2D rb = AnimalsPos.GetComponent<Rigidbody2D>();
                    rb.velocity = Vector2.zero;
                }
                foreach (GameObject AnimalsPos in Tag_Animal_Stun){
                    Rigidbody2D rb = AnimalsPos.GetComponent<Rigidbody2D>();
                    rb.velocity = Vector2.zero;
                }
                foreach (GameObject EventsPos in Tag_Events){
                    EventsPos.transform.Translate(Vector3.down * 1);
                    if (EventsPos.transform.position.y < -4){
                        Destroy(gameObject);
                    }
                }
                // 동물들이 다 움직이고 1초 후, 추가 점수 UI를 숨기고, 동물 생성 차례(Aim_Fire.Ready++)로 넘깁니다
                // 효과음 배열에서 장전 사운드(SFX[1])을 실행시킵니다
                yield return new WaitForSeconds(1f);
                StreakUI.SetActive(false);
                Aim_Fire.Ready++;
                Play_SFX.clip = SFX[1];
                Play_SFX.Play();
            }

            // 동물들이 생성된 후
            if(Phase == 0){
                // 강화 점수가 있을 경우 강화 선택지 UI를 활성화 합니다
                if (Upgrade > 0){
                    UpgradeUI.SetActive(true);
                    UPUI.SetText("Select Upgrade Perk\nUpgrade Point : " + Upgrade);
                    if(Aim_Fire.Bullets >= 8 + BossSpawn.DealBuff*2){
                        PowerUI.SetActive(true);
                    }else{
                        PowerUI.SetActive(false);
                    }
                }else{
                    UpgradeUI.SetActive(false);
                }
            }
            
            // 탄환 발사된 후
            if (Phase == 1){
                // 사라진 탄환 수(Bullet.cs 에서 증가될 변수) == 발사 가능한 탄환 수 일경우 동물들이 움직일 차례(Aim_Fire.Ready)를 수정합니다
                if (BulletCount == Aim_Fire.Bullets){
                    BulletCount = 0;
                    if (Add_Bullet >= 1){
                        Aim_Fire.Bullets = Aim_Fire.Bullets + Add_Bullet;
                        Add_Bullet = 0;
                    }
                    Aim_Fire.Ready -= 3;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void Update(){
        
    }

    public void BreakScore(){
        UI_BreakCount++;
        BreakUI.SetText("Score\n" + UI_BreakCount.ToString());
    }

    public void BulletCounter(){
        UI_BulletCount++;
        BulletUI.SetText("Bullet : " + UI_BulletCount.ToString() + "\nDeal : " + (1+BossSpawn.DealBuff));
    }

    // 강화 UI에서 추가 점수를 받는 버튼을 누를 때 작동하는 함수입니다
    public void GBS() {
        Upgrade--;
        for (int i = 0; i < 50; i++){
            instance.BreakScore();
        }
    }

    // 강화 UI에서 탄환 수량 늘리는 버튼을 누를 때 작동하는 함수입니다
    public void Quantity() {
        Upgrade--;
        float Add = 0;
        Add = Aim_Fire.Bullets * 0.125f; // 현재 탄환에서 12.5% 값을 구합니다
        Add = Mathf.CeilToInt(Add); // 그 후 소숫점 올림 값으로 재정의 합니다
        //Debug.Log(Add);
        // Add 수치 만큼 탄환을 증가시킵니다
        for (int i = 0; i < Add; i++){
            Aim_Fire.Bullets++;
            instance.BulletCounter();
        }
    }

    public void PowerUp(){
        Upgrade--;
        float multiple = 0;
        int subtract = 0;
        multiple = Aim_Fire.Bullets * 0.375f; // 현재 탄환에서 37.5% 값을 구합니다
        subtract = Mathf.CeilToInt(multiple); // 그 후 소숫점 올림 값으로 재정의 합니다
        Aim_Fire.Bullets -= subtract; // 현재 탄환에서 subtract 수치만큼 감소합니다
        UI_BulletCount -= subtract;
        BossSpawn.DealBuff++; // 피해량을 증가시킵니다
        BulletUI.SetText("Bullet : " + UI_BulletCount.ToString() + "\nDeal : " + (1+BossSpawn.DealBuff));
    }
}