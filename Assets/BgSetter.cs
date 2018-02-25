using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgSetter : MonoBehaviour {

	public SpriteRenderer spriteRender;

	public Sprite world1;
	public Sprite world2;

	// Use this for initialization
	void Start () {
		Dictionary<MissionDataType, int> missionDataDict = MissionData.GetMissionDataDict();
		int stageIndex = missionDataDict[MissionDataType.StageIndex];

		if (stageIndex < 11) {
			spriteRender.sprite = world1;
		}
		else if (stageIndex < 21) {
			spriteRender.sprite = world2;
		}
		else {
			spriteRender.sprite = world1;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
