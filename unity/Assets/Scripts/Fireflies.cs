using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fireflies : MonoBehaviour {
	private List<OTAnimatingSprite> fireflies = new List<OTAnimatingSprite>();
	
	private List<Vector2> spawnPoints = new List<Vector2>();
	
	private bool spawning = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(spawning && fireflies.Count < 16 && Random.Range(0.0f, 1.0f) < 0.009f && spawnPoints.Count > 0) {
			Vector2 pos = spawnPoints[Random.Range (0, spawnPoints.Count)];
			
			Add (pos.x, pos.y);
		}
		
		foreach(OTAnimatingSprite firefly in fireflies) {
			if(firefly.GetComponent<Firefly>().Dead()) {
				fireflies.Remove(firefly);
				OT.Destroy(firefly);
				break;
			}
		}
	}
	
	public void Add(float x, float y, bool immediate = false) {
		OTAnimatingSprite firefly = (OTAnimatingSprite) OT.CreateSprite("Firefly");
		firefly.transform.parent = this.transform;
		firefly.transform.localPosition = new Vector3(x, y, 0f);
		
		fireflies.Add(firefly);
		if(immediate) {
			firefly.GetComponent<Firefly>().immediate = true;
		}
	}
	
	public void SpawnAt(float x, float y) {
		spawnPoints.Add (new Vector2(x * 1f, y * 1f));
	}
	
	public void StartSpawning() {
		spawning = true;
	}
	
	public bool Captures(Rect net) {
		foreach(OTAnimatingSprite firefly in fireflies) {
			if((net.Contains(firefly.transform.position) && firefly.GetComponent<Firefly>().Visible())) {
				fireflies.Remove(firefly);
				OT.Destroy(firefly);
				return true;
			}
		}
		return false;
	}
}
