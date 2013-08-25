using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntroCutscene : Cutscene {
	public bool showCutscene = true;
	
	public Boy boy;
	
	public GUISkin titleSkin;
	
	public float cameraPos = 900f;
	
		
	// Use this for initialization
	void Start () {		
		OT.view.position = new Vector2(0.0f, 900f);
		
		new OTTween(this, 6.5f, OTEasing.CubicInOut)
			.Wait (20f)
			.Tween("cameraPos",  900f, 0f);
		
		Rect kidPos = new Rect(60f, Screen.height / 2 - 60f, 600f, 64f);
		Rect dadPos = new Rect(Screen.width - 60f - 600f, Screen.height / 2 + 60f, 600f, 64f);
		
		Color kidColor = new Color(0.89f, 0.933f, 0.933f);
		Color dadColor = Color.white;
		
		GUIStyle kidStyle = new GUIStyle();
		kidStyle.alignment = TextAnchor.MiddleLeft;
		kidStyle.normal.textColor = kidColor;
		
		GUIStyle dadStyle = new GUIStyle();
		dadStyle.alignment = TextAnchor.MiddleRight;
		dadStyle.normal.textColor = dadColor;

		allDialog.Add(new Dialog(kidPos, "Look at all the stars, dad.", kidColor, kidStyle, 4.0f, 7.0f));
		allDialog.Add(new Dialog(dadPos, "Yeah.", dadColor, dadStyle, 7.5f, 10.5f));
		allDialog.Add(new Dialog(kidPos, "There are so many of them!", kidColor, kidStyle, 11.0f, 14.0f));
		allDialog.Add(new Dialog(dadPos, "But the universe is so big. A star dies every ten seconds.", dadColor, dadStyle, 14.5f, 19.5f));
		
		if(showCutscene) {
			Camera.mainCamera.GetComponent<FadeController>().FadeIn(2.5f, 2f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		OT.view.position = new Vector2(0f, cameraPos);
		
		if(Time.time > 22f || !showCutscene) {
			boy.gameObject.SetActive(true);
		}
		
		if(Time.time > 27f || !showCutscene) {
			GetComponent<CameraController>().enabled = true;
			this.enabled = false;
		}
	}
	
	void OnGUI() {
		if(Time.time < 3.5) {
			GUI.skin = titleSkin;			
			GUI.color = new Color(1f, 1f, 1f, 3.5f - Time.time);
			GUI.depth = -1005;
			
			GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Firefly");
		}
		doGUI();
	}
}
