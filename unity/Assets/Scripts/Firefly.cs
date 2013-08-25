using UnityEngine;
using System.Collections;

public class Firefly : MonoBehaviour {
	public OTAnimation visibleAnimation;
	public float visibleTime;
	public float speed;
	
	private enum FlyDirection {
		LEFT,
		RIGHT
	}
	
	private FlyDirection currentDirection = FlyDirection.RIGHT;
	
	private float visibilityChangeTime;
	private bool visible = true;
	
	private float perlinXOffset;
	private float perlinYOffset;
	
	// Use this for initialization
	void Start () {
		visibilityChangeTime = Time.time - Random.Range(0.0f, 10.0f);
		
		perlinXOffset = Random.Range(-500.0f, 500.0f);
		perlinYOffset = Random.Range(-500.0f, 500.0f);
		
		GetComponent<OTAnimatingSprite>().PlayLoop("right");
	}
	
	// Update is called once per frame
	void Update () {
		if(visible && Time.time - visibilityChangeTime > visibleTime) {
			GetComponent<OTAnimatingSprite>().alpha = 0.005f;
			visible = false;
		} else if(!visible && Time.time - visibilityChangeTime > 10.0f) {
			GetComponent<OTAnimatingSprite>().alpha = 1.0f;
			visibilityChangeTime = Time.time;
			visible = true;
		}
		
		float xDirection = Mathf.PerlinNoise(perlinXOffset + Time.time / 1.5f, 0.0f) - 0.48f;
		float yDirection = Mathf.PerlinNoise(0.0f, perlinYOffset + Time.time / 1.5f) - 0.47f;
				
		transform.Translate(speed * xDirection * Time.deltaTime, speed * yDirection * Time.deltaTime, 0.0f);
		
		if(xDirection > 0 && currentDirection != FlyDirection.RIGHT) {
			currentDirection = FlyDirection.RIGHT;
			GetComponent<OTAnimatingSprite>().PlayLoop("right");
		} else if(xDirection < 0 && currentDirection != FlyDirection.LEFT) {
			currentDirection = FlyDirection.LEFT;
			GetComponent<OTAnimatingSprite>().PlayLoop("left");
		}
	}
}
