using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MakeSuperfoodAnim : MonoBehaviour {

	public IEnumerator StartAnim (Vector3 startPos, Vector3 endPos) {
		yield return new WaitForSeconds(0.2f);
		Tween tw = transform.DOMove(endPos, 0.5f);
		yield return tw.WaitForCompletion();
		Destroy(gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
