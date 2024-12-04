using System.Collections;
using TMPro;
using UnityEngine;

public class AnimalsSpawn : MonoBehaviour{
    // 동물들 소환 조건에 관련된 코드입니다

    [SerializeField]
    private GameObject[] Animals;

    [SerializeField]
    private GameObject Event;

    private int Phase = 0;

    // 동물들을 생성시키기 위한 X좌표입니다
    private float[] AnimalPosX = { -2.5f, -1.5f, -0.5f, 0.5f, 1.5f, 2.5f };

    // 동물들 생성 횟수를 알기 위해 넣었습니다
    private int SpawnCount = 0;

    // 초록색 Event가 나오지 않을 경우 생성될 확률을 늘리는 보정치입니다
    private int correction = 0;

    public static bool Bosses = false;

    [SerializeField]
    private GameObject StatUp;
    [SerializeField]
    private TextMeshProUGUI StatUpUI;
    [SerializeField]
    private TextMeshProUGUI BonusSpawn;

    // Start is called before the first frame update
    void Start(){
        Phase = Aim_Fire.Ready;
        correction = 9;
        Bosses = false;
        StatUp.SetActive(false);
    }

    void StartSpawnRoutine(){
        Aim_Fire.Ready++;
        StartCoroutine("SpawnRoutine");
    }

    IEnumerator SpawnRoutine(){
        // 보스 출현 시 동물 생성을 비활성화합니다
        if (Bosses == false){

            // 동물 중복 생성 확인용 변수를 초기화합니다
            int[] checking = new int[AnimalPosX.Length];
            for(int i = 0; i<checking.Length; i++){
                checking[i] = -1;
            }
            int check = 0;
            // 동물 생성 구역(6개) 중 빈칸으로 나올 1개 랜덤으로 결정합니다
            float Gap = Random.Range(0, 6);

            foreach (float PosX in AnimalPosX){
                // 랜덤으로 결정된 값(변수 Gap)과 생성 X축이 일치할 경우, 해당 줄에는 동물이 생성되지 않습니다
                if (PosX+2.5 == Gap){
                    // 초록색 Event 오브젝트 생성률을 10% 단위로 변동이 있을 예정이기에, 0~9 랜덤값을 생성합니다
                    int Lucky = Random.Range(0, 10);
                    // 랜덤 값이 9-보정값 일 경우 초록색 Event 오브젝트를 생성하고, 보정 수치를 초기화합니다
                    if (Lucky >= (9 - correction)){
                        Vector3 EventPos = new Vector3(PosX, transform.position.y, transform.position.z);
                        Instantiate(Event, EventPos, Quaternion.identity);
                        correction = 0;
                        SpawnCount++;
                        // 만약 초록색 Event 오브젝트 생성이 4번 생성될 경우, 동물들의 체력을 증가시킵니다
                        if(SpawnCount == 4){
                            BossSpawn.Advantage--;
                            SpawnCount = 0;
                            StatUp.SetActive(true);
                            StatUpUI.SetText("Aniaml MaxHp Up!" + "\nHp : " + (2*(BossSpawn.BC + 1) - BossSpawn.Advantage)+", " + (3 * (BossSpawn.BC + 1) - BossSpawn.Advantage) + ", " + (4 * (BossSpawn.BC + 1) - BossSpawn.Advantage));
                        }
                    }
                    // 만약 초록색 Event 오브젝트를 생성시키지 못했을 경우, 확률 보정 수치를 올립니다
                    else{
                        correction++;
                    }
                    // 초록색 Event 오브젝트 생성 확률 UI를 갱신합니다
                    BonusSpawn.SetText("Spawn\n" + (1+correction)*10 + "%");
                }
                else{
                    Reroll:
                    // 12지신 동물 수 = 12마리
                    int AnimalsIndex = 12;
                    int index = Random.Range(0, AnimalsIndex);
                    for (int i = 0; i < checking.Length; i++){
                        // 만약 무작위로 생성한 값이 이미 소환된 동물일 경우, Reroll: 지점으로 돌려보내서 중복 소환을 막습니다
                        if (checking[i] == index){
                            goto Reroll;
                        }
                    }
                    // 만약 중복이 아닐 경우, 체크용 배열에 소환한 동물을 저장시켜서 위의 확인 절차를 통해 동일 동물 소환 방지
                    checking[check] = index;
                    check++;
                    RandomSpawn(PosX, index);
                }
            }
        }
        yield return new WaitForSeconds(5f);
        StatUp.SetActive(false);
    }

    void RandomSpawn(float AnimalPosX, int index){
        Vector3 SpawnPos = new Vector3(AnimalPosX, transform.position.y, transform.position.z);
        Instantiate(Animals[index], SpawnPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update(){
        Phase = Aim_Fire.Ready;
        if (Phase == -1){
            //Debug.Log(Phase);
            StartSpawnRoutine();
        }
    }
}