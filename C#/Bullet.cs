using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour{

    Rigidbody2D rb;

    [SerializeField]
    private float MoveSpeed = 7.5f;

    public static int Damage = 1;

    private float ReflectDirection = 0;


    // źȯ�� � ������ ������ ��, �̹��� ���� ������ŵ�ϴ� (X��)
    // ���� ������ ���� �̹����� ȸ����Ű�� ����Դϴ�
    void X_ReflectValues(){
        Vector3 Recochet = Vector3.Reflect(rb.velocity, Vector3.right);
        rb.velocity = Recochet;
        if (transform.eulerAngles[2] > 270){
            ReflectDirection = (transform.eulerAngles[2] - 360) * -2;
            //Debug.Log(ReflectDirection);
            transform.Rotate(0 ,0, ReflectDirection);
        }else if(transform.eulerAngles[2] < 90){
            ReflectDirection = transform.eulerAngles[2] * -2;
            transform.Rotate(0, 0, ReflectDirection);
        }else{
            ReflectDirection = (transform.eulerAngles[2] - 180)* -2;
            transform.Rotate(0, 0, ReflectDirection);
        }
    }

    // źȯ�� � ������ ������ ��, �̹��� ���� ������ŵ�ϴ� (Y��)
    // ���� ������ ���� �̹����� ȸ����Ű�� ����Դϴ�
    void Y_ReflectValues(){
        Vector3 Recochet = Vector3.Reflect(rb.velocity, Vector3.down);
        rb.velocity = Recochet;
        if (transform.eulerAngles[2] < 90){
            ReflectDirection = Mathf.Abs(transform.eulerAngles[2] - 90);
            //Debug.Log(transform.eulerAngles[2]);
            //Debug.Log(ReflectDirection);
            transform.Rotate(0, 0, ReflectDirection * 2);
        }else if(transform.eulerAngles[2] < 180){
            ReflectDirection = transform.eulerAngles[2] - 90;
            transform.Rotate(0, 0, ReflectDirection * -2);
        }else if(transform.eulerAngles[2] < 270){
            ReflectDirection = Mathf.Abs(transform.eulerAngles[2] - 270);
            transform.Rotate(0, 0, ReflectDirection * 2);
        }else{
            ReflectDirection = transform.eulerAngles[2] - 270;
            transform.Rotate(0, 0, ReflectDirection * -2);
        }
    }

    /*
    źȯ�� �浹�� �Ӽ��� {���� ��, õ��, ����, ����_������, ����, ��} �� ���� ����� �޶����ϴ�
    ���� �� : X������ ������Ű�� �Լ��� ����մϴ�
    õ�� : Y������ ������Ű�� �Լ��� ����մϴ�
    ����, ����_������ : źȯ�� ���� ��ġ��, ������ ���� ��ġ�� ã�Ƽ� ������ �մϴ�
    X���� ���밪�� 0.5 ���϶�� X������ ������Ű�� �ƴ� ��� Y������ ������ŵ�ϴ�
    �� : źȯ�� ���ŵ˴ϴ�
     */
    void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Side"){
            //Debug.Log("Collid Side Wall");
            if (transform.position.y < -3){
                Destroy(gameObject);
            }
            X_ReflectValues();
        }else if (other.tag == "Roof"){
            //Debug.Log("Collid Roof Wall");
            Y_ReflectValues();
        }else if (other.tag == "Animals"){
            //Debug.Log("Collid Errors");
            Animal animal = other.gameObject.GetComponent<Animal>();
            Vector2 BulletPoint = transform.position;
            Vector2 AnimalPoint = animal.transform.position;
            Vector2 dir = BulletPoint - AnimalPoint;
            //Debug.Log(dir);

            if (dir[0] > 0.5 || dir[0] < -0.5) {
                X_ReflectValues();
            }else{
                Y_ReflectValues();
            }
        }else if (other.tag == "Animal_Stun"){
            Animal_Stun animal = other.gameObject.GetComponent<Animal_Stun>();
            Vector2 BulletPoint = transform.position;
            Vector2 AnimalPoint = animal.transform.position;
            Vector2 dir = BulletPoint - AnimalPoint;

            if (dir[0] > 0.5 || dir[0] < -0.5){
                X_ReflectValues();
            }else{
                Y_ReflectValues();
            }
        }else if (other.tag == "Boss"){
            Vector2 BulletPoint = transform.position;
            Vector2 BossPoint = other.transform.position;
            Vector2 dir = BulletPoint - BossPoint;
            Debug.Log(dir);

            GameObject Type = GameObject.Find(other.gameObject.name);
            if (Type.name == "Boss1(Clone)"){
                if (dir[1] >= 1.2 || dir[1] <= -0.8){
                    Y_ReflectValues();
                }else{
                    X_ReflectValues();
                }
            }else if(Type.name == "Boss3(Clone)"){
                if (dir[1] >= 1.9 || dir[1] <= -0.3){
                    Y_ReflectValues();
                }else{
                    X_ReflectValues();
                }
            }else{
                if (dir[1] >= 0.9 || dir[1] <= -0.9){
                    Y_ReflectValues();
                }else{
                    X_ReflectValues();
                }
            }
        }else if (other.tag == "Shield"){
            Destroy(gameObject);
        }
    }

    // ����� źȯ ������ ������ŵ�ϴ�
    private void OnDestroy(){
        //Debug.Log("�μ�����");
        GameManager.BulletCount++;
    }

    // Start is called before the first frame update
    void Start(){
        Damage = 1 + BossSpawn.DealBuff;
        MoveSpeed = 7.5f;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * MoveSpeed;
    }
    
    // Update is called once per frame
    // ����Ű �鸦 �����ų� y�� ���� -5.1 ����(�� ��Ż)�� ��� �����մϴ�
    void Update(){
        if (Input.GetKeyDown(KeyCode.DownArrow)){
            Destroy(gameObject);
        }
        if (transform.position[1] < -5.1){
            Destroy(gameObject);
        }
        // �� ó���� �Ʒ� �ּ������� źȯ�� ����������, ������ void Strat�� Rigidbody2D�� �̿��Ͽ� �����̰� �־��
        /* transform.position += Vector3.up * MoveSpeed * Time.deltaTime;
        rb.AddForce (transform.up*MoveSpeed); */
    }

    // ���� ��ư�� ���� ��쿡�� źȯ�� �����մϴ�
    public void Skip(){
        GameObject[] Each = GameObject.FindGameObjectsWithTag("Bullet");
        for(int i = 0; i <Each.Length; i++){
            Destroy(Each[i]);
        }
    }
}