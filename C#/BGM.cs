using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BGM : MonoBehaviour{
    // 배경 음악을 위한 코드입니다

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

    // 매 초 음악이 작동중인지 확인해서 작동중이지 않을 경우, 다음 곡을 작동시킵니다
    // 보스를 처치하여 BGM_Skip이 true가 된 경우에는 현재 곡을 중지시키고 다음 곡을 작동시킵니다
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