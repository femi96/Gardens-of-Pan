using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelMenu : MonoBehaviour {
  // Game controller that handles wheel menu

  public Garden garden;
  public WandTools tools;

  // All public variables are assigned in editor

  public void SetToolShovel() {
    tools.SetTool(ToolType.Shovel);
    tools.ToolWheelToggle();
  }

  public void SetToolGrassSeed() {
    tools.SetTool(ToolType.GrassSeed);
    tools.ToolWheelToggle();
  }

  public void SetToolHumidityPump() {
    tools.SetTool(ToolType.HumidityPump);
    tools.ToolWheelToggle();
  }

  public void SetToolTemperatureControl() {
    tools.SetTool(ToolType.TemperatureControl);
    tools.ToolWheelToggle();
  }

  public void SetToolSeed() {
    tools.SetTool(ToolType.Seed);
    tools.ToolWheelToggle();
  }

  public void SetToolNone() {
    tools.SetTool(ToolType.None);
    tools.ToolWheelToggle();
  }

  public void WheelToGarden() {
    tools.ToolWheelToggle();
  }

  public void WheelToTitle() {
    garden.SaveGarden();
    garden.SetGardenMode(GardenMode.Title);
  }
}