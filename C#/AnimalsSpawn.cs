using System.Collections;
using TMPro;
using UnityEngine;

public class AnimalsSpawn : MonoBehaviour{
    // ������ ��ȯ ���ǿ� ���õ� �ڵ��Դϴ�

    [SerializeField]
    private GameObject[] Animals;

    [SerializeField]
    private GameObject Event;

    private int Phase = 0;

    // �������� ������Ű�� ���� X��ǥ�Դϴ�
    private float[] AnimalPosX = { -2.5f, -1.5f, -0.5f, 0.5f, 1.5f, 2.5f };

    // ������ ���� Ƚ���� �˱� ���� �־����ϴ�
    private int SpawnCount = 0;

    // �ʷϻ� Event�� ������ ���� ��� ������ Ȯ���� �ø��� ����ġ�Դϴ�
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
        // ���� ���� �� ���� ������ ��Ȱ��ȭ�մϴ�
        if (Bosses == false){

            // ���� �ߺ� ���� Ȯ�ο� ������ �ʱ�ȭ�մϴ�
            int[] checking = new int[AnimalPosX.Length];
            for(int i = 0; i<checking.Length; i++){
                checking[i] = -1;
            }
            int check = 0;
            // ���� ���� ����(6��) �� ��ĭ���� ���� 1�� �������� �����մϴ�
            float Gap = Random.Range(0, 6);

            foreach (float PosX in AnimalPosX){
                // �������� ������ ��(���� Gap)�� ���� X���� ��ġ�� ���, �ش� �ٿ��� ������ �������� �ʽ��ϴ�
                if (PosX+2.5 == Gap){
                    // �ʷϻ� Event ������Ʈ �������� 10% ������ ������ ���� �����̱⿡, 0~9 �������� �����մϴ�
                    int Lucky = Random.Range(0, 10);
                    // ���� ���� 9-������ �� ��� �ʷϻ� Event ������Ʈ�� �����ϰ�, ���� ��ġ�� �ʱ�ȭ�մϴ�
                    if (Lucky >= (9 - correction)){
                        Vector3 EventPos = new Vector3(PosX, transform.position.y, transform.position.z);
                        Instantiate(Event, EventPos, Quaternion.identity);
                        correction = 0;
                        SpawnCount++;
                        // ���� �ʷϻ� Event ������Ʈ ������ 4�� ������ ���, �������� ü���� ������ŵ�ϴ�
                        if(SpawnCount == 4){
                            BossSpawn.Advantage--;
                            SpawnCount = 0;
                            StatUp.SetActive(true);
                            StatUpUI.SetText("Aniaml MaxHp Up!" + "\nHp : " + (2*(BossSpawn.BC + 1) - BossSpawn.Advantage)+", " + (3 * (BossSpawn.BC + 1) - BossSpawn.Advantage) + ", " + (4 * (BossSpawn.BC + 1) - BossSpawn.Advantage));
                        }
                    }
                    // ���� �ʷϻ� Event ������Ʈ�� ������Ű�� ������ ���, Ȯ�� ���� ��ġ�� �ø��ϴ�
                    else{
                        correction++;
                    }
                    // �ʷϻ� Event ������Ʈ ���� Ȯ�� UI�� �����մϴ�
                    BonusSpawn.SetText("Spawn\n" + (1+correction)*10 + "%");
                }
                else{
                    Reroll:
                    // 12���� ���� �� = 12����
                    int AnimalsIndex = 12;
                    int index = Random.Range(0, AnimalsIndex);
                    for (int i = 0; i < checking.Length; i++){
                        // ���� �������� ������ ���� �̹� ��ȯ�� ������ ���, Reroll: �������� ���������� �ߺ� ��ȯ�� �����ϴ�
                        if (checking[i] == index){
                            goto Reroll;
                        }
                    }
                    // ���� �ߺ��� �ƴ� ���, üũ�� �迭�� ��ȯ�� ������ ������Ѽ� ���� Ȯ�� ������ ���� ���� ���� ��ȯ ����
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