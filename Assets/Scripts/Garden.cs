using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garden : MonoBehaviour {
  // Game controller that handles garden data, includes:
  //    garden meta data (size, name)
  //    garden contents (units in garden)

  private GardenBoard gardenBoard;
  private GardenMode gardenMode;

  // Garden meta:
  public string gardenName;
  public int gardenSize = 4;  // Garden dimensions 4x4

  // Garden contents:
  private float unitSizeLimit = 6;
  private List<Unit> units = new List<Unit>();  // List of units in garden
  private Transform unitsCont; // GameObject container for unit gameObjects

  // Garden mode gameobjects
  public GameObject mainMenuUI;
  public GameObject mainMenuCamera;
  public GameObject playUI;
  public GameObject playWand;

  // All public variables are assigned in editor

  void Awake() {

    // Awake with components
    gardenBoard = GetComponent<GardenBoard>();

    unitsCont = transform.Find("Units");
  }

  void Start() {
    SetGardenMode(GardenMode.MainMenu);
  }

  // Set garden mode and update related
  public void SetGardenMode(GardenMode m) {
    gardenMode = m;
    mainMenuUI.SetActive(gardenMode == GardenMode.MainMenu);
    mainMenuCamera.SetActive(gardenMode == GardenMode.MainMenu);
    playUI.SetActive(gardenMode == GardenMode.Play);
    playWand.SetActive(gardenMode == GardenMode.Play);

    if (gardenMode == GardenMode.Play)
      Time.timeScale = 1f;
    else
      Time.timeScale = 0f;
  }

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
