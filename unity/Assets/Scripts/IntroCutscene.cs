using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntroCutscene : MonoBehaviour {
	
	public Boy boy;
	
	public GUISkin skin;
	public GUISkin titleSkin;
	
	private Texture2D dummyTex;
	private Texture2D fadeTex;
	
	private GUIStyle backgroundStyle = new GUIStyle();
	
	public float cameraPos = 900f;
	
	private class Dialog {
		public Dialog(Rect pos, string text, Color color, GUIStyle style, float start, float end) {
			this.pos = pos;
			this.text = text;
			this.color = color;
			this.style = style;
			this.start = start;
			this.end = end;
		}
		
		public Rect pos;
		public string text;
		public Color color;
		public GUIStyle style;
		public float start;
		public float end;
	}
	
	private List<Dialog> allDialog = new List<Dialog>();
		
	// Use this for initialization
	void Start () {		
		OT.view.position = new Vector2(0.0f, 900f);
		
		dummyTex = new Texture2D(1,1);
        dummyTex.SetPixel(0,0,Color.black);
		
		fadeTex = new Texture2D(1,1);
		fadeTex.SetPixel(0,0,Color.black);
		
		backgroundStyle.normal.background = fadeTex;
		
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
		

		allDialog.Add(new Dialog(kidPos, "Look at all the stars, dad", kidColor, kidStyle, 4.0f, 7.0f));
		allDialog.Add(new Dialog(dadPos, "Yeah.", dadColor, dadStyle, 7.5f, 10.5f));
		allDialog.Add(new Dialog(kidPos, "There are so many of them", kidColor, kidStyle, 11.0f, 14.0f));
		allDialog.Add(new Dialog(dadPos, "But the universe is so big. A star dies every ten seconds.", dadColor, dadStyle, 14.5f, 19.5f));
	}
	
	// Update is called once per frame
	void Update () {
		OT.view.position = new Vector2(0f, cameraPos);
		
		if(Time.time > 24f) {
			boy.gameObject.SetActive(true);
		}
		
		if(Time.time > 27f) {
			GetComponent<CameraController>().enabled = true;
			this.enabled = false;
		}
	}

	
	void OnGUI() {
		if(Time.time < 3.5) {
			GUI.skin = titleSkin;			
			GUI.color = new Color(1f, 1f, 1f, 3.5f - Time.time);
			
			GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Firefly");
			
			//GUI.color = new Color(0f, 0f, 0f, Mathf.Clamp(3.5f - Time.time, 0f, 1f));
			//GUI.Label(new Rect(-10, -10, Screen.width + 10, Screen.height + 10), dummyTex, backgroundStyle);
		}
		
		GUI.skin = skin;
		
		foreach(Dialog dialog in allDialog) {
			if(Time.time > dialog.start && Time.time < dialog.end) {
				GUI.color = dialog.color;
				GUI.Label(dialog.pos, dialog.text, dialog.style);
			}	
		}
	}
}
