using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour{
    // 이 코드는 챗 GPT에게서 복사 붙여넣기 한 코드입니다
    // 해상도에 따라 비율을 맞추려는 함수입니다
    // 각 화면에서 1번씩만 발동하기에 갤럭시 플립이나 PC에서 창 크기를 변경할 경우 비율이 안맞습니다
    // 그것을 해결하기에 매 초 함수를 작동하는건 엄청난 메모리 낭비라고 판단하여 생략했습니다

    // Start is called before the first frame update
    void Start(){
        /*SetResolution();
        Camera.main.aspect = 6f / 10f;*/
        Camera cam = GetComponent<Camera>();

        Rect viewportRect = cam.rect;

        float screenAspectRatio = (float)Screen.width / Screen.height;
        float targetAspectRatio = 6f / 10f;

        //Screen.SetResolution(Screen.width, Screen.height, false);

        if (screenAspectRatio < targetAspectRatio){
            viewportRect.height = screenAspectRatio / targetAspectRatio;
            viewportRect.y = (1f - viewportRect.height) / 2f;
        }else{
            viewportRect.width = targetAspectRatio / screenAspectRatio;
            viewportRect.x = (1f - viewportRect.width) / 2f;
        }

        cam.rect = viewportRect;
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void SetResolution(){
        int setWidth = 600; // 화면 너비
        int setHeight = 1000; // 화면 높이

        //해상도를 설정값에 따라 변경
        //3번째 파라미터는 풀스크린 모드를 설정 > true : 풀스크린, false : 창모드
        Screen.SetResolution(setWidth, setHeight, false);
    }
}
