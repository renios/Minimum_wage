using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeOfSoundChanger : MonoBehaviour {

	// SoundManager가 DontDestroyOnLoad에 올라가 있어,
	// Ingame 씬의 인스펙터 창에서 SoundManager를 지정해줘도
	// World 씬에서 넘어오면 지정해준 SoundManager가 사라져서
	// 이를 방지하는 코드

	public bool isForMusic;
	Toggle toggle;

	void Start(){
		toggle = GetComponent<Toggle>();
		toggle.isOn = InitialValue();
	}
	public void SetVolume(bool value) {
		SoundManager.Play(SoundType.Button);
		if (isForMusic) SoundManager.SetVolumeOfMusicPlayer(value);
		else SoundManager.SetVolumeOfSoundPlayer(value);
	}

	bool InitialValue(){
		if(isForMusic){
			if (SoundPlayer.musicVolume > 0.5f) return true;
			else return false;
		} else {
			if (SoundPlayer.soundVolume > 0.5f) return true;
			else return false;
		}
	}
}
