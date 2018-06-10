using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelMenu : MonoBehaviour {
  // Game controller that handles wheel menu

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
}