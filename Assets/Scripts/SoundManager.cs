 using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SoundType { Button, Cashier, Coin, FemaleSatisfy, MaleSatisfy, Tap, FemaleDisappoint, MaleDissapoint, Raspberry }
public enum MusicType { Main, Ambient1, Ambient2 }

[System.Serializable]
public class SoundDic{
    public SoundType type;
    public AudioClip clip;
}
[System.Serializable]
public class MusicDic{
    public MusicType type;
    public AudioClip clip;
}
public class SoundManager : MonoBehaviour{
    static SoundManager instance;
    static Queue<SoundPlayer> spPool;
    static SoundPlayer musicPlayer;
    
    public GameObject standardSoundPlayer;
    public SoundDic[] soundDictionary;
    public MusicDic[] musicDictionary;
    static SoundPlayer PullNewSoundPlayer(){
        if (spPool.Count > 0) {
            SoundPlayer sp = spPool.Dequeue();
            sp.gameObject.SetActive(true);
            return sp;
        } else {
            GameObject go = Instantiate(instance.standardSoundPlayer, instance.gameObject.transform);
            return go.GetComponent<SoundPlayer>();
        }
    }
    public static void PushUsedSoundPlayer(SoundPlayer sp) { 
        sp.gameObject.SetActive(false);
        spPool.Enqueue(sp);
    }

    public void PlayNonStatic(SoundType st, Vector2 pos = new Vector2()){
        SoundManager.Play(st, pos);
    }
    public static void Play(SoundType st, Vector2 pos = new Vector2()){
        var selectedDics = instance.soundDictionary.Where(sd => sd.type == st).ToArray();
        if(selectedDics.Length == 0){
            Debug.LogError("NullSoundTypeException : cannot find AudioClip that matches given SoundType(" + st.ToString() + ")");
            return;
        }
        var clip = selectedDics[Random.Range(0,selectedDics.Length)].clip;
        PullNewSoundPlayer().Play(clip, pos);
    }
    public static void Play(MusicType mt){
        var clip = instance.musicDictionary.First(md => md.type == mt).clip;
        if(clip == null){
            Debug.LogError("NullMusicTypeException : cannot find AudioClip that matches given MusicType(" + mt.ToString() + ")");
            return;
        }
        musicPlayer.Play(clip);
    }
    public static void StopMusic(){
        musicPlayer.Stop();
    }

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
            spPool = new Queue<SoundPlayer>();
            var musicPlayerGO = Instantiate(instance.standardSoundPlayer, instance.gameObject.transform);
            musicPlayer = musicPlayerGO.GetComponent<SoundPlayer>();
            musicPlayer.SetMusicPlayer();
            DontDestroyOnLoad(instance);
        }
    }

    public static void PlayCustomerReaction(Enums.Gender gender, bool isSatisfied)
    {
        SoundType st;
        switch(gender){
            case Enums.Gender.Female:{
                if (isSatisfied) st = SoundType.FemaleSatisfy;
                else {
                   st = Random.value > 0.5f?  SoundType.FemaleDisappoint : SoundType.Raspberry;
                }  
                break;
            }
            case Enums.Gender.Male:{
                if (isSatisfied) st = SoundType.MaleSatisfy;
                else {
                   st = Random.value > 0.5f?  SoundType.MaleDissapoint : SoundType.Raspberry;
                }  
                break;
            }
            default:{
                st = SoundType.Raspberry;
                Debug.LogError("WrongGenderException : given gender isn't female nor male");
                break;
            }
        }
        Play(st);
    }
}