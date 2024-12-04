using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstScreen : MonoBehaviour{
    // ù ȭ�鿡�� UI�� �����ϴ� �ڵ��Դϴ�

    [SerializeField]
    private GameObject Main;
    [SerializeField]
    private GameObject HowToPlay;
    [SerializeField]
    private GameObject Stats;
    [SerializeField]
    private GameObject Copyright;

    // Start is called before the first frame update
    void Start(){
        Main.SetActive(true);
        HowToPlay.SetActive(false);
        Stats.SetActive(false);
        Copyright.SetActive(false);
    }

    // [Close] ��ư�� ������ â�� ���� ���, �ʱ� �Լ��� �����ŵ�ϴ�
    public void Close(){
        Start();
    }

    // [Game Start] ��ư�� ���� ���, ���� ȭ������ ��ȯ�մϴ�
    public void Starting(){
        SceneManager.LoadScene("InGame");
        //Debug.Log("Start!");
    }

    // [How to Play] ��ư�� ���� ���, ���� ���� UI�� Ȱ��ȭ �ϰ� �������� ��Ȱ��ȭ ��ŵ�ϴ�
    public void How_To_Play(){
        Main.SetActive(false);
        HowToPlay.SetActive(true);
        Copyright.SetActive(false);
    }

    // How to Play�� [Next] ��ư�� ���� ���, ���� �ɷ�ġ ���� UI�� Ȱ��ȭ �ϰ� �������� ��Ȱ��ȭ ��ŵ�ϴ�
    public void HtP_Next(){
        HowToPlay.SetActive(false);
        Stats.SetActive(true);
    }

    // How to Play�� ���� �ɷ�ġ �κп��� [Back] ��ư�� ���� ���, ���� ���� UI�� Ȱ��ȭ �ϰ� �������� ��Ȱ��ȭ ��ŵ�ϴ�
    public void Stats_Back(){
        HowToPlay.SetActive(true);
        Stats.SetActive(false);
    }

    // [BGM] ��ư�� ���� ���, BGM ���۱� �ȳ� UI�� Ȱ��ȭ �ϰ� �������� ��Ȱ��ȭ ��ŵ�ϴ�
    public void BGM(){
        Main.SetActive(false);
        Stats.SetActive(false);
        Copyright.SetActive(true);
    }

    // [Exit] ��ư�� ���� ���, ������ �����մϴ�
    public void Exit(){
        Application.Quit();
        // �� �Ʒ� �ڵ�� ����Ƽ���� �ùķ����� ������ �뵵�̹Ƿ� Build�� �� �ּ�ó�� �Ұ�
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    // Update is called once per frame
    void Update(){
        
    }
}
