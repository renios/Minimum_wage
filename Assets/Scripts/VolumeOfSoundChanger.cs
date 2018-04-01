using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeOfSoundChanger : MonoBehaviour {

	// SoundManager가 DontDestroyOnLoad에 올라가 있어,
	// Ingame 씬의 인스펙터 창에서 SoundManager를 지정해줘도
	// World 씬에서 넘어오면 지정해준 SoundManager가 사라져서
	// 이를 방지하는 코드
	public void SetSound(bool value) {
		SoundManager.SetVolumeOfSoundPlayer(value);
	}
	public void SetMusic (bool value) {
		SoundManager.SetVolumeOfMusicPlayer(value);
	}
}
