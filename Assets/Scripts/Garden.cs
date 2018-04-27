using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garden : MonoBehaviour {
  // Game controller that handles garden data, includes:
  //    garden meta data (size, name)
  //    garden contents (units in garden)

  private GardenBoard gardenBoard;

  // Garden meta:
  public string gardenName;
  public int gardenSize = 4;  // Garden dimensions 4x4

  // Garden contents:
  private float unitSizeLimit = 6;
  private List<Unit> units = new List<Unit>();  // List of units in garden
  private Transform unitsCont; // GameObject container for unit gameObjects

  // Garden time: (TODO : Re design and implement, might separate)

  void Awake() {

    // Awake with components
    gardenBoard = GetComponent<GardenBoard>();

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
    go.transform.position += new Vector3(0, go.GetComponent<MonsterMover>().height, 0);

    Unit unit = go.GetComponent<Unit>();
    units.Add(unit);
  }

  // Remove a monster from the garden, and destroy it's gameObject
  public void RemoveMonster(Monster monster) {

    Unit unit = (Unit)monster;
    units.Remove(unit);

    Destroy(unit.gameObject);
  }

  // Get garden's board
  public GardenBoard GetBoard() {
    return gardenBoard;
  }

  // Get number of units of type in garden
  public int GetUnitTypeCount(Type t) {

    int count = 0;

    foreach (Unit unit in units) {

      if (unit.GetType() == t)
        count += 1;
    }

    return count;
  }
}
