using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BGM : MonoBehaviour{
    // ��� ������ ���� �ڵ��Դϴ�

    private AudioSource BackMusic;

    [SerializeField]
    private AudioClip[] Music;
    public static int BGM_Count = 0;
    public static bool BGM_Skip = false;

    public static int BGMTimer = 0;
    [SerializeField]
    private TextMeshProUGUI BGMTime;

    // Start is called before the first frame update
    void Start(){
        BGMTimer = 229;
        BGMTime.SetText("Next Round\n" + (BGMTimer/60)+":"+(BGMTimer%60));
        BGM_Count = 0;
        BGM_Skip = false;
        BackMusic = GetComponent<AudioSource>();
        BackMusic.clip = Music[0];
        BackMusic.Play();
        BGM_Count++;
        Debug.Log("BGM_Count : " + BGM_Count);
        StartCoroutine("BGM_List");
    }

    // �� �� ������ �۵������� Ȯ���ؼ� �۵������� ���� ���, ���� ���� �۵���ŵ�ϴ�
    // ������ óġ�Ͽ� BGM_Skip�� true�� �� ��쿡�� ���� ���� ������Ű�� ���� ���� �۵���ŵ�ϴ�
    IEnumerator BGM_List(){
        while (true){
            yield return new WaitForSeconds(1.0f);

            if (!BackMusic.isPlaying){
                BackMusic.clip = Music[BGM_Count];
                BackMusic.Play();
                BGM_Count++;
                Debug.Log("BGM_Count : " + BGM_Count);
            }
            if (BGM_Skip == true){
                BackMusic.Stop();
                BGM_Skip = false;
            }
            if(BGM_Count == 12 && AnimalsSpawn.Bosses == true){
                BGM_Count = 0;
            }

            BGMTimer--;
            BGMTime.SetText("Next Round\n" + (BGMTimer / 60) + ":" + (BGMTimer % 60));
        }
    }

    public void BGM_Skip_Test(){
        BGM_Skip=true;
        BGMTimer = 7;
    }
}