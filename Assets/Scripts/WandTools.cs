using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WandTools : MonoBehaviour {
  //  Controller that handles the player's tool inputs behaviors

  // Assigned in Editor:
  public GardenBoard gardenBoard;

  private ToolType[] tools;
  private int toolIndex;

  private ToolAction toolActionMain;
  private ToolAction toolActionOff;

  private float swapTime;
  private const float swapCooldown = 0.25f;

  private float actionTime;
  private const float actionCooldown = 0.1f;

  // UI variables:

  public GameObject actionMainUI;
  public Text actionMainText;

  public GameObject actionOffUI;
  public Text actionOffText;

  public Text toolText;
  public Text swapLeftText;
  public Text swapRightText;

  // Effect variables:
  public Transform effectContainer;

  public GameObject dirtCloud;
  public GameObject grassCloud;
  public GameObject sandCloud;

  // All public variables are assigned in editor

  void Start() {
    toolIndex = 0;
    tools = new ToolType[] {ToolType.None, ToolType.Shovel, ToolType.Grass, ToolType.Sand};
  }

  void Update() {

    swapTime += Time.deltaTime;
    actionTime += Time.deltaTime;

    if (swapTime >= swapCooldown && actionTime >= actionCooldown)
      InputSwap();

    if (actionTime >= actionCooldown)
      InputAction();

    // if (swapTime >= swapCooldown && actionTime >= actionCooldown)
    if (actionTime >= actionCooldown)
      UpdateToolActions();

    UpdateToolUI();
  }

  // Change tool index on input
  private void InputSwap() {

    if (Input.GetAxis("SwapLeft") > 0) {
      swapTime = 0;
      toolIndex -= 1;
    }

    if (Input.GetAxis("SwapRight") > 0) {
      swapTime = 0;
      toolIndex += 1;
    }

    toolIndex = (toolIndex + tools.Length) % tools.Length;
  }

  // Act if tool input
  private void InputAction() {

    // Get tool action from input
    ToolAction t;
    t = ToolAction.None;

    if (Input.GetAxis("Click2") > 0)
      t = toolActionOff;

    if (Input.GetAxis("Click1") > 0)
      t = toolActionMain;

    // Apply action if exists
    if (t != ToolAction.None) {
      actionTime = 0;
      ApplyAction(t);
    }
  }

  // Apply input to effect garden
  private void ApplyAction(ToolAction t) {

    // Apply tool action to appropriate effect
    Vector3 v = transform.position;
    BlockType b = gardenBoard.GetType(transform.position);

    switch (t) {

    case ToolAction.Dig:
      if (b == BlockType.Dirt || b == BlockType.Grass || b == BlockType.Sand)
        gardenBoard.SetType(v, BlockType.Water);

      Instantiate(dirtCloud, transform.position, Quaternion.identity, effectContainer);
      break;

    case ToolAction.Flatten:
      if (b == BlockType.Rough)
        gardenBoard.SetType(v, BlockType.Dirt);

      Instantiate(dirtCloud, transform.position, Quaternion.identity, effectContainer);
      break;

    case ToolAction.Fill:
      if (b == BlockType.Water)
        gardenBoard.SetType(v, BlockType.Dirt);

      break;

    case ToolAction.Grass:
      if (b == BlockType.Dirt || b == BlockType.Sand)
        gardenBoard.SetType(v, BlockType.Grass);

      Instantiate(grassCloud, transform.position, Quaternion.identity, effectContainer);
      break;

    case ToolAction.Remove:
      if (b == BlockType.Grass || b == BlockType.Sand)
        gardenBoard.SetType(v, BlockType.Dirt);

      break;

    case ToolAction.Sand:
      if (b == BlockType.Dirt || b == BlockType.Grass)
        gardenBoard.SetType(v, BlockType.Sand);

      Instantiate(sandCloud, transform.position, Quaternion.identity, effectContainer);
      break;

    default:
      break;
    }
  }

  // Update UI and strings based on index
  private void UpdateToolActions() {

    ToolType t = tools[toolIndex];
    BlockType b = gardenBoard.GetType(transform.position);

    switch (t) {

    case ToolType.Shovel:
      toolActionMain = ToolAction.Dig;
      toolActionOff = ToolAction.Fill;

      if (b == BlockType.Rough) {
        toolActionMain = ToolAction.Flatten;
        toolActionOff = ToolAction.None;
      }

      break;

    case ToolType.Grass:
      toolActionMain = ToolAction.Grass;
      toolActionOff = ToolAction.Remove;

      if (b == BlockType.Rough || b == BlockType.Water) {
        toolActionMain = ToolAction.None;
        toolActionOff = ToolAction.None;
      }

      break;

    case ToolType.Sand:
      toolActionMain = ToolAction.Sand;
      toolActionOff = ToolAction.Remove;

      if (b == BlockType.Rough || b == BlockType.Water) {
        toolActionMain = ToolAction.None;
        toolActionOff = ToolAction.None;
      }

      break;

    case ToolType.None:
      toolActionMain = ToolAction.None;
      toolActionOff = ToolAction.None;

      break;
    }
  }

  // Update UI based on tool index and actions
  private void UpdateToolUI() {

    // Update index, left, and right
    toolText.text = tools[toolIndex].ToString();

    int indexLeft = (toolIndex - 1 + tools.Length) % tools.Length;
    swapLeftText.text = tools[indexLeft].ToString();

    int indexRight = (toolIndex + 1) % tools.Length;
    swapRightText.text = tools[indexRight].ToString();

    // Enable UI if needs to display a tool action
    actionMainUI.SetActive(true);
    actionMainText.text = toolActionMain.ToString();
    actionOffUI.SetActive(true);
    actionOffText.text = toolActionOff.ToString();

    if (toolActionMain == ToolAction.None) { actionMainUI.SetActive(false); }

    if (toolActionOff == ToolAction.None) { actionOffUI.SetActive(false); }
  }
}
