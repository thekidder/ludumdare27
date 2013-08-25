using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fireflies : MonoBehaviour {
	private List<OTAnimatingSprite> fireflies = new List<OTAnimatingSprite>();
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Add(int x, int y) {
		OTAnimatingSprite firefly = (OTAnimatingSprite) OT.CreateSprite("Firefly");
		firefly.transform.parent = this.transform;
		firefly.transform.localPosition = new Vector3(x, y, 0f);
		
		fireflies.Add(firefly);
	}
	
	public bool Captures(Rect net) {
		foreach(OTAnimatingSprite firefly in fireflies) {
			if(net.Contains(firefly.transform.localPosition)) {
				fireflies.Remove(firefly);
				OT.Destroy(firefly);
				return true;
			}
		}
		return false;
	}
}
