using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinMove : MonoBehaviour {

	float duration1 = 0.7f;
	float duration2 = 0.5f;
	float duration3;
	public float width;
	public float yValue;
	public float jumpPower;
	public float scale;
	public Vector3 destPos;

	// Use this for initialization
	public IEnumerator StartByEffector () {
		yield return new WaitForSeconds(Random.Range(0, 0.1f));

		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		transform.DOScale(scale, 0);
		// sr.DOFade(0.2f, 0);
		Vector3 endPos = transform.position + new Vector3(width, -yValue, 0);
		sr.DOFade(1, duration1 * 1f).SetEase(Ease.OutExpo);
		Tween tw = transform.DOJump(endPos, jumpPower, 1, duration1 + Random.Range(-0.05f, 0.05f)).SetEase(Ease.InOutSine);
		
		yield return tw.WaitForCompletion();

		transform.DOMove(destPos, duration2 + Random.Range(-0.05f, 0.05f)).SetEase(Ease.InCirc);
		float endScale = scale * 0.4f;
		transform.DOScale(endScale, duration2).SetEase(Ease.InCirc);	

		yield return null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
