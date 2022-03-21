using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour {
  // Game controller that handles main menu

  public Garden garden;
  public WandTools tools;

  public GameObject mainUI;

  public GameObject swapUI;
  public GameObject swapFilePrefab;
  public GameObject swapFilePrefabCurrent;
  public Transform swapFilesContainer;

  public GameObject newUI;

  public Text currentGardenText;

  private string newGardenName = "Scadrial";

  // All public variables are assigned in editor

  void Start() {
    bool firstTime = garden.GetAllGardenSaves().Count == 0;

    if (firstTime) {
      mainUI.SetActive(false);
      swapUI.SetActive(false);
      newUI.SetActive(true);
    } else {
      mainUI.SetActive(true);
      swapUI.SetActive(false);
      newUI.SetActive(false);
    }

    UpdateSwapFiles();
  }

  public void TitleOpenTwitter() {
    Application.OpenURL("https://twitter.com/femifel");
  }

  public void MainPlay() {
    tools.Setup();
    garden.SetGameMode(GameMode.Play);
  }

  public void MainToSwap() {

    // Update swap files
    UpdateSwapFiles();

    // Set active
    mainUI.SetActive(false);
    swapUI.SetActive(true);
  }

  public void MainToNew() {

    // Set active
    mainUI.SetActive(false);
    newUI.SetActive(true);
  }

  public void MainQuit() {

#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
    Application.OpenURL(webplayerQuitURL);
#else
    Application.Quit();
#endif
  }

  public void SwapChangeName(string newName) {
    newGardenName = newName;
  }

  public void SwapGarden(GardenSave save) {
    garden.SetGardenFromSave(save);
    UpdateSwapFiles();
    SwapToMain();
  }

  public void DeleteGarden(GardenSave save) {
    garden.DeleteGarden(save);
    UpdateSwapFiles();

    if (!garden.Loaded())
      SwapToNew();
  }

  public void SwapToMain() {
    mainUI.SetActive(true);
    swapUI.SetActive(false);
  }

  public void SwapToNew() {
    newUI.SetActive(true);
    swapUI.SetActive(false);
  }

  public void UpdateSwapFiles() {

    if (!garden.Loaded())
      currentGardenText.text = "";
    else
      currentGardenText.text = "Current Garden: " + garden.GetGardenTitle();

    // Fill swap
    List<GardenSave> saves = garden.GetAllGardenSaves();

    // Clear files
    foreach (Transform child in swapFilesContainer)
      Destroy(child.gameObject);

    // Fill files
    foreach (GardenSave save in saves) {
      GameObject go;

      if (garden.gardenId == save.gardenId && garden.gardenName == save.gardenName)
        go = Instantiate(swapFilePrefabCurrent, swapFilePrefab.transform.position, Quaternion.identity, swapFilesContainer);
      else
        go = Instantiate(swapFilePrefab, swapFilePrefab.transform.position, Quaternion.identity, swapFilesContainer);

      go.name = "File: " + save.gardenName;

      Button btn = go.transform.Find("Button").gameObject.GetComponent<Button>();
      btn.onClick.AddListener(() => { SwapGarden(save); });

      Text txt = go.transform.Find("Button/Text").gameObject.GetComponent<Text>();
      txt.text = garden.GetGardenTitle(save.gardenName, save.gardenId);

      Button del = go.transform.Find("Delete").gameObject.GetComponent<Button>();
      del.onClick.AddListener(() => { DeleteGarden(save); });
    }
  }

  public void NewCreateNewGarden() {
    if (newGardenName.Length == 0)
      return;

    garden.NewGarden(newGardenName);
    UpdateSwapFiles();
    NewToMain();
  }

  public void NewToMain() {
    newUI.SetActive(false);
    mainUI.SetActive(true);
  }
}