using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class Garden : MonoBehaviour {
  /*
  Handles garden meta data, file management, & game mode
  */

  private GardenBoard gardenBoard;
  private GameMode gameMode;

  [Header("Debug")]
  public bool debugSaveGarden = false;

  [Header("Garden Meta")]
  public string gardenName;
  public int gardenId;
  public int gardenSize = 25;
  private bool gardenLoaded = false;

  [Header("Game Mode")]
  public GameObject mainMenuUI;
  public GameObject mainMenuCamera;
  public GameObject playUI;
  public GameObject playWand;
  public Text currentGardenText;

  [Header("Saving")]
  public GameObject saveUI;
  private float saveTime = 5f;
  private string recentSaveFilePath;
  private const float AutoSaveInterval = 60;

  void Awake() {
    gardenBoard = GetComponent<GardenBoard>();
    recentSaveFilePath = Application.persistentDataPath + "/recent_garden.path";
    SetGameMode(GameMode.Title);
    TryInitialLoad();
  }

  void Update() {
    if (gameMode == GameMode.Play) {
      // Autosave
      saveTime += Time.deltaTime;
      saveUI.SetActive(saveTime <= 2f);

      if (debugSaveGarden || saveTime >= AutoSaveInterval) {
        SaveGarden();
        debugSaveGarden = false;
        saveTime = 0;
      }
    }
  }

  /*
  File management
  */

  private string GetFilePath(string name, int id) {
    return Application.persistentDataPath + "/garden_" + name + "_" + id + ".garden";
  }

  public string GetGardenTitle(string name, int id) {
    string title = name;

    if (id > 0)
      title += " ";

    for (int i = 0; i < id; i++) {
      title += "I";
    }

    return title;
  }

  public string GetGardenTitle() {
    return GetGardenTitle(gardenName, gardenId);
  }

  // Get a list of all garden save files
  public List<GardenSave> GetAllGardenSaves() {
    DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath);
    FileInfo[] files = info.GetFiles("garden_*.garden");
    List<GardenSave> saves = new List<GardenSave>();

    foreach (FileInfo fileInfo in files) {

      if (File.Exists(fileInfo.FullName)) {
        try {
          BinaryFormatter bf = new BinaryFormatter();
          FileStream file = File.Open(fileInfo.FullName, FileMode.Open);
          GardenSave save = (GardenSave)bf.Deserialize(file);
          file.Close();
          saves.Add(save);
        } catch (System.Exception e) { Debug.LogError(e); }
      }
    }

    return saves;
  }

  // Creates a garden save representation of this garden
  private GardenSave CreateGardenSave() {
    GardenSave save = new GardenSave();
    save.gardenName = gardenName;
    save.gardenId = gardenId;
    save.voxelMap = gardenBoard.GetVoxelMap();
    return save;
  }

  // Sets this garden from a garden save representation
  public void SetGardenFromSave(GardenSave save) {
    gardenName = save.gardenName;
    gardenId = save.gardenId;
    gardenBoard.SetVoxelMap(save.voxelMap);
    gardenLoaded = true;
    currentGardenText.text = "Current Garden: " + GetGardenTitle();
  }

  // Setup garden as a new garden with given name
  public void NewGarden(string name) {
    gardenName = name;
    gardenId = 0;

    string filePath = GetFilePath(gardenName, gardenId);

    while (File.Exists(filePath)) {
      gardenId += 1;
      filePath = GetFilePath(gardenName, gardenId);
    }

    gardenBoard.NewBoard();
    gardenLoaded = true;
    SaveGarden();
  }

  // Save this garden to a garden save file
  public void SaveGarden() {
    try {

      // 0: Get file path
      string saveFilePath = GetFilePath(gardenName, gardenId);

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

    } catch (System.Exception e) { Debug.LogError(e); }
  }

  // Load the garden from the most recent file, or create an empty garden
  public void TryInitialLoad() {

    // Try to find recent save file path
    if (File.Exists(recentSaveFilePath)) {
      try {
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
      } catch (System.Exception e) { Debug.LogError(e); }
    }

    // If no recent file, try to find another file, and load the first
    List<GardenSave> saves = GetAllGardenSaves();

    foreach (GardenSave save in saves) {
      SetGardenFromSave(save);
      return;
    }

    // If no garden files at all
    gardenLoaded = false;
  }

  // Delete garden save file given garden save
  public void DeleteGarden(GardenSave save) {
    string filePath = GetFilePath(save.gardenName, save.gardenId);
    Debug.Log("Garden " + save.gardenName + " deleted from " + filePath);
    File.Delete(filePath);

    // Deleted current garden
    if (save.gardenName == gardenName && save.gardenId == gardenId) {

      // Try to load next
      List<GardenSave> saves = GetAllGardenSaves();

      foreach (GardenSave fileSave in saves) {
        SetGardenFromSave(fileSave);
        return;
      }

      // Cant load next, no garden
      gardenLoaded = false;
      // also clear garden
    }
  }

  public bool Loaded() {
    return gardenLoaded;
  }

  /*
  Game mode
  */

  // Set garden mode and update related
  public void SetGameMode(GameMode m) {
    gameMode = m;
    mainMenuUI.SetActive(gameMode == GameMode.Title);
    mainMenuCamera.SetActive(gameMode == GameMode.Title);
    playUI.SetActive(gameMode == GameMode.Play);
    playWand.SetActive(gameMode == GameMode.Play);

    if (gameMode == GameMode.Play)
      Time.timeScale = 1f;
    else
      Time.timeScale = 0f;
  }
}
