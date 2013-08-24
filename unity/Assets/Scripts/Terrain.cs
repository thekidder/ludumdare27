using UnityEngine;
using System.Collections;

public class Terrain : MonoBehaviour {
	public OTContainer terrainSheet;
	
	private GameObject terrain;
	
	// Use this for initialization
	void Start () {
		terrain = new GameObject();
		
		for(int i = -8; i <= 8; ++i) {
			GameObject chunk = createChunk(i, -2);
			chunk.transform.parent = terrain.transform;
		}
				
		OTTween tweener = new OTTween(terrain.transform, 3.5f, OTEasing.SineInOut)
			.Tween("position", new Vector3(0f, 8f, 0f));
		
		//tweener.playLoop = true;
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
