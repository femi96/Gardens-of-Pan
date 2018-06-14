using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class Garden : MonoBehaviour {
  // Game controller that handles garden data, includes:
  //    SIZE/NAME of garden
  //    UNITS in garden
  //    SAVING/LOADING garden

  private GardenBoard gardenBoard;
  private GardenMode gardenMode;

  // Garden meta:
  public string gardenName;
  public int gardenID;
  public int gardenSize = 4;  // Garden dimensions 4x4
  public bool saveGarden = false;
  public bool loadGarden = true;

  // Garden contents:
  private float unitSizeLimit = 6;
  private List<Unit> units = new List<Unit>();  // List of units in garden
  private Transform unitsCont; // GameObject container for unit gameObjects

  // Garden mode gameobjects:
  public GameObject mainMenuUI;
  public GameObject mainMenuCamera;
  public GameObject playUI;
  public GameObject playWand;

  // Garden saving
  private float saveTime = 2f;
  private const float saveInterval = 60; // Saves every 60s
  private string saveFilePath;
  private string recentSaveFilePath;
  public GameObject saveUI;

  // All public variables are assigned in editor

  void Awake() {

    // Awake with components
    gardenBoard = GetComponent<GardenBoard>();

    recentSaveFilePath = Application.persistentDataPath + "/recent_garden.garden";
    unitsCont = transform.Find("Units");
  }

  void Start() {
    SetGardenMode(GardenMode.Title);

    if (loadGarden)
      LoadGarden(recentSaveFilePath);
  }

  void Update() {
    saveTime += Time.deltaTime;

    if (saveGarden || saveTime >= saveInterval) {
      SaveGarden();
      saveGarden = false;
      saveTime -= saveInterval;
      saveTime = Mathf.Max(saveTime, 0);
    }

    saveUI.SetActive(saveTime <= 2f);
  }

  // Creates a garden save representation of this garden
  private GardenSave CreateGardenSave() {

    GardenSave save = new GardenSave();
    save.gardenName = gardenName;
    save.gardenID = gardenID;
    save.blockMap = gardenBoard.GetBlockMap();
    return save;
  }

  // Sets this garden from a garden save representation
  private void SetGardenFromSave(GardenSave save) {

    gardenName = save.gardenName;
    gardenID = save.gardenID;
    gardenBoard.SetBlockMap(save.blockMap);
  }

  // Save the garden
  private void SaveGarden() {

    // 0: Update file path
    saveFilePath = Application.persistentDataPath + "/garden_" + gardenName + "_" + gardenID + ".garden";

    // 1: Create save instance
    GardenSave save = CreateGardenSave();

    // 2: Save file and close file stream
    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Create(saveFilePath);
    bf.Serialize(file, save);
    Debug.Log("Garden " + gardenName + " saved to " + saveFilePath);
    file.Close();

    FileStream rFile = File.Create(recentSaveFilePath);
    bf.Serialize(rFile, save);
    file.Close();

    // 3: Ending save
    // Close game or whatever, based on whatever
  }

  // Load the garden
  private bool LoadGarden(string filePath) {

    if (File.Exists(filePath)) {

      // 1: Prepare garden

      // 2: Load save file
      BinaryFormatter bf = new BinaryFormatter();
      FileStream file = File.Open(filePath, FileMode.Open);
      GardenSave save = (GardenSave)bf.Deserialize(file);
      file.Close();

      // 3: Apply saved garden to garden
      SetGardenFromSave(save);
      return true;

    } else {

      Debug.Log("No game saved!");
      return false;
    }
  }

  // Set garden mode and update related
  public void SetGardenMode(GardenMode m) {
    gardenMode = m;
    mainMenuUI.SetActive(gardenMode == GardenMode.Title);
    mainMenuCamera.SetActive(gardenMode == GardenMode.Title);
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
