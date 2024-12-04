using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Aim_Fire : MonoBehaviour{
    // ���� ȭ���� ������ ���� �ڵ��Դϴ�

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
    �߻� ���, �غ� ����(Ready == 0)�̸鼭 ��ȭ ��ġ�� �� ��� ���� ���(&& GameManager.Upgrade <= 0) ��� �����մϴ�
    �غ� ���¸� 1�� �������Ѽ� ������ �߻��ϴ°��� ����, �߻� ȿ������ �����Ű��(Fire_SFX.Play();)
    ���ؼ�, �߻��ư UI�� ��Ȱ��ȭ({AimLine, FireUI}.SetActive(false)) �� ���� ��ư UI�� Ȱ��ȭ(SkipUI.SetActive(true);)
    �׸��� źȯ ���� ��ƾ�� �����Ѵ�(StartCoroutine(BulletFire());)
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

    // �߻� ������ źȯ ��������, {FireRate} �� �������� źȯ�� �����մϴ�
    IEnumerator BulletFire(){
        for(int i = 0; i < Bullets; i++){
            Instantiate(Bullet, BulletTransform.position, transform.rotation);
            yield return new WaitForSeconds(FireRate);
        }
    }

    // �� �� ��ư�� ������ ����, 0.025�� �������� 0.75���� ȸ���մϴ�
    // ��! ȸ�� ������ �ݰ��� +- �� 60���� �Դϴ�
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

    // �Ʒ��� public �Լ����� ����Ͽ�(��ư)������ ���� ȸ����Ű�� �����
    // �� ê GPT���� ������ ���� ��
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
        // ���� ���콺 ��ġ�� UI ��Ұ� �ִ��� Ȯ��
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    void RotateCannonTowardsMouse(Vector3 targetPosition){
        Vector2 direction = new Vector2(targetPosition.x - Cannon.transform.position.x, targetPosition.y - Cannon.transform.position.y);

        // ���� ���͸� ȸ�� ������ ��ȯ
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);

        // Cannon�� ȸ����ŵ�ϴ�.
        Cannon.transform.rotation = rotation;
    }
    // �� ê GPT���� ������ ���� ���� ��

    // Start is called before the first frame update
    // ������ �� ó�� 1�� �۵��ϴ� �ʱ�ȭ �Լ����Դϴ�
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
    1�����Ӹ��� �ݺ��ϴ� ���� �Լ��Դϴ�
    �غ� ����(Ready == 0)�� ���, {��ȭ �����ϴ� �� �߻� ��ư ��Ȱ��ȭ // ��ȭ�� �� �ߴ� �� ���ؼ��� �߻� ��ư Ȱ��ȭ}
    PC�� ��� ����Ű �� Ű�� ���� �߻��� �� ������, �� �ܿ��� �߻� ��ư���� Fire() �Լ��� ȣ���Ͽ� �߻��մϴ�
    �¿� ��ư�� ���� �ݽð�, �ð� �������� ������ ȸ����ų �� �ֽ��ϴ�
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