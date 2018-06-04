using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour {
  // Game controller that handles main menu

  public Garden garden;

  // All public variables are assigned in editor

  public void Play() {
    garden.SetGardenMode(GardenMode.Play);
  }

  public void Quit() {

#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
    Application.OpenURL(webplayerQuitURL);
#else
    Application.Quit();
#endif
  }
}