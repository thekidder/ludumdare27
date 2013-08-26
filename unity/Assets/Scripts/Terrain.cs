using UnityEngine;
using System;
using System.Collections;
using SimpleJSON;

public class Terrain : MonoBehaviour {
	
	private const uint FLIPPED_HORIZONTAL = 1u << 31;
	private const uint FLIPPED_VERTICAL   = 1u << 30;
	private const uint FLIPPED_DIAGONAL   = 1u << 29;

	private const uint FLIP_MASK = (FLIPPED_HORIZONTAL | FLIPPED_VERTICAL | FLIPPED_DIAGONAL);
	
	private struct Tileset {
		public Tileset(OTContainer s, uint g, string n) {
			spritesheet = s;
			firstgid = g;
			name = n;
		}
		
		public OTContainer spritesheet;
		public uint firstgid;
		public string name;
	}

	public Fireflies fireflies;
	
	public TextAsset levelFile;
	public OTContainer terrainSheet;
	public OTContainer objectsSheet;

	private CollisionDetector collisionDetector;
		
	// Use this for initialization
	void Start () {
		GameObject world = this.transform.parent.gameObject;
		
		var level = JSON.Parse(levelFile.text);
		
		int levelHeight = level["height"].AsInt;
		int levelWidth  = level["width"].AsInt;
		
		collisionDetector = new CollisionDetector(levelWidth, levelHeight);
		
		Tileset[] tilesets = new Tileset[level["tilesets"].AsArray.Count];
		for(int i = 0; i < level["tilesets"].AsArray.Count; ++i) {
			JSONNode tileset = level["tilesets"].AsArray[i];
			string name = tileset["name"];
			
			if(name == "terrain") {
				tilesets[i] = new Tileset(terrainSheet, tileset["firstgid"].AsUInt, "terrain");
			} else if(name == "objects") {
				tilesets[i] = new Tileset(objectsSheet, tileset["firstgid"].AsUInt, "objects");
			} else if(name == "collision") {
				tilesets[i] = new Tileset(null, tileset["firstgid"].AsUInt, "collision");
			} else {
				Debug.Log ("tileset name: '" + tileset["name"] + "'");
				tilesets[i] = new Tileset(null, tileset["firstgid"].AsUInt, "NO SHEET");
			}
		}
		
		Array.Reverse(tilesets);
		
		int depth = -1;
		foreach(JSONNode layer in level["layers"].AsArray) {
			for(int i = 0; i < layer["data"].AsArray.Count; ++i) {
				uint tileIndex = layer["data"].AsArray[i].AsUInt;
				
				if (tileIndex == 0) {
					continue;
				}
				
				int x = i % levelWidth;
				int y = i / levelWidth;
				y = levelHeight - y - 1;
				
				uint flipped = tileIndex & FLIP_MASK;
				tileIndex = tileIndex & ~FLIP_MASK;
				
				OTContainer spriteContainer = null;
				foreach(Tileset t in tilesets) {
					if(tileIndex >= t.firstgid) {
						tileIndex -= t.firstgid;
						
						if(t.name == "collision") {
							collisionDetector.SetCollidable(x, y);
						}
						
						spriteContainer = t.spritesheet;
						break;
					}
				}
				
				if(!layer["visible"].AsBool) {
					continue;
				}
				
				if(tileIndex == 4 && spriteContainer == objectsSheet) {
					// firefly, add to fireflies manager instead
					fireflies.SpawnAt(x * 32, y * 32);
					continue;
				}
				
				bool flip_x = (flipped & FLIPPED_HORIZONTAL) > 0;
                bool flip_y = (flipped & FLIPPED_VERTICAL) > 0;
				
				float rotation = 0f;
				
                if((flipped & FLIPPED_DIAGONAL) > 0) {
                    if((flipped & FLIPPED_HORIZONTAL) > 0 && (flipped & FLIPPED_VERTICAL) == 0) {
                        rotation = 90f;
                        flip_x = true;
					} else if((flipped & FLIPPED_HORIZONTAL) == 0 && (flipped & FLIPPED_VERTICAL) > 0) {
                        rotation = -90f;
                        flip_y = true;
					} else if((flipped & FLIPPED_HORIZONTAL) == 0 && (flipped & FLIPPED_VERTICAL) == 0) {
                        rotation = -90f;
                        flip_x = true;
					} else {
                        rotation = 90f;
                        //flip_y = false;
						flip_x = true;
					}
				}

				OTSprite tile = createSprite(x, y, world); // random offsets that puts terrain where I want it
				tile.spriteContainer = spriteContainer;
				tile.depth = depth;
				tile.rotation = rotation;
				tile.flipHorizontal = flip_x;
				tile.flipVertical = flip_y;
				tile.frameIndex = (int)tileIndex;
			}
			
			depth -= 1;
		}
				
		// make the world bob
		Vector3 bobPos = world.transform.position + new Vector3(0f, 8f, 0f);
		OTTween tweener = new OTTween(world.transform, 3.5f, OTEasing.SineInOut)
			.Tween("position", bobPos);
		
		tweener.pingPong = true;
		tweener.playCount = -1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public Vector2 Move(Vector3 position, Rect collisionBox, Vector2 moveRequest) {
		Rect collider = new Rect(collisionBox.x + position.x, collisionBox.y + position.y, collisionBox.width, collisionBox.height);
		return collisionDetector.Move(collider, moveRequest);// * 0.99f;
	}
	
	public bool CanMoveTo(float x, float y, Rect collisionBox) {
		Rect collider = new Rect(collisionBox.x + x, collisionBox.y + y, collisionBox.width, collisionBox.height);
		return !collisionDetector.Collides(collider);
	}
	
	private OTSprite createSprite(int x, int y, GameObject world) {
		OTSprite sprite = OT.CreateObject(OTObjectType.Sprite).GetComponent<OTSprite>();
		sprite.size = new Vector2(32, 32);
		sprite.pivot = OTObject.Pivot.Center;
		sprite.transform.parent = world.transform;
		sprite.position = new Vector2(x * 32 + 16, y * 32 + 16);
		
		return sprite;
	}
}
