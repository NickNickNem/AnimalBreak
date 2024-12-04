using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour{

    Rigidbody2D rb;

    [SerializeField]
    private float MoveSpeed = 7.5f;

    public static int Damage = 1;

    private float ReflectDirection = 0;


    // 탄환의 운동 방향이 반전될 때, 이미지 또한 반전시킵니다 (X축)
    // 현재 각도에 따라서 이미지를 회전시키는 방식입니다
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

    // 탄환의 운동 방향이 반전될 때, 이미지 또한 반전시킵니다 (Y축)
    // 현재 각도에 따라서 이미지를 회전시키는 방식입니다
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
    탄환이 충돌된 속성이 {측면 벽, 천장, 동물, 동물_기절됨, 보스, 방어막} 에 따라서 기능이 달라집니다
    측면 벽 : X방향을 반전시키는 함수를 사용합니다
    천장 : Y방향을 반전시키는 함수를 사용합니다
    동물, 동물_기절됨 : 탄환의 현재 위치와, 동물의 현재 위치를 찾아서 감산을 합니다
    X축의 절대값이 0.5 이하라면 X방향을 반전시키고 아닐 경우 Y방향을 반전시킵니다
    방어막 : 탄환이 제거됩니다
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

    // 사라진 탄환 개수를 증가시킵니다
    private void OnDestroy(){
        //Debug.Log("부셔졌엉");
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
    // 방향키 ↓를 누르거나 y축 값이 -5.1 이하(맵 이탈)일 경우 제거합니다
    void Update(){
        if (Input.GetKeyDown(KeyCode.DownArrow)){
            Destroy(gameObject);
        }
        if (transform.position[1] < -5.1){
            Destroy(gameObject);
        }
        // 맨 처음엔 아래 주석문으로 탄환을 움직였으나, 지금은 void Strat의 Rigidbody2D를 이용하여 움직이고 있어요
        /* transform.position += Vector3.up * MoveSpeed * Time.deltaTime;
        rb.AddForce (transform.up*MoveSpeed); */
    }

    // 생략 버튼을 누를 경우에도 탄환을 제거합니다
    public void Skip(){
        GameObject[] Each = GameObject.FindGameObjectsWithTag("Bullet");
        for(int i = 0; i <Each.Length; i++){
            Destroy(Each[i]);
        }
    }
}