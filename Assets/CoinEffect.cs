using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class CoinEffect : MonoBehaviour {

	public GameObject coinPrefab;
	public float duration1;
	public float duration2;
	float maxWidth = 2;
	float minJumpPower = 2f;
	float maxJumpPower = 3.5f;
	public int particleNumber;

	// Use this for initialization
	void Start () {
		Vector3 destPos = GameObject.Find("CoinDest").transform.position;
		List<GameObject> particles = new List<GameObject>();
		for (int i = 0; i < particleNumber; i++) {
			Vector2 spread = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
			GameObject particle = Instantiate(coinPrefab, transform.position + (Vector3)spread, Quaternion.identity);
			float width = Random.Range(-maxWidth, maxWidth);
			float scale = Random.Range(0.8f, 1.2f);
			float yValue = Random.Range(0.8f, 1.3f);
			float jumpPower = Random.Range(minJumpPower, maxJumpPower);

			CoinMove cm = particle.GetComponent<CoinMove>();
			cm.width = width;
			cm.scale = scale;
			cm.yValue = yValue;
			cm.jumpPower = jumpPower;
			cm.destPos = destPos;
			
			particles.Add(particle);
		}

		particles.ForEach(coin => StartCoroutine(coin.GetComponent<CoinMove>().StartByEffector()));

		Destroy(gameObject, 1f);
	}
}
