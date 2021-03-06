using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boy : MonoBehaviour {
	public GUISkin boyDialogSkin;
	public Texture fireflyUITex;
	
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
	
	private class Dialog {
		public Dialog(string text, float duration) {
			this.text = text;
			this.start = Time.time;
			this.end = Time.time + duration;
		}
		
		public Dialog(string text, float start, float duration) {
			this.text = text;
			this.start = start;
			this.end = start + duration;
		}
		
		public string text;
		public float start;
		public float end;
	}
	
	private WalkDirection currentWalkDirection = WalkDirection.RIGHT;
	private Rect collidingBox    = new Rect(-20.0f, -32.0f, 36.0f, 64.0f);
	
	private bool falling = false;
	private bool isCurrentlySwinging = false;
	private float verticalSpeed = 0f;
	
	private List<Dialog> allDialog = new List<Dialog>();
	private Color textColor = new Color(0.89f, 0.933f, 0.933f);
	
	private OTSound landSound;
	private OTSound jumpSound;
	private OTSound pickupSound;
	
	private bool firstLine = false;
	private float fireflyTime = -1f;
	
	private List<OTAnimatingSprite> uiFireflies = new List<OTAnimatingSprite>();
	
	private Vector3 lastJumpPos;
	private int lastJumpCount;

	private float nightTimeEnd;
	private bool fadedOut = false;
	
	// Use this for initialization
	void Start () {
		GetComponent<OTAnimatingSprite>().PlayLoop("walkRight"); //ensure we have the right frameset queued
		GetComponent<OTAnimatingSprite>().Pauze();
		
		(landSound = new OTSound("land")).Idle();
		(jumpSound = new OTSound("jump")).Idle();
		(pickupSound = new OTSound("pickup")).Idle();

		
		allDialog.Add (new Dialog("The stars are dying! I have to save them!", Time.time + 2.5f, 2f));
		allDialog.Add (new Dialog("Maybe if I climb that hill I can reach them.", Time.time + 5f, 2f));
		
		nightTimeEnd = Camera.mainCamera.GetComponent<CameraController>().nightTime + Camera.mainCamera.GetComponent<CameraController>().start;
		Debug.Log ("night ending at " + nightTimeEnd + " currently " + Time.time);
		allDialog.Add (new Dialog("It's almost day time. I should get home.", nightTimeEnd - 4f, 2f));
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveRequest = new Vector3();
		
		moveRequest = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, 0.0f); 
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
				
			Rect net = new Rect(this.transform.position.x + netXCoord, this.transform.position.y - 30f, 40f, 60f);
			
			bool captured = fireflies.Captures(net);
			if(captured) {
				Debug.Log ("Got one!");
				pickupSound.Play();
				
				OTAnimatingSprite firefly = (OTAnimatingSprite) OT.CreateSprite("Firefly");
				firefly.GetComponent<Firefly>().enabled = false;
				firefly.depth = -100;
				firefly.size = new Vector2(64f, 64f);
				firefly.pivot = OTObject.Pivot.BottomLeft;
				firefly.PlayLoop("right");
				
				uiFireflies.Add (firefly);
				
				OnSwingDone(null);
			}
		}
		
		moveRequest *= Time.deltaTime * moveSpeed;
		
		bool wasFalling = falling;
		falling = terrain.CanMoveTo(this.transform.localPosition.x, this.transform.localPosition.y - 1.2f, collidingBox);
		
		if(!falling && !terrain.CanMoveTo(this.transform.localPosition.x + moveRequest.x * 10f, this.transform.localPosition.y - 1.2f, collidingBox)) {
			lastJumpPos = this.transform.localPosition;
			lastJumpCount = uiFireflies.Count;
		}
		

		moveRequest = terrain.Move(this.transform.localPosition, collidingBox, moveRequest);
		this.transform.Translate(moveRequest);
		
		
		if(wasFalling && !falling) {
			landSound.Play();
		}
		
		if(!falling && verticalSpeed < 0f) {
			verticalSpeed = 0f;
			
			if(Input.GetKeyDown(KeyCode.Space)) {
				jumpSound.Play();
				verticalSpeed = jumpSpeed;
			}
		}
		
		Fall();
		UpdateAnimation(moveRequest.sqrMagnitude > 0.0, direction);
		
		currentWalkDirection = direction;
		
		UpdateDialog();
		
		UpdateEvents();
		
		UpdateUISprites();
		
		if(this.transform.localPosition.y < 100) {
			this.transform.localPosition = lastJumpPos;
			while(uiFireflies.Count > lastJumpCount) {
				uiFireflies.RemoveAt(0);
			}
		}

	}
	
	void OnGUI() {
		GUI.skin = boyDialogSkin;

		Vector3 boyScreenPos = Camera.mainCamera.WorldToScreenPoint(this.transform.position);
		foreach(Dialog d in allDialog) {
			if(Time.time < d.start) {
				continue;
			}
			
			float x;
			if(boyScreenPos.x > 512f) {
				GUI.skin.label.alignment = TextAnchor.MiddleRight;
				x = boyScreenPos.x - 48f - 416f;
			} else {
				GUI.skin.label.alignment = TextAnchor.MiddleLeft;
				x = boyScreenPos.x + 48f;
			}
			
			GUI.skin.label.normal.textColor = textColor;
			GUI.Label(new Rect(x, Screen.height - boyScreenPos.y  - 96f, 416f, 64f), d.text);
			break;
		}
	}
	
	private void OnSwingDone(OTObject owner)
	{
		if(!isCurrentlySwinging) {
			return;
		}
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
	
	private void UpdateDialog() {
		foreach(Dialog d in allDialog) {
			if(Time.time > d.end) {
				allDialog.Remove (d);
				break;
			}
		}
	}
	
	private void UpdateEvents() {
		if(this.transform.localPosition.x > 1016f && this.transform.localPosition.x < 1112 && !firstLine) {
			allDialog.Add (new Dialog("I'm still so far away...", 2f));
			allDialog.Add (new Dialog("Wait - what was that?", Time.time + 3.5f, 2f));
			firstLine = true;
			
			fireflyTime = Time.time + 2.0f;
		}
		
		if(Time.time > fireflyTime && fireflyTime != -1) {
			fireflies.Add(31 * 32, 18 * 32, true);
			fireflyTime = -1;
			
			fireflies.StartSpawning();
		}
		
		if(Time.time > nightTimeEnd && !fadedOut) {
			Camera.mainCamera.GetComponent<FadeController>().FadeOut(Time.time, 2f);
			fadedOut = true;
		}
		
		if(Time.time > nightTimeEnd + 2f) {
			Camera.mainCamera.GetComponent<CameraController>().enabled = false;
			Camera.mainCamera.GetComponent<OutroCutscene>().enabled = true;
			Camera.mainCamera.GetComponent<OutroCutscene>().num = uiFireflies.Count;
			this.enabled = false;
		}
	}
	
	private void UpdateUISprites() {
		Vector3 basePos = Camera.mainCamera.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));		
		const float padding = 16f;
		
		for(int i = 0; i < uiFireflies.Count; ++i) {	
			uiFireflies[i].position = new Vector2(basePos.x + padding + i * (64f + padding), basePos.y + padding);
		}
	}
}
