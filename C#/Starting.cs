using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstScreen : MonoBehaviour{
    // 첫 화면에서 UI를 조작하는 코드입니다

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

    // [Close] 버튼을 눌러서 창을 닫을 경우, 초기 함수를 실행시킵니다
    public void Close(){
        Start();
    }

    // [Game Start] 버튼을 누를 경우, 게임 화면으로 전환합니다
    public void Starting(){
        SceneManager.LoadScene("InGame");
        //Debug.Log("Start!");
    }

    // [How to Play] 버튼을 누를 경우, 게임 설명 UI만 활성화 하고 나머지를 비활성화 시킵니다
    public void How_To_Play(){
        Main.SetActive(false);
        HowToPlay.SetActive(true);
        Copyright.SetActive(false);
    }

    // How to Play의 [Next] 버튼을 누를 경우, 동물 능력치 설명 UI만 활성화 하고 나머지를 비활성화 시킵니다
    public void HtP_Next(){
        HowToPlay.SetActive(false);
        Stats.SetActive(true);
    }

    // How to Play의 동물 능력치 부분에서 [Back] 버튼을 누를 경우, 게임 설명 UI만 활성화 하고 나머지를 비활성화 시킵니다
    public void Stats_Back(){
        HowToPlay.SetActive(true);
        Stats.SetActive(false);
    }

    // [BGM] 버튼을 누를 경우, BGM 저작권 안내 UI만 활성화 하고 나머지를 비활성화 시킵니다
    public void BGM(){
        Main.SetActive(false);
        Stats.SetActive(false);
        Copyright.SetActive(true);
    }

    // [Exit] 버튼을 누를 경우, 게임을 종료합니다
    public void Exit(){
        Application.Quit();
        // ↓ 아래 코드는 유니티에서 시뮬레이터 돌리는 용도이므로 Build할 때 주석처리 할것
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    // Update is called once per frame
    void Update(){
        
    }
}
