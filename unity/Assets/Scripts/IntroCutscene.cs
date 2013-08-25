using UnityEngine;
using System.Collections;

public class IntroCutscene : MonoBehaviour {
	public GUISkin skin;
	public GUISkin titleSkin;
	
	private Texture2D dummyTex;
	private Texture2D fadeTex;
	
	private GUIStyle backgroundStyle = new GUIStyle();
	
	public float cameraPos = 900f;
		
	// Use this for initialization
	void Start () {		
		OT.view.position = new Vector2(0.0f, 900f);
		
		dummyTex = new Texture2D(1,1);
        dummyTex.SetPixel(0,0,Color.black);
		
		fadeTex = new Texture2D(1,1);
		fadeTex.SetPixel(0,0,Color.black);
		
		backgroundStyle.normal.background = fadeTex;
		
		new OTTween(this, 6.5f, OTEasing.CubicInOut)
			.Wait (3.5f)
			.Tween("cameraPos",  900f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		OT.view.position = new Vector2(0f, cameraPos);
	}

	
	void OnGUI() {
		if(Time.time < 3.5) {
			GUI.skin = titleSkin;
			GUIStyle textStyle = new GUIStyle();
			
			GUI.color = new Color(1f, 1f, 1f, 3.5f - Time.time);
			
			GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Firefly");
			
			//GUI.color = new Color(0f, 0f, 0f, Mathf.Clamp(3.5f - Time.time, 0f, 1f));
			//GUI.Label(new Rect(-10, -10, Screen.width + 10, Screen.height + 10), dummyTex, backgroundStyle);
		}
		
		GUI.skin = skin;
	}
}
