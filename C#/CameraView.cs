using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour{
    // �� �ڵ�� ê GPT���Լ� ���� �ٿ��ֱ� �� �ڵ��Դϴ�
    // �ػ󵵿� ���� ������ ���߷��� �Լ��Դϴ�
    // �� ȭ�鿡�� 1������ �ߵ��ϱ⿡ ������ �ø��̳� PC���� â ũ�⸦ ������ ��� ������ �ȸ½��ϴ�
    // �װ��� �ذ��ϱ⿡ �� �� �Լ��� �۵��ϴ°� ��û�� �޸� ������ �Ǵ��Ͽ� �����߽��ϴ�

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
        int setWidth = 600; // ȭ�� �ʺ�
        int setHeight = 1000; // ȭ�� ����

        //�ػ󵵸� �������� ���� ����
        //3��° �Ķ���ʹ� Ǯ��ũ�� ��带 ���� > true : Ǯ��ũ��, false : â���
        Screen.SetResolution(setWidth, setHeight, false);
    }
}
