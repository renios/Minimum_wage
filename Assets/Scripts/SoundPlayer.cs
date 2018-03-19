using System.Collections;
using UnityEngine;

public class SoundPlayer : MonoBehaviour{
    public static float soundVolume = 1;
    public static float musicVolume = 1;
    bool isMusicPlayer = false;
    bool isPlaying = false;
    AudioSource audio;
    float duration;
    float time;

    public void SetMusicPlayer(){
        isMusicPlayer = true;
    }
    void Awake(){
        audio = GetComponent<AudioSource>();
    }
    public void Play(AudioClip clip, Vector2 pos = new Vector2(), bool isLoop = false){
        SoundManager.AddUsingSoundPlayer(this);
        audio.Pause();
        audio.clip = clip;
        transform.position = pos;
        if (isMusicPlayer) audio.loop = isLoop;
        if (!audio.loop) {
            duration = clip.length;
            time = 0;
        }
        isPlaying = true;
        audio.Play();
    }  
    public void Pause(){
        audio.Pause();
    }
    public void Unpause(){
        audio.UnPause();
    }
    public void Stop(){
        audio.Stop();
    }
    public void SetVolume(){
        audio.volume = isMusicPlayer? musicVolume : soundVolume;
    }
    void Update(){

        if (isPlaying) time += Time.deltaTime;

        if (!audio.loop && time >= duration){
            SoundManager.RemoveUsingSoundPlayer(this);
            SoundManager.PushUsedSoundPlayer(this);
            isPlaying = false;
        }
    }
}