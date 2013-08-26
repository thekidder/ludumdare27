using System.Collections;
using UnityEngine;

public class CollisionDetector {
	private const int TILE_SIZE = 32;
	
	private bool[] tiles;
	private int width;
	private int height;
	
	private enum Corner {
		TOP_LEFT,
		TOP_RIGHT,
		BOTTOM_LEFT,
		BOTTOM_RIGHT
	}
	
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

	private Point Collides(float x, float y, Corner corner) {
		if(x % TILE_SIZE == 0 && y % TILE_SIZE == 0) {
			return null; // boundaries do not collide
		}
		
		int intX = (int)(x);// + 0.5);
		int intY = (int)(y);// + 0.5);
		
		if(x % TILE_SIZE == 0f) {
			if(corner == Corner.TOP_RIGHT || corner == Corner.BOTTOM_RIGHT) {
				intX -= 1;
			}
		}
		
		if(y % TILE_SIZE == 0f) {
			if(corner == Corner.TOP_RIGHT || corner == Corner.TOP_LEFT) {
				intX -= 1;
			}
		}
		
		intX /= TILE_SIZE;
		intY /= TILE_SIZE;
		
		if(intX < 0 || intX >= width || intY < 0 || intY >= height) {
			return null;
		}
		
		if(tiles[intX + intY * width]) {
			return new Point(intX, intY);
		}
		return null;
	}
	
	public bool Collides(Rect collider) {
		return Collides (collider.xMin, collider.yMin, Corner.BOTTOM_LEFT) != null
			|| Collides (collider.xMin, collider.yMax, Corner.TOP_LEFT) != null
			|| Collides (collider.xMax, collider.yMax, Corner.TOP_LEFT) != null
			|| Collides (collider.xMax, collider.yMin, Corner.BOTTOM_RIGHT) != null;
	}
	
	public Vector2 Move(Rect collider, Vector2 moveVector) {
		Point bottomLeft  = Collides (collider.xMin + moveVector.x, collider.yMin + moveVector.y, Corner.BOTTOM_LEFT);
		Point topLeft     = Collides (collider.xMin + moveVector.x, collider.yMax + moveVector.y, Corner.TOP_LEFT);
		Point topRight    = Collides (collider.xMax + moveVector.x, collider.yMax + moveVector.y, Corner.TOP_RIGHT);
		Point bottomRight = Collides (collider.xMax + moveVector.x, collider.yMin + moveVector.y, Corner.BOTTOM_RIGHT);
		
		if(bottomLeft == null && bottomRight == null && topLeft == null && topRight == null) {
			return moveVector;
		}
		
		bool left   = bottomLeft  != null && topLeft     != null;
		bool right  = bottomRight != null && topRight    != null;
		bool top    = topLeft     != null && topRight    != null;
		bool bottom = bottomLeft  != null && bottomRight != null;
		
		if(!left && !right && !top && !bottom) {
			// single collision
			float minSq = float.MaxValue;
			Vector2 r = Vector2.zero;
			
			if(bottomLeft != null) {
				Vector2 v = theCollision(collider, bottomLeft, moveVector);
				if(v.sqrMagnitude < minSq) {
					minSq = v.sqrMagnitude;
					r = v;
				}
			}
			
			if(bottomRight != null) {
				Vector2 v = theCollision(collider, bottomRight, moveVector);
				if(v.sqrMagnitude < minSq) {
					minSq = v.sqrMagnitude;
					r = v;
				}
			}

			if(topLeft != null) {
				Vector2 v = theCollision(collider, topLeft, moveVector);
				if(v.sqrMagnitude < minSq) {
					minSq = v.sqrMagnitude;
					r = v;
				}
			}
			
			if(topRight != null) {
				Vector2 v = theCollision(collider, topRight, moveVector);
				if(v.sqrMagnitude < minSq) {
					minSq = v.sqrMagnitude;
					r = v;
				}
			}
			
			return r;
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
	
	private Vector2 Project(Rect r, Vector2 axis) {
		Vector2 n = axis.normalized;
		
		float xMin = r.xMin * n.x;
		float xMax = r.xMax * n.x;
		
		float yMin = r.yMin * n.y;
		float yMax = r.yMax * n.y;
		
		float[] values = {
			xMin + yMin, xMin + yMax,
			xMax + yMin, xMax + yMax };
		
		float min = Mathf.Min(values);
		float max = Mathf.Max(values);
				
		return new Vector2(min, max);
	}
	
	private float Overlap(Vector2 a, Vector2 b) {
		if(a.x < b.x) {
			return a.y - b.x; 
		} else {
			return a.x - b.y;
		}
	}
	
	private Vector2 theCollision(Rect collider, Point collisionPoint, Vector2 moveVector) {
		Vector2 pCollider = Project (collider, moveVector);
		Vector2 pTile = Project (new Rect(collisionPoint.x * 32f, collisionPoint.y * 32f, 32f, 32f), moveVector);
				
		Vector2 r = -moveVector.normalized * Overlap (pCollider, pTile);
		return r;
	}
}
