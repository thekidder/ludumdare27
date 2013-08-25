using UnityEngine;
using System.Collections;

public class FadeController : MonoBehaviour {
	public Texture2D fadeTex;

	private Color fadeColor = new Color(0f, 0f, 0f, 0.0f);
	private int fadeDir = -1;
	private float fadeSpeed = 0;
	private float startTime = -1f;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > startTime) {
			fadeColor.a += fadeDir * fadeSpeed * Time.deltaTime;  
    		fadeColor.a = Mathf.Clamp01(fadeColor.a);
		}
	}
	
	void OnGUI() {
		GUI.color = fadeColor;
    	GUI.depth = -1000;
   	 	GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTex);
	}
	
	public void FadeOut(float start, float duration) {
		startTime = start;
		fadeColor.a = 0.0f;
		fadeDir = 1;
		fadeSpeed = 1f / duration;
	}
	
	public void FadeIn(float start, float duration) {
		startTime = start;
		fadeColor.a = 1.0f;
		fadeDir = -1;
		fadeSpeed = 1f / duration;
	}
}
