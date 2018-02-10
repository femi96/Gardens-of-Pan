using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garden : MonoBehaviour {
	// Garden: Controller that handles the garden data
	

	// Garden variables:
	[Header("Garden")]
	public string gardenName;
	public int gardenSize = 4;

	// Day/Night variables:
	[Header("Day/Night")]
	public float gardenTime;
	private float dayTime = 20f;
	private float nightTime = 15f;
	private float cycleTime;

	public float dayLightIntensity;
	private float dayLightMagnitude = 0.75f;

	private Light dayLight;

	// Spawns
	[Header("Spawns")]

	// Units
	[Header("Units")]
	public float unitSizeLimit;
	public List<Unit> units = new List<Unit>();

	private Transform unitsCont; // Unit Container


	// Unity MonoBehavior Functions:
	void Awake() {

		// Awake with components
		// wild = GetComponent<Wild>();			// Awake w wild component
		// chunk = GetComponent<Chunk>();		// Awake w chunk component
		unitsCont = transform.Find("Units");	// Awake w units container child

		dayLight = transform.Find("DayLight").GetComponent<Light>();
	}

	void Start() {

		// Start at 0s
		gardenTime = 0f;

		// Calculate cycle time
		cycleTime = dayTime + nightTime;
	}
	
	void Update() {

		// Increment garden time
		gardenTime += Time.deltaTime;
		if(gardenTime > cycleTime) {
			gardenTime -= cycleTime;
		}

		// Manage light based on time
		if(gardenTime <= dayTime) {
			dayLightIntensity = dayLightMagnitude * Mathf.Sin(gardenTime*(Mathf.PI/dayTime));
		} else {
			dayLightIntensity = 0;
		}
		dayLight.intensity = dayLightIntensity;
		dayLight.intensity = dayLightMagnitude; // Temp disabled
	}

	void FixedUpdate() {}

	// Update spawn points from block data
	public void UpdateSpawnPoints() {

	}

	// Get total size of all units
	public float UnitSizeCount() {
		float sizeTotal = 0;
		foreach(Unit unit in units) {
			sizeTotal += unit.size;
		}
		return sizeTotal;
	}

	// Get remaining room for new units
	public float FreeRoom() {
		return unitSizeLimit - UnitSizeCount();
	}

	// Instantiate Monster from prefab and add to collection
	public void AddMonster(GameObject newMonster) {
		GameObject go = Instantiate(newMonster);
		Unit unit = go.GetComponent<Unit>();
		units.Add(unit);
		go.transform.parent = unitsCont;
	}
}
