using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WandTools : MonoBehaviour {
  // Controller that handles the player's tool inputs behaviors

  // Assigned in Editor:
  public GardenBoard gardenBoard;

  private ToolType[] tools;
  private int toolIndex;

  private ToolAction toolActionMain;
  private ToolAction toolActionOff;

  // Timing:
  private float swapTime;
  private const float SwapCooldown = 0.25f;

  private float actionTime;
  private const float ActionCooldown = 0.1f;

  // UI:
  public GameObject actionMainUI;
  private Text actionMainText;

  public GameObject actionOffUI;
  private Text actionOffText;

  public Text toolText;
  public Text swapLeftText;
  public Text swapRightText;

  // Effect:
  public Transform effectContainer;

  public GameObject dirtCloud;
  public GameObject grassCloud;

  // All public variables are assigned in editor

  void Awake() {

    actionMainText = actionMainUI.transform.Find("Text").GetComponent<Text>();
    actionOffText = actionOffUI.transform.Find("Text").GetComponent<Text>();
  }

  void Start() {

    toolIndex = 0;
    tools = new ToolType[] {
      ToolType.None, ToolType.Shovel, ToolType.LifeOrb, ToolType.WetDryOrb, ToolType.HotColdOrb,
    };
  }

  void Update() {

    swapTime += Time.deltaTime;
    actionTime += Time.deltaTime;

    if (actionTime >= ActionCooldown)
      InputAction();

    if (swapTime >= SwapCooldown && actionTime >= ActionCooldown)
      InputSwap();

    // if (swapTime >= SwapCooldown && actionTime >= ActionCooldown)
    if (actionTime >= ActionCooldown)
      UpdateToolActions();

    UpdateToolUI();
  }

  // Change tool index on input
  private void InputSwap() {

    if (Input.GetAxis(InputConstants.ToolSwapLeft) > 0) {
      swapTime = 0;
      toolIndex -= 1;
    }

    if (Input.GetAxis(InputConstants.ToolSwapRight) > 0) {
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

    if (Input.GetAxis(InputConstants.ToolUseOff) > 0)
      t = toolActionOff;

    if (Input.GetAxis(InputConstants.ToolUseMain) > 0)
      t = toolActionMain;

    // Apply action if exists
    if (t != ToolAction.None) {
      actionTime = 0;
      ApplyAction(t);
    } else {
      // reset previous action, so can hold
    }
  }

  // Apply input to effect garden
  private void ApplyAction(ToolAction a) {

    // Apply tool action to appropriate effect
    Vector3 v = transform.position;

    switch (a) {

    case ToolAction.Dig:
      Instantiate(dirtCloud, transform.position, Quaternion.identity, effectContainer);
      break;

    case ToolAction.Fill:
      break;

    case ToolAction.Flatten:
      Instantiate(dirtCloud, transform.position, Quaternion.identity, effectContainer);
      break;

    case ToolAction.Grass:
      Instantiate(grassCloud, transform.position, Quaternion.identity, effectContainer);
      break;

    case ToolAction.Remove:
      break;

    case ToolAction.Wet:
      break;

    case ToolAction.Dry:
      break;

    case ToolAction.Heat:
      break;

    case ToolAction.Chill:
      break;

    default:
      break;
    }

    gardenBoard.ApplyAction(v, a);
  }

// Update UI and strings based on index
  private void UpdateToolActions() {

    ToolType tool = tools[toolIndex];
    BlockType b = gardenBoard.GetType(transform.position);

    toolActionMain = ToolAction.None;
    toolActionOff = ToolAction.None;

    switch (tool) {

    case ToolType.Shovel:
      if (b == BlockType.Rough) {
        toolActionMain = ToolAction.Flatten;
        break;
      }

      if (!BlockTypes.InGroup(b, BlockTypes.DepthDeep))
        toolActionMain = ToolAction.Dig;

      if (!BlockTypes.InGroup(b, BlockTypes.DepthGround))
        toolActionOff = ToolAction.Fill;

      break;

    case ToolType.LifeOrb:

      if (b == BlockType.Dirt || b == BlockType.Scorch || b == BlockType.Tundra)
        toolActionMain = ToolAction.Grass;

      if (b == BlockType.Grassland || b == BlockType.Ashland || b == BlockType.Snow)
        toolActionOff = ToolAction.Remove;

      break;

    case ToolType.WetDryOrb:

      if (b == BlockType.Sand || b == BlockType.Grassland)
        toolActionMain = ToolAction.Wet;

      if (b == BlockType.Dirt || b == BlockType.Wetland)
        toolActionOff = ToolAction.Dry;

      break;

    case ToolType.HotColdOrb:

      if (!BlockTypes.InGroup(b, BlockTypes.TempHot))
        toolActionMain = ToolAction.Heat;

      if (!BlockTypes.InGroup(b, BlockTypes.TempCold))
        toolActionOff = ToolAction.Chill;

      break;

    default:
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
