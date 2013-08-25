using UnityEngine;
using System.Collections;

public class Boy : MonoBehaviour {
	public float moveSpeed;
	public float gravity;
	public float terminalVelocity;
	public float jumpSpeed;
	
	public Terrain terrain;
	public Fireflies fireflies;
	
	private enum WalkDirection {
		LEFT,
		RIGHT
	}
	
	private WalkDirection currentWalkDirection = WalkDirection.RIGHT;
	private Rect collidingBox    = new Rect(-20.0f, -32.0f, 36.0f, 64.0f);
	
	private bool falling = false;
	private bool isCurrentlySwinging = false;
	private float verticalSpeed = 0f;
	
	// Use this for initialization
	void Start () {
		GetComponent<OTAnimatingSprite>().PlayLoop("walkRight"); //ensure we have the right frameset queued
		GetComponent<OTAnimatingSprite>().Pauze();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveRequest = new Vector3();
		
		if(!isCurrentlySwinging) {
			moveRequest = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, 0.0f); 
		}
		WalkDirection direction = currentWalkDirection;

		if (moveRequest.sqrMagnitude > 0.0) {
			moveRequest.Normalize();
			
			if (moveRequest.x > 0) {
				direction = WalkDirection.RIGHT;
			} else {
				direction = WalkDirection.LEFT;
			}	
		}
		
		if(Input.GetKeyDown(KeyCode.Z) && !isCurrentlySwinging) {
			isCurrentlySwinging = true;
			
			OTAnimatingSprite sprite = GetComponent<OTAnimatingSprite>();
			if(direction == WalkDirection.LEFT) {
				sprite.PlayOnce("catchLeft");
			} else {
				sprite.PlayOnce("catchRight");
			}
			sprite.Play();
			sprite.onAnimationFinish = OnSwingDone;
		}
		
		if(isCurrentlySwinging) {
			float netXCoord;
			if(direction == WalkDirection.LEFT) {
				netXCoord = -64.0f;
			} else {
				netXCoord = 24.0f;
			}
				
			Rect net = new Rect(this.transform.localPosition.x + netXCoord, this.transform.localPosition.y - 30f, 40f, 60f);
			
			bool captured = fireflies.Captures(net);
			if(captured) {
				Debug.Log ("Got one!");
			}
		}
		
		moveRequest *= Time.deltaTime * moveSpeed;
		moveRequest = terrain.Move(this.transform.localPosition, collidingBox, moveRequest);
		this.transform.Translate(moveRequest);
		
		falling = terrain.CanMoveTo(this.transform.localPosition.x, this.transform.localPosition.y - 1.2f, collidingBox);
		
		if(!falling && verticalSpeed < 0f) {
			verticalSpeed = 0f;
			
			if(Input.GetKeyDown(KeyCode.Space)) {
				verticalSpeed = jumpSpeed;
			}
		}
		
		Fall();
		UpdateAnimation(moveRequest.sqrMagnitude > 0.0, direction);
		
		currentWalkDirection = direction;

	}
	
	private void OnSwingDone(OTObject owner)
	{
		isCurrentlySwinging = false;
		
		OTAnimatingSprite sprite = GetComponent<OTAnimatingSprite>();
		if(currentWalkDirection == WalkDirection.LEFT) {
			sprite.PlayLoop("walkLeft");
		} else {
			sprite.PlayLoop("walkRight");
		}
		sprite.onAnimationFinish = null;
	}
	
	private void UpdateAnimation(bool moving, WalkDirection direction) {
		OTAnimatingSprite sprite = GetComponent<OTAnimatingSprite>();

		if(isCurrentlySwinging) {
			return;
		}
				
		if(moving && currentWalkDirection != direction) {
			if(direction == WalkDirection.LEFT) {
				sprite.PlayLoop("walkLeft");
			} else {
				sprite.PlayLoop("walkRight");
			}
		}
		
		if(moving) {
			if(!sprite.isPlaying) {
				sprite.Play();
			}
		} else {
			sprite.Pauze();
			sprite.frameIndex = 1;
			if (direction == WalkDirection.LEFT) {
				sprite.frameIndex += 4;
			}
		}
		
		if(falling) {
			sprite.Pauze();
			sprite.frameIndex = 0;
			if (direction == WalkDirection.LEFT) {
				sprite.frameIndex += 4;
			}
		}
	}
	
	private void Fall() {
		verticalSpeed += gravity;
		verticalSpeed = Mathf.Max(verticalSpeed, terminalVelocity);
				
		this.transform.Translate(terrain.Move (this.transform.localPosition, collidingBox, new Vector2(0f, verticalSpeed * Time.deltaTime)));		
	}
}
