using System.Collections;
using UnityEngine;

public class SoundPlayer : MonoBehaviour{
    public static float soundVolume = 1;
    public static float musicVolume = 1;
    bool isMusicPlayer = false;
    AudioSource audio;

    public void SetMusicPlayer(){
        isMusicPlayer = true;
    }
    void Awake(){
        audio = GetComponent<AudioSource>();
    }
    public void Play(AudioClip clip, Vector2 pos = new Vector2()){
        audio.Stop();
        audio.clip = clip;
        transform.position = pos;
        audio.Play();
        if (isMusicPlayer) audio.loop = true;
        else StartCoroutine(PushAfterDelay(clip.length));
    }  
    IEnumerator PushAfterDelay(float length){
        yield return new WaitForSeconds(length);
        SoundManager.PushUsedSoundPlayer(this);
    }
    public void Stop(){
        audio.Stop();
    }
    void Update(){
        audio.volume = isMusicPlayer? musicVolume : soundVolume;
    }
}