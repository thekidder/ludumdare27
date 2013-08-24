using UnityEngine;
using System.Collections;

public class Boy : MonoBehaviour {
	
	public OTAnimation walkRight;
	public OTAnimation walkLeft; 
	
	private enum WalkDirection {
		LEFT,
		RIGHT
	}
	
	private WalkDirection currentWalkDirection = WalkDirection.RIGHT	;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveRequest = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0.0f); 
		WalkDirection direction = currentWalkDirection;

		if (moveRequest.sqrMagnitude > 0.0) {
            moveRequest.Normalize();
			
			if (moveRequest.x > 0) {
				direction = WalkDirection.RIGHT;
				if(currentWalkDirection != WalkDirection.RIGHT) {
					GetComponent<OTAnimatingSprite>().animation = walkRight;
				}
			} else {
				direction = WalkDirection.LEFT;
				if(currentWalkDirection != WalkDirection.LEFT) {
					GetComponent<OTAnimatingSprite>().animation = walkLeft;
				}
			}	
			
			if(!GetComponent<OTAnimatingSprite>().isPlaying) {
				GetComponent<OTAnimatingSprite>().Play();
			}
        } else {
			GetComponent<OTAnimatingSprite>().Pauze();
			GetComponent<OTAnimatingSprite>().frameIndex = 1;
			if (currentWalkDirection == WalkDirection.LEFT) {
				GetComponent<OTAnimatingSprite>().frameIndex += 4;
			}
		}
		
		this.transform.Translate(moveRequest * 2.0f);
		currentWalkDirection = direction;
	}
}
