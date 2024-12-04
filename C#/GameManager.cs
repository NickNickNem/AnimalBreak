using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{
    // �������� ���� �帧�� ���õ� �ڵ��Դϴ�

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
        // 0.05�ʸ��� ���� �ݺ��մϴ� (yield return new WaitForSeconds(0.05f);)
        while (true){
            Phase = Aim_Fire.Ready;

            /*
            źȯ�� ��� ���ŵǾ��� ���, �߰� ������ �ִ����� Ȯ���մϴ�
            �� �� ��� {����, ����_������, �̺�Ʈ} �Ӽ����� Speed ������ ���� �������ϴ�
            1�� �� {����, ����_������, �̺�Ʈ} �Ӽ����� �̵� �ӵ��� 0���� ���� ������ŵ�ϴ�
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
                // �������� �� �����̰� 1�� ��, �߰� ���� UI�� �����, ���� ���� ����(Aim_Fire.Ready++)�� �ѱ�ϴ�
                // ȿ���� �迭���� ���� ����(SFX[1])�� �����ŵ�ϴ�
                yield return new WaitForSeconds(1f);
                StreakUI.SetActive(false);
                Aim_Fire.Ready++;
                Play_SFX.clip = SFX[1];
                Play_SFX.Play();
            }

            // �������� ������ ��
            if(Phase == 0){
                // ��ȭ ������ ���� ��� ��ȭ ������ UI�� Ȱ��ȭ �մϴ�
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
            
            // źȯ �߻�� ��
            if (Phase == 1){
                // ����� źȯ ��(Bullet.cs ���� ������ ����) == �߻� ������ źȯ �� �ϰ�� �������� ������ ����(Aim_Fire.Ready)�� �����մϴ�
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

    // ��ȭ UI���� �߰� ������ �޴� ��ư�� ���� �� �۵��ϴ� �Լ��Դϴ�
    public void GBS() {
        Upgrade--;
        for (int i = 0; i < 50; i++){
            instance.BreakScore();
        }
    }

    // ��ȭ UI���� źȯ ���� �ø��� ��ư�� ���� �� �۵��ϴ� �Լ��Դϴ�
    public void Quantity() {
        Upgrade--;
        float Add = 0;
        Add = Aim_Fire.Bullets * 0.125f; // ���� źȯ���� 12.5% ���� ���մϴ�
        Add = Mathf.CeilToInt(Add); // �� �� �Ҽ��� �ø� ������ ������ �մϴ�
        //Debug.Log(Add);
        // Add ��ġ ��ŭ źȯ�� ������ŵ�ϴ�
        for (int i = 0; i < Add; i++){
            Aim_Fire.Bullets++;
            instance.BulletCounter();
        }
    }

    public void PowerUp(){
        Upgrade--;
        float multiple = 0;
        int subtract = 0;
        multiple = Aim_Fire.Bullets * 0.375f; // ���� źȯ���� 37.5% ���� ���մϴ�
        subtract = Mathf.CeilToInt(multiple); // �� �� �Ҽ��� �ø� ������ ������ �մϴ�
        Aim_Fire.Bullets -= subtract; // ���� źȯ���� subtract ��ġ��ŭ �����մϴ�
        UI_BulletCount -= subtract;
        BossSpawn.DealBuff++; // ���ط��� ������ŵ�ϴ�
        BulletUI.SetText("Bullet : " + UI_BulletCount.ToString() + "\nDeal : " + (1+BossSpawn.DealBuff));
    }
}