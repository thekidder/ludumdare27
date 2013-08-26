using UnityEngine;
using System.Collections;

public class OutroCutscene : Cutscene {
	public int num;
	
	private GUIStyle style;
	private OTAnimatingSprite firefly;
	
	// Use this for initialization
	void Start () {
		OT.view.position = new Vector2(0.0f, 900f);
		Camera.mainCamera.GetComponent<FadeController>().FadeIn(Time.time, 2f);
		
		Rect pos = new Rect(Screen.width/2 - 300f, Screen.height / 2 - 32f, 600f, 64f);
		
		Color color = new Color(1f, 1f, 1f);
		
		style = new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter;
		style.normal.textColor = color;
		
		allDialog.Add(new Dialog(pos, "Good work, son.", color, style, Time.time, 5.0f + Time.time));
		allDialog.Add(new Dialog(pos, "End.", color, style, Time.time + 5.0f, 10000000.0f + Time.time));
		
		firefly = (OTAnimatingSprite) OT.CreateSprite("Firefly");
		firefly.GetComponent<Firefly>().enabled = false;
		firefly.depth = -100;
		firefly.size = new Vector2(32f, 32f);
		firefly.pivot = OTObject.Pivot.Center;
		firefly.PlayLoop("right");
		
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 basePos = Camera.mainCamera.ScreenToWorldPoint(new Vector3(Screen.width/2f - 32f, Screen.height/2f - 32f, 0f));
		firefly.position = new Vector2(basePos.x, basePos.y);
	}
	
	void OnGUI() {
		doGUI();
				
		GUI.Label(new Rect(Screen.width/2f, Screen.height/2f + 24f, 24f, 24f), string.Format("x {0}", num), style);
	}
}
