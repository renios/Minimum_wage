using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HighlightBorder : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	public Material highlightBorderMaterial;
	Material defaultMaterial;

	readonly float delay = 1.0f;

	public void ActiveBorder() {
		spriteRenderer.material = highlightBorderMaterial;
		StartCoroutine(ChangeColor());
	}

	public void InactiveBorder() {
		StopAllCoroutines();
		spriteRenderer.material = defaultMaterial;
	}

	IEnumerator ChangeColor() {
		Tween tw;
		while (true) {
			spriteRenderer.material.color = Color.cyan;
			tw = spriteRenderer.material.DOColor(Color.yellow, delay).SetEase(Ease.Linear);
			yield return tw.WaitForCompletion();
			tw = spriteRenderer.material.DOColor(Color.magenta, delay).SetEase(Ease.Linear);
			yield return tw.WaitForCompletion();
			tw = spriteRenderer.material.DOColor(Color.cyan, delay).SetEase(Ease.Linear);
			yield return tw.WaitForCompletion();
		}
	}

	// Use this for initialization
	void Start () {
		defaultMaterial = spriteRenderer.material;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}