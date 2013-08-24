using UnityEngine;
using System.Collections;

public class Boy : MonoBehaviour {
	
	public OTAnimation walkRight;
	public OTAnimation walkLeft;
	public float moveSpeed;
	public float gravity;
	public float terminalVelocity;
	
	public Terrain terrain;
	
	private enum WalkDirection {
		LEFT,
		RIGHT
	}
	
	private WalkDirection currentWalkDirection = WalkDirection.RIGHT;
	private Rect collidingBox = new Rect(-20.0f, -32.0f, 36.0f, 64.0f);
	
	private bool falling = false;
	private float fallSpeed = 0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveRequest = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, 0.0f); 
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
		
		moveRequest *= Time.deltaTime * moveSpeed;
		moveRequest = terrain.Move(this.transform.localPosition, collidingBox, moveRequest);
		//if(terrain.CanMoveTo(this.transform.localPosition.x + moveRequest.x, this.transform.localPosition.y + moveRequest.y, collider)) {
			this.transform.Translate(moveRequest);
		//}
		if(terrain.CanMoveTo(this.transform.localPosition.x, this.transform.localPosition.y + 1.0f, collidingBox)) {
			Fall();
		}
		
		currentWalkDirection = direction;
	}
	
	private void Fall() {
		if(!falling) {
			fallSpeed = 0.0f;
		}
		
		fallSpeed += gravity;
		fallSpeed = Mathf.Max(fallSpeed, terminalVelocity);
		
		this.transform.Translate(terrain.Move (this.transform.localPosition, collidingBox, new Vector2(0f, fallSpeed * Time.deltaTime)));
		
		//this.transform.Translate(0.0f, fallSpeed * Time.deltaTime, 0.0f);
		
		falling = true;
	}
}
