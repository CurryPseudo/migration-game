using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {
	public SpriteRenderer black;
	public float blackAlpha {
		get {
			return black.color.a;
		}
		set {
			Color c = black.color;
			c.a = value;
			black.color = c;
		}
	}
	public float fadeTime = 2;
	public IEnumerator Main() {
		{
			blackAlpha = 1;
			float timeCount = 0;	
			while(timeCount < fadeTime) {
				yield return null;
				timeCount += Time.deltaTime;
				blackAlpha = Mathf.Lerp(1, 0, timeCount / fadeTime);
			}
		}
		yield return new WaitUntil(() => Input.GetAxis("Button_A") > 0.2f || Input.GetKeyDown(KeyCode.J));
		{
			blackAlpha = 0;
			float timeCount = 0;	
			while(timeCount < fadeTime) {
				yield return null;
				timeCount += Time.deltaTime;
				blackAlpha = Mathf.Lerp(0, 1, timeCount / fadeTime);
			}
		}
		SceneManager.LoadScene(1);

	}
}
