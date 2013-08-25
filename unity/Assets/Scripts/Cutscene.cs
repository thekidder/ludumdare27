using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cutscene : MonoBehaviour {
	public GUISkin skin;

	protected class Dialog {
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
	
	protected List<Dialog> allDialog = new List<Dialog>();
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	protected void doGUI() {
		GUI.skin = skin;
		
		foreach(Dialog dialog in allDialog) {
			if(Time.time > dialog.start && Time.time < dialog.end) {
				GUI.color = dialog.color;
				GUI.Label(dialog.pos, dialog.text, dialog.style);
			}	
		}
	}
}
