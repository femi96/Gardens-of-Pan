﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class Garden : MonoBehaviour {
  // Game controller that handles garden data, includes:
  //    SIZE/NAME of garden
  //    FILE MANAGEMENT or SAVING/LOADING garden
  //    GARDEN MODE of game/garden
  //    UNITS in garden

  private GardenBoard gardenBoard;
  private GardenMode gardenMode;

  // Garden meta:
  public string gardenName;
  public int gardenID;
  public int gardenSize = 4;  // Garden dimensions 4x4
  public bool saveGarden = false;
  public bool noGarden = true;

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

  private string recentSaveFilePath;
  public GameObject saveUI;

  // All public variables are assigned in editor

  void Awake() {

    // Awake with components
    gardenBoard = GetComponent<GardenBoard>();

    recentSaveFilePath = Application.persistentDataPath + "/recent_garden.path";
    unitsCont = transform.Find("Units");

    // Try load garden so title can start
    SetGardenMode(GardenMode.Title);
    LoadGarden();
  }

  void Update() {

    // Autosave on interval
    saveTime += Time.deltaTime;

    if (saveGarden || saveTime >= saveInterval) {
      SaveGarden();
      saveGarden = false;
      saveTime -= saveInterval;
      saveTime = Mathf.Max(saveTime, 0);
    }

    saveUI.SetActive(saveTime <= 2f);
  }

  // ========================================
  // FILE MANAGEMENT or SAVING/LOADING garden
  // ========================================

  // Get file path for garden from its name and ID
  private string GetFilePath(string name, int iD) {
    return Application.persistentDataPath + "/garden_" + name + "_" + iD + ".garden";
  }

  // Get a list of all garden save files
  public List<GardenSave> GetAllGardenSaves() {

    DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath);
    FileInfo[] files = info.GetFiles("garden_*.garden");
    List<GardenSave> saves = new List<GardenSave>();

    foreach (FileInfo fileInfo in files) {

      if (File.Exists(fileInfo.FullName)) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(fileInfo.FullName, FileMode.Open);
        GardenSave save = (GardenSave)bf.Deserialize(file);
        file.Close();

        saves.Add(save);
      }
    }

    return saves;
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
  public void SetGardenFromSave(GardenSave save) {

    gardenName = save.gardenName;
    gardenID = save.gardenID;
    gardenBoard.SetBlockMap(save.blockMap);

    noGarden = false;
  }

  // Setup garden as a new garden with given name
  public void NewGarden(string name) {

    // Setup garden class
    gardenName = name;
    gardenID = 0;

    string filePath = GetFilePath(gardenName, gardenID);

    while (File.Exists(filePath)) {
      gardenID += 1;
      filePath = GetFilePath(gardenName, gardenID);
    }

    gardenBoard.NewBoard();

    noGarden = false;

    // Create save file
    SaveGarden();
  }

  // Save this garden to a garden save file
  public void SaveGarden() {

    // 0: Get file path
    string saveFilePath = GetFilePath(gardenName, gardenID);

    // 1: Create save instance
    GardenSave save = CreateGardenSave();

    // 2: Save file and close file stream
    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Create(saveFilePath);
    bf.Serialize(file, save);
    Debug.Log("Garden " + gardenName + " saved to " + saveFilePath);
    file.Close();

    // 2b: Save file as most recent file path
    FileStream rFile = File.Create(recentSaveFilePath);
    bf.Serialize(rFile, saveFilePath);
    Debug.Log("Recent garden's path saved to " + gardenName + " saved to " + recentSaveFilePath);
    rFile.Close();

    // 3: Ending save
    // Post save operations if necessary
  }

  // Load the garden from the most recent file, or create an empty garden
  public void LoadGarden() {

    // Try to find recent save file path
    if (File.Exists(recentSaveFilePath)) {
      BinaryFormatter bf = new BinaryFormatter();

      FileStream rFile = File.Open(recentSaveFilePath, FileMode.Open);
      string saveFilePath = (string)bf.Deserialize(rFile);
      rFile.Close();

      // Try to load recent save file
      if (File.Exists(saveFilePath)) {
        FileStream file = File.Open(saveFilePath, FileMode.Open);
        GardenSave save = (GardenSave)bf.Deserialize(file);
        file.Close();

        SetGardenFromSave(save);
        return;
      }
    }

    // If no recent file, try to find another file, and load the first
    List<GardenSave> saves = GetAllGardenSaves();

    foreach (GardenSave save in saves) {
      SetGardenFromSave(save);
      return;
    }

    // If no garden files at all
    noGarden = true;
  }

  // Delete garden save file given garden save
  public void DeleteGarden(GardenSave save) {
    string filePath = GetFilePath(save.gardenName, save.gardenID);
    Debug.Log("Garden " + save.gardenName + " deleted from " + filePath);
    File.Delete(filePath);

    // Deleted current garden
    if (save.gardenName == gardenName && save.gardenID == gardenID) {

      // Try to load next

      // Cant load next, no garden
      noGarden = true;
      // also clear garden
    }
  }

  // ==========================
  // GARDEN MODE of game/garden
  // ==========================

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
