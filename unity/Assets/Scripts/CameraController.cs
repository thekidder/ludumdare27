using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject sky;
	public float nightTime = 5f;
	
	public float cameraXPadding;
	public float cameraYPadding;
	
	public GameObject toFollow;
	
	// Use this for initialization
	void Start () {
		
		
		new OTTween(sky.GetComponent<OTGradientSprite>(), nightTime, OTEasing.Linear)
			.Tween("alpha", 0f);// new Color(237f/255f, 48f/255f, 60f/255f));
		
		//new OTTween(sky.GetComponent<OTGradientSprite>().gradientColors[1], nightTime, OTEasing.Linear)
		//	.Tween("color",  new Color(254f/255f, 252f/255f, 76f/255f));
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 position = toFollow.transform.position;
		
		float x = OT.view.position.x;
		float y = OT.view.position.y;
		
		if(x - position.x > cameraXPadding) {
			x = position.x + cameraXPadding;
		} else if(position.x - x > cameraXPadding) {
			x = position.x - cameraXPadding;
		}
		
		if(y - position.y > cameraYPadding) {
			y = position.y + cameraYPadding;
		} else if(position.y - y > cameraYPadding) {
			y = position.y - cameraYPadding;
		}
		
		OT.view.position = new Vector2(x, y);
	}
}
