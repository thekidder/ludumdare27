using UnityEngine;
using System.Collections;

public class Terrain : MonoBehaviour {
	public OTContainer terrainSheet;
	public OTContainer houseSprite;
		
	// Use this for initialization
	void Start () {
		GameObject world = this.transform.parent.gameObject;
		
		for(int i = -8; i <= 8; ++i) {
			GameObject chunk = createChunk(i, -2);
			chunk.transform.parent = world.transform;
		}
		
		OTSprite house = OT.CreateObject(OTObjectType.Sprite).GetComponent<OTSprite>();
		house.size = new Vector2(128, 64);
		house.pivot = OTObject.Pivot.Center;
		house.position = new Vector2(-4 * 64, -1 * 64 - 32);
		house.depth = -1;
		house.spriteContainer = houseSprite;
		house.transform.parent = world.transform;
				
		// make the world bob
		OTTween tweener = new OTTween(world.transform, 3.5f, OTEasing.SineInOut)
			.Tween("position", new Vector3(0f, 8f, 0f));
		
		tweener.pingPong = true;
		tweener.playCount = -1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private GameObject createChunk(int x, int y) {
		OTSprite top = createSprite(0, 0);
		top.frameIndex = Random.Range (0, 4);
		
		OTSprite middle = createSprite(0, -1);
		middle.frameIndex = Random.Range (4, 8);
		
		OTSprite bottom = createSprite(0, -2);
		bottom.frameIndex = Random.Range (8, 12);
		
		GameObject container = new GameObject();
		
		top.transform.parent = container.transform;
		middle.transform.parent = container.transform;
		bottom.transform.parent = container.transform;
		
		container.transform.Translate(new Vector3(x * 64, y * 64, 0));
		
		return container;
	}
	
	private OTSprite createSprite(int x, int y) {
		OTSprite sprite = OT.CreateObject(OTObjectType.Sprite).GetComponent<OTSprite>();
		sprite.size = new Vector2(64, 64);
		sprite.pivot = OTObject.Pivot.Center;
		sprite.position = new Vector2(x * 64, y * 64);
		sprite.depth = -1;
		sprite.spriteContainer = terrainSheet;
		
		return sprite;
	}
}
