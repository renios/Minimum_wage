 using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SoundType { Button, Cashier, Coin, Combo, FemaleDisappoint, FemaleSatisfy, MaleSatisfy, MaleDisappoint, Raspberry, Tap }
public enum MusicType { Ambient, Main, Start, GameOver }

[System.Serializable]
public class SoundDic{
    public SoundType type;
    public AudioClip clip;
}
[System.Serializable]
public class WorldSoundData{
    public AudioClip start;
    public AudioClip ambient;
    public AudioClip gameOver;
    public AudioClip[] combo;
}
public class SoundManager : MonoBehaviour{
    static SoundManager instance;
    static Queue<SoundPlayer> spPool;
    static SoundPlayer musicPlayer;
    static int worldIndex;
    static int comboCount;
    
    public GameObject standardSoundPlayer;
    public SoundDic[] soundDictionary;
    public WorldSoundData[] worldSoundData;
    public AudioClip defaultMusic;
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
        var clip = ChooseSound(st);
        if (clip == null){
            Debug.LogError("NullSoundTypeException : cannot find AudioClip that matches given SoundType(" + st.ToString() + ")");
            return;
        }
        PullNewSoundPlayer().Play(clip, pos);
    }
    static AudioClip ChooseSound(SoundType st){
        AudioClip clip;
        switch (st){
            case SoundType.Combo:{
                var selectedCombo = instance.worldSoundData[worldIndex].combo;
                if (selectedCombo.Length == 0) return null;
                clip = (comboCount - 1 < selectedCombo.Length)? selectedCombo[comboCount - 1] : selectedCombo.Last();
                break;
            }
            default:{
                var selectedDics = instance.soundDictionary.Where(sd => sd.type == st).ToArray();
                if(selectedDics.Length == 0) return null;
                clip = selectedDics[Random.Range(0,selectedDics.Length)].clip;
                break;
            }
        }
        return clip;
    }
    public static void Play(MusicType mt){
        AudioClip clip;
        bool isLoop = false;
        var selectedWorld = instance.worldSoundData[worldIndex];
        switch(mt){
            case MusicType.Ambient:{
                clip = selectedWorld.ambient;
                isLoop = true;
                break;
            }
            case MusicType.Start:{
                clip = selectedWorld.start;
                isLoop = false;
                break;
            }
            default:{
                clip = instance.defaultMusic;
                isLoop = true;
                break;
            }
        }
        if(clip == null){
            Debug.LogError("NullMusicTypeException : cannot find AudioClip that matches given MusicType(" + mt.ToString() + ")");
            return;
        }
        musicPlayer.Play(clip, instance.transform.position, isLoop);
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

    public static void SetWorldIndex(int inputIndex){
        worldIndex = inputIndex;
        Debug.Log("SoundManager.SetWorldIndex : " + worldIndex);
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
                   st = Random.value > 0.5f?  SoundType.MaleDisappoint : SoundType.Raspberry;
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
    public static void PlayCombo(int combo){
        comboCount = combo;
        Play(SoundType.Combo);
    }
}