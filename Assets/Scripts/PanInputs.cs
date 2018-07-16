using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PanInputs {

  public const string ToolUseMain = "Click1";
  public const string ToolUseOff = "Click2";
  public const string ToolSpace = "Space";

  public static bool FastForward() {
    return Input.GetKey(KeyCode.F);
  }

  public static bool ScreenCapture() {
    return Input.GetButtonDown("ScreenCapture");
  }

  public static float WandX() { return Input.GetAxis("Horizontal"); }
  public static float WandZ() { return Input.GetAxis("Vertical"); }

  public static float CameraX() { return Input.GetAxis("MouseX"); }
  public static float CameraY() { return Input.GetAxis("MouseY"); }
  public static float CameraZoom() { return Input.GetAxis("MouseScrollWheel"); }
}