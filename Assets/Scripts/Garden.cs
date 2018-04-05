using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garden : MonoBehaviour {
  // Game controller that handles garden data, includes:
  //    garden meta data (size, name)
  //    garden contents (units in garden)

  // Garden meta:
  public string gardenName;
  public int gardenSize = 4;  // Garden dimensions 4x4

  // Garden contents:
  private float unitSizeLimit = 4;
  private List<Unit> units = new List<Unit>();  // List of units in garden
  private Transform unitsCont; // GameObject container for unit gameObjects

  // Garden time: (TODO : Re design and implement, might separate)

  void Awake() {

    // Awake with components
    unitsCont = transform.Find("Units");
  }

  void Start() {}

  void Update() {}

  void FixedUpdate() {}

  // Get total size of all units
  public float UnitSizeCount() {

    float sizeTotal = 0;

    foreach (Unit unit in units) {
      sizeTotal += unit.GetSize();
    }

    return sizeTotal;
  }

  // Get remaining room for new units
  public float FreeRoom() {

    return unitSizeLimit - UnitSizeCount();
  }

  // Create monster from prefab newMonster and add it to garden at spawn
  public void AddMonster(GameObject newMonster, SpawnPoint spawn) {

    GameObject go = Instantiate(newMonster, spawn.GetPosition(), spawn.GetRotation(), unitsCont);
    Unit unit = go.GetComponent<Unit>();
    units.Add(unit);
  }
}
