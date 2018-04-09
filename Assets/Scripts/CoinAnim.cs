using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinAnim : MonoBehaviour {

	public IEnumerator StartAnim (float delay) {
		Vector3 destPos = GameObject.Find("CoinDest").transform.position;
		Vector3 originPos = transform.position;
		// yield return new WaitForSeconds(delay*0.2f);
		Tween tw = transform.DOJump(originPos, 3, 1, delay*0.5f);
		yield return tw.WaitForCompletion();
		tw = transform.DOMove(destPos, delay*0.5f);
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
