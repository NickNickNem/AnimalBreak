using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Aim_Fire : MonoBehaviour{
    // 게임 화면의 대포에 대한 코드입니다

    [SerializeField]
    private GameObject Cannon;

    [SerializeField]
    private GameObject Bullet;

    [SerializeField]
    private Transform BulletTransform;

    [SerializeField]
    private GameObject AimLine;

    [SerializeField]
    private float InputInterval = 0.025f;

    [SerializeField]
    private float LastInput = 0f;

    [SerializeField]
    private float RotateSpeed = 0.75f;

    public static int Bullets = 1;
    public static float FireRate = 0.5f;

    public static int Ready = -1;

    private bool Left = false;
    private bool Right = false;
    [SerializeField]
    private GameObject FireUI;
    [SerializeField]
    private GameObject SkipUI;
    [SerializeField]
    private GameObject LeftUI;
    [SerializeField]
    private GameObject RightUI;

    [SerializeField]
    private AudioSource Fire_SFX;
    [SerializeField]
    private AudioClip SFX;

    /*
    발사 기능, 준비 상태(Ready == 0)이면서 강화 수치를 다 사용 했을 경우(&& GameManager.Upgrade <= 0) 사용 가능합니다
    준비 상태를 1로 증가시켜서 여러번 발사하는것을 막고, 발사 효과음을 실행시키며(Fire_SFX.Play();)
    조준선, 발사버튼 UI를 비활성화({AimLine, FireUI}.SetActive(false)) 후 생략 버튼 UI를 활성화(SkipUI.SetActive(true);)
    그리고 탄환 생성 루틴을 실행한다(StartCoroutine(BulletFire());)
    */
    public void Fire(){
        if(Ready == 0 && GameManager.Upgrade <= 0){
            Ready++;
            Fire_SFX.Play();
            AimLine.SetActive(false);
            FireUI.SetActive(false);
            SkipUI.SetActive(true);
            StartCoroutine(BulletFire());
        }
    }

    // 발사 가능한 탄환 수량까지, {FireRate} 초 간격으로 탄환을 생성합니다
    IEnumerator BulletFire(){
        for(int i = 0; i < Bullets; i++){
            Instantiate(Bullet, BulletTransform.position, transform.rotation);
            yield return new WaitForSeconds(FireRate);
        }
    }

    // 좌 우 버튼을 누르는 동안, 0.025초 간격으로 0.75도씩 회전합니다
    // 단! 회전 가능한 반경은 +- 약 60도씩 입니다
    void Rotate(){
        if(Time.time - LastInput > InputInterval){
            if ((Input.GetKey(KeyCode.LeftArrow) || Left == true) && (Cannon.transform.eulerAngles[2] < 59.9 || Cannon.transform.eulerAngles[2] >= 299.9)){
                //Debug.Log("Left");
                transform.Rotate(0, 0, RotateSpeed);
                LastInput = Time.time;
                //Debug.Log(Cannon.transform.eulerAngles[2]);
                //Debug.Log(Cannon.transform.rotation.z);
            }else if ((Input.GetKey(KeyCode.RightArrow) || Right == true) && (Cannon.transform.eulerAngles[2] < 59.9 || Cannon.transform.eulerAngles[2] >= 299.9)){
                transform.Rotate(0, 0, -RotateSpeed);
                LastInput = Time.time;
            }
        }
    }

    // 아래의 public 함수들은 모바일용(버튼)누르는 동안 회전시키게 만들기
    // ↓ 챗 GPT에서 복붙한 구간 ↓
    public void PressLeft(){
        Left = true;
    }
    public void ReleaseLeft(){
        Left=false;
    }

    public void PressRight(){
        Right = true;
    }
    public void ReleaseRight(){
        Right = false;
    }

    bool IsPointerOverUIObject(){
        // 현재 마우스 위치에 UI 요소가 있는지 확인
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    void RotateCannonTowardsMouse(Vector3 targetPosition){
        Vector2 direction = new Vector2(targetPosition.x - Cannon.transform.position.x, targetPosition.y - Cannon.transform.position.y);

        // 방향 벡터를 회전 각도로 변환
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);

        // Cannon을 회전시킵니다.
        Cannon.transform.rotation = rotation;
    }
    // ↑ 챗 GPT에서 복붙한 구간 종료 ↑

    // Start is called before the first frame update
    // 실행할 때 처음 1번 작동하는 초기화 함수들입니다
    void Start(){
        Ready = -1;
        Bullets = 1;
        AimLine = GameObject.Find("AimLine");
        Cannon = GameObject.Find("Cannon");
        FireRate = 0.5f;
        Left = false;
        Right = false;
        Fire_SFX = GetComponent<AudioSource>();
        Fire_SFX.clip = SFX;
    }

    // Update is called once per frame
    /*
    1프레임마다 반복하는 무한 함수입니다
    준비 상태(Ready == 0)일 경우, {강화 가능하다 → 발사 버튼 비활성화 // 강화를 다 했다 → 조준선과 발사 버튼 활성화}
    PC의 경우 방향키 ↑ 키를 통해 발사할 수 있으며, 그 외에는 발사 버튼에서 Fire() 함수를 호출하여 발사합니다
    좌우 버튼을 통해 반시계, 시계 방향으로 포신을 회전시킬 수 있습니다
     */
    void Update(){
        if(Ready == 0){
            if(GameManager.Upgrade > 0){
                FireUI.SetActive(false);
            }else{
                AimLine.SetActive(true);
                FireUI.SetActive(true);
            }
            SkipUI.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)){
            Fire();
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Left == true || Right == true){
            Rotate();
        }

        if (Input.GetMouseButton(0) && (!(GameManager.Upgrade > 0) || Ready != 0)){
            if (!IsPointerOverUIObject()) {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (mousePosition.y >= -2.9f && (mousePosition.x > -3 && mousePosition.x < 3)){
                    RotateCannonTowardsMouse(mousePosition);
                }
            }
        }
    }

    public void Upgrade_Test(){
        GameManager.Upgrade++;
    }
}