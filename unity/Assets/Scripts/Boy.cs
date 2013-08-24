using UnityEngine;
using System.Collections;

public class Boy : MonoBehaviour {
	
	private enum WalkDirection {
		LEFT,
		STANDING,
		RIGHT
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveRequest = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0.0f); 
		WalkDirection direction = WalkDirection.STANDING;
		
		if (moveRequest.sqrMagnitude > 0.0) {
            moveRequest.Normalize();
			
			if (moveRequest.x > 0) {
				direction = WalkDirection.RIGHT;
			} else {
				direction = WalkDirection.LEFT;
			}	
        }
		
		this.transform.Translate(moveRequest * 2.0f);
	}
}
