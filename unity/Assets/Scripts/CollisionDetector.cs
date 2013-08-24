using System.Collections;
using UnityEngine;

public class CollisionDetector {
	private const int TILE_SIZE = 32;
	
	private bool[] tiles;
	private int width;
	private int height;
	
	private class Point {
		public Point(int i, int j) {
			x = i;
			y = j;
		}
		
		public int x;
		public int y;
	}
		
	public CollisionDetector(int w, int h) {
		width = w;
		height = h;
		tiles = new bool[width * height];
	}
	
	public void SetCollidable(int x, int y) {
		tiles[x + y * width] = true;
	}

	private Point Collides(float x, float y) {
		int intX = (int)(x + 0.5);
		int intY = (int)(y + 0.5);
		
		if(tiles[intX / TILE_SIZE + (intY / TILE_SIZE) * width]) {
			return new Point(intX / TILE_SIZE, intY / TILE_SIZE);
		}
		return null;
	}
	
	public bool Collides(Rect collider) {
		return Collides (collider.xMin, collider.yMin) != null
			|| Collides (collider.xMin, collider.yMax) != null
			|| Collides (collider.xMax, collider.yMax) != null
			|| Collides (collider.xMax, collider.yMin) != null;
	}
	
	public Vector2 Move(Rect collider, Vector2 moveVector) {
		Point bottomLeft  = Collides (collider.xMin + moveVector.x, collider.yMin + moveVector.y);
		Point topLeft     = Collides (collider.xMin + moveVector.x, collider.yMax + moveVector.y);
		Point topRight    = Collides (collider.xMax + moveVector.x, collider.yMax + moveVector.y);
		Point bottomRight = Collides (collider.xMax + moveVector.x, collider.yMin + moveVector.y);
		
		if(bottomLeft == null && bottomRight == null && topLeft == null && topRight == null) {
			return moveVector;
		}
		
		bool left   = bottomLeft  != null && topLeft     != null;
		bool right  = bottomRight != null && topRight    != null;
		bool top    = topLeft     != null && topRight    != null;
		bool bottom = bottomLeft  != null && bottomRight != null;
		
		if(!left && !right && !top && !bottom) {
			// single collision
			if(bottomLeft != null) {
				float x = (bottomLeft.x + 1) * TILE_SIZE - (collider.xMin + moveVector.x);
				float y = (bottomLeft.y + 1) * TILE_SIZE - (collider.yMin + moveVector.y);
				
				if(x > y) {
					return new Vector2( ((bottomLeft.x + 1) * TILE_SIZE) - collider.xMin, moveVector.y);
				} else {
					return new Vector2(moveVector.x, ((bottomLeft.y + 1) * TILE_SIZE) - collider.yMin);
				}
			}
			
			if(bottomRight != null) {
				float x = (collider.xMax + moveVector.x) - bottomRight.x * TILE_SIZE;
				float y = (bottomRight.y + 1) * TILE_SIZE - (collider.yMin + moveVector.y);
				
				if(x > y) {
					return new Vector2(bottomRight.x * TILE_SIZE - collider.xMax, moveVector.y);
				} else {
					return new Vector2(moveVector.x, ((bottomRight.y + 1) * TILE_SIZE) - collider.yMin);
				}
			}

			if(topLeft != null) {
				float x = (topLeft.x + 1) * TILE_SIZE - (collider.xMin + moveVector.x);
				float y = (collider.yMax + moveVector.y) - topLeft.y * TILE_SIZE;
				
				if(x > y) {
					return new Vector2( ((topLeft.x + 1) * TILE_SIZE) - collider.xMin, moveVector.y);
				} else {
					return new Vector2(moveVector.x, topLeft.y * TILE_SIZE - collider.yMax);
				}
			}
			
			if(topRight != null) {
				float x = (collider.xMax + moveVector.x) - topRight.x * TILE_SIZE;
				float y = (collider.yMax + moveVector.y) - topRight.y * TILE_SIZE;
				
				if(x > y) {
					return new Vector2(topRight.x * TILE_SIZE - collider.xMax, moveVector.y);
				} else {
					return new Vector2(moveVector.x, topRight.y * TILE_SIZE - collider.yMax);
				}
			}
		} else {// side collision
			float x = 0;
			float y = 0;
			if(left) {
				x = ((topLeft.x + 1) * TILE_SIZE) - collider.xMin;
			} else if(right) {
				x = topRight.x * TILE_SIZE - collider.xMax;
			}
			
			if(top) {
				y = topRight.y * TILE_SIZE - collider.yMax;
			} else if(bottom) {
				y = ((bottomRight.y + 1) * TILE_SIZE) - collider.yMin;
			}
			
			Vector2 m = new Vector2(x, y);
			return m;
		}
		
		return new Vector2(); // never happens
	}
}
