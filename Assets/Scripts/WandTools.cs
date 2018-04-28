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

  // Timing:
  private float swapTime;
  private const float swapCooldown = 0.25f;

  private float actionTime;
  private const float actionCooldown = 0.1f;

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

    if (actionTime >= actionCooldown)
      InputAction();

    if (swapTime >= swapCooldown && actionTime >= actionCooldown)
      InputSwap();

    // if (swapTime >= swapCooldown && actionTime >= actionCooldown)
    // if (actionTime >= actionCooldown)
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

    if (Input.GetButtonDown(InputConstants.ToolUseOff))
      t = toolActionOff;

    if (Input.GetButtonDown(InputConstants.ToolUseMain))
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

    int[] features = BlockTypeFeatures.GetFeatures(b);

    switch (t) {

    case ToolAction.Dig:
      features[0] -= 1;
      Instantiate(dirtCloud, transform.position, Quaternion.identity, effectContainer);
      break;

    case ToolAction.Fill:
      features[0] += 1;
      break;

    case ToolAction.Flatten:
      features[0] -= 1;
      Instantiate(dirtCloud, transform.position, Quaternion.identity, effectContainer);
      break;

    case ToolAction.Grass:
      features[1] += 1;
      Instantiate(grassCloud, transform.position, Quaternion.identity, effectContainer);
      break;

    case ToolAction.Remove:
      features[1] -= 1;
      break;

    case ToolAction.Wet:
      features[2] += 1;
      // Instantiate(dirtCloud, transform.position, Quaternion.identity, effectContainer);
      break;

    case ToolAction.Dry:
      features[2] -= 1;
      // Instantiate(dirtCloud, transform.position, Quaternion.identity, effectContainer);
      break;

    case ToolAction.Heat:
      features[3] += 1;
      // Instantiate(dirtCloud, transform.position, Quaternion.identity, effectContainer);
      break;

    case ToolAction.Chill:
      features[3] -= 1;
      // Instantiate(dirtCloud, transform.position, Quaternion.identity, effectContainer);
      break;

    default:
      break;
    }

    gardenBoard.SetType(v, BlockTypeFeatures.GetType(features));
  }

// Update UI and strings based on index
  private void UpdateToolActions() {

    ToolType t = tools[toolIndex];
    BlockType b = gardenBoard.GetType(transform.position);

    toolActionMain = ToolAction.None;
    toolActionOff = ToolAction.None;

    switch (t) {

    case ToolType.Shovel:

      if (b == BlockType.Rough)
        toolActionMain = ToolAction.Flatten;

      if (BlockTypeGroups.InGroup(b, BlockTypeGroups.Ground)
          || BlockTypeGroups.InGroup(b, BlockTypeGroups.Shallow))
        toolActionMain = ToolAction.Dig;

      if (BlockTypeGroups.InGroup(b, BlockTypeGroups.Shallow)
          || BlockTypeGroups.InGroup(b, BlockTypeGroups.Deep))
        toolActionOff = ToolAction.Fill;

      break;

    case ToolType.LifeOrb:

      if (BlockTypeGroups.InGroup(b, BlockTypeGroups.GroundBasic)
          || BlockTypeGroups.InGroup(b, BlockTypeGroups.Life)
          || b == BlockType.Scorch || b == BlockType.Tundra)
        toolActionMain = ToolAction.Grass;

      if (BlockTypeGroups.InGroup(b, BlockTypeGroups.Life)
          || b == BlockType.Overgrowth
          || b == BlockType.Ashland || b == BlockType.Snow)
        toolActionOff = ToolAction.Remove;

      break;

    case ToolType.WetDryOrb:

      if (b == BlockType.Sand || b == BlockType.DirtDry || b == BlockType.Dirt
          || b == BlockType.Grassland || b == BlockType.Aridland || b == BlockType.Overgrowth)
        toolActionMain = ToolAction.Wet;

      if (BlockTypeGroups.InGroup(b, BlockTypeGroups.GroundBasic)
          || b == BlockType.Grassland || b == BlockType.Wetland || b == BlockType.Overgrowth)
        toolActionOff = ToolAction.Dry;

      break;

    case ToolType.HotColdOrb:

      if (BlockTypeGroups.InGroup(b, BlockTypeGroups.Temperate)) {
        toolActionMain = ToolAction.Heat;
        toolActionOff = ToolAction.Chill;
      }

      if (BlockTypeGroups.InGroup(b, BlockTypeGroups.Cold))
        toolActionMain = ToolAction.Heat;

      if (BlockTypeGroups.InGroup(b, BlockTypeGroups.Hot))
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
