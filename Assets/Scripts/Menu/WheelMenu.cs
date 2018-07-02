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

  public void SetToolLifeOrb() {
    tools.SetTool(ToolType.LifeOrb);
    tools.ToolWheelToggle();
  }

  public void SetToolWetDryOrb() {
    tools.SetTool(ToolType.WetDryOrb);
    tools.ToolWheelToggle();
  }

  public void SetToolHotColdOrb() {
    tools.SetTool(ToolType.HotColdOrb);
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