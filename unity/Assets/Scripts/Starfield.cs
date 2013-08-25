using UnityEngine;
using System.Collections;

public class Starfield : MonoBehaviour {
	public int numStars;
	
	private static string[] sprites = {"BlueStar", "RedStar", "YellowStar", "WhiteStar"};

	// Use this for initialization
	void Start () {
		for(int i = 0; i < numStars; ++i) {
			OTAnimatingSprite star = (OTAnimatingSprite) OT.CreateSprite(sprites[Random.Range(0, sprites.Length)]);
			Vector3 scale = this.transform.localScale;
			Vector3 pos = this.transform.position;
			star.position = new Vector2(Random.Range(pos.x - scale.x, pos.x + scale.x), Random.Range(pos.y - scale.y, pos.y + scale.y));
			star.depth = -1;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
