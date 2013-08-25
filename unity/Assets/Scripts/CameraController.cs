using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public float cameraXPadding;
	public float cameraYPadding;

	
	public GameObject toFollow;
	
	// Use this for initialization
	void Start () {
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
