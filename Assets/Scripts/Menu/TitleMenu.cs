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
  public Transform swapFilesContainer;

  private string newGardenName = "Scadrial";

  // All public variables are assigned in editor

  void Start() {
    mainUI.SetActive(true);
    swapUI.SetActive(false);
  }

  public void MainPlay() {
    tools.Setup();
    garden.SetGardenMode(GardenMode.Play);
  }

  public void MainToSwap() {

    // Update swap files
    UpdateSwapFiles();

    // Set active
    mainUI.SetActive(false);
    swapUI.SetActive(true);
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

  public void SwapNewGarden() {
    if (newGardenName.Length == 0)
      return;

    garden.NewGarden(newGardenName);
    SwapToMain();
  }

  public void SwapChangeName(string newName) {
    newGardenName = newName;
  }

  public void SwapGarden(GardenSave save) {
    garden.SetGardenFromSave(save);
    SwapToMain();
  }

  public void DeleteGarden(GardenSave save) {
    garden.DeleteGarden(save);
    UpdateSwapFiles();
  }

  public void SwapToMain() {
    mainUI.SetActive(true);
    swapUI.SetActive(false);
  }

  public void UpdateSwapFiles() {

    // Fill swap
    List<GardenSave> saves = garden.GetAllGardenSaves();

    // Clear files
    foreach (Transform child in swapFilesContainer)
      Destroy(child.gameObject);

    // Fill files
    foreach (GardenSave save in saves) {
      GameObject go = Instantiate(swapFilePrefab, swapFilePrefab.transform.position, Quaternion.identity, swapFilesContainer);

      go.name = "File: " + save.gardenName;

      Button btn = go.transform.Find("Button").gameObject.GetComponent<Button>();
      btn.onClick.AddListener(() => { SwapGarden(save); });

      Text txt = go.transform.Find("Button/Text").gameObject.GetComponent<Text>();
      txt.text = save.gardenName;

      Button del = go.transform.Find("Delete").gameObject.GetComponent<Button>();
      del.onClick.AddListener(() => { DeleteGarden(save); });

      if (save.gardenID > 0)
        txt.text += " ";

      for (int i = 0; i < save.gardenID; i++) {
        txt.text += "I";
      }

      if (save.gardenID == 0)
        txt.text = save.gardenName;
    }
  }
}