using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSpawn : MonoBehaviour{
    // 보스 생성 함수입니다

    [SerializeField]
    private GameObject Boss1;
    [SerializeField]
    private GameObject Boss2;
    [SerializeField]
    private GameObject Boss3;
    [SerializeField]
    private GameObject Boss4;

    public static int BC = 0;
    public static int Advantage = 0;
    public static int DealBuff = 0;

    [SerializeField]
    private GameObject Field;
    [SerializeField]
    private GameObject[] BS_BG;

    [SerializeField]
    private GameObject StatUp;
    [SerializeField]
    private TextMeshProUGUI StatUpUI;

    private int[] BC_Slot;

    // Start is called before the first frame update
    void Start() {
        BC_Slot = new int[] {246, 219, 176, 356};
        BC = 0;
        Advantage = 0;
        DealBuff = 0;
        StartCoroutine(BS());
    }

    // 매 초 BGM의 배열 값을 조회하여 보스 테마곡 차례가 될 경우 보스를 소환합니다
    IEnumerator BS(){
        while (true){
            if(AnimalsSpawn.Bosses == false){
                if (BGM.BGM_Count == 3 && BC == 0){
                    Vector3 BSS = new Vector3(0.25f, -0.2f, 0);
                    Instantiate(Boss1, BSS, transform.rotation);
                    BGM.BGMTimer = BC_Slot[BC];
                    ChangeUI();
                    yield return new WaitForSeconds(5);
                    StatUp.SetActive(false);
                }else if (BGM.BGM_Count == 6 && BC == 1){
                    Vector3 BSS = new Vector3(0, 0, 0);
                    Instantiate(Boss2, BSS, transform.rotation);
                    BGM.BGMTimer = BC_Slot[BC];
                    ChangeUI();
                    yield return new WaitForSeconds(5);
                    StatUp.SetActive(false);
                }else if (BGM.BGM_Count == 9 && BC == 2){
                    Vector3 BSS = new Vector3(1, -0.8f, 0);
                    Instantiate(Boss3, BSS, transform.rotation);
                    BGM.BGMTimer = BC_Slot[BC];
                    ChangeUI();
                    yield return new WaitForSeconds(5);
                    StatUp.SetActive(false);
                }else if (BGM.BGM_Count == 12 && BC == 3){
                    Vector3 BSS = new Vector3(0.625f, 0.05f, 0);
                    Instantiate(Boss4, BSS, transform.rotation);
                    BGM.BGMTimer = BC_Slot[BC];
                    ChangeUI();
                    yield return new WaitForSeconds(5);
                    StatUp.SetActive(false);
                    BGM.BGM_Count = 0;
                }
            }

            if (BGM.BGM_Count % 3 != 0){
                Field.SetActive(true);
                BS_BG[0].SetActive(false);
                BS_BG[1].SetActive(false);
                BS_BG[2].SetActive(false);
                BS_BG[3].SetActive(false);
            }

            yield return new WaitForSeconds(1);
        }
    }

    // 보스가 생성될 때 마다 배경이 바뀌고 이후 생성될 동물들의 능력치가 상승하며 이를 UI로도 알려줍니다
    private void ChangeUI(){
        BS_BG[BC].SetActive(true);
        Field.SetActive(false);
        AnimalsSpawn.Bosses = true;
        BC++;
        StatUp.SetActive(true);
        StatUpUI.SetText("Aniaml MaxHp Up!" + "\nHp : " + (2 * (BC + 1) - Advantage) + ", " + (3 * (BC + 1) - Advantage) + ", " + (4 * (BC + 1) - Advantage));
    }

    // Update is called once per frame
    void Update(){
        
    }
}