using UnityEngine;
using System.Collections;

public class Starfield : MonoBehaviour {
	private static string[] sprites = {"BlueStar", "RedStar", "YellowStar", "WhiteStar"};
	
	
	// Use this for initialization
	void Start () {
		for(int i = 0; i < 30; ++i) {
			OTAnimatingSprite star = (OTAnimatingSprite) OT.CreateSprite(sprites[Random.Range(0, sprites.Length)]);
			star.position = new Vector2(Random.Range(-500.0f, 502.0f), Random.Range(12.0f, 288.0f));
			star.depth = -1;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
