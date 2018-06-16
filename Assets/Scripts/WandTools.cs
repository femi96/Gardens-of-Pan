using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WandTools : MonoBehaviour {
  //  Controller that handles the player wand's behaviors:
  //    TOOL ACTIONS
  //    TOOL MENUS

  // Assigned in Editor:
  public GardenBoard gardenBoard;
  public WandCamera wandCamera;

  public bool wandMenuOpen = false;

  private ToolType tool;
  private ToolAction toolActionMain;
  private ToolAction toolActionOff;

  // Timing:
  private float actionTime;
  private const float ActionCooldown = 0.1f;

  // UI:
  public GameObject toolGuide;
  public GameObject toolWheel;

  // Effect:
  public Transform effectContainer;

  public GameObject dirtCloud;
  public GameObject grassCloud;

  // All public variables are assigned in editor

  void Awake() {}

  void Start() {}

  void Update() {

    actionTime += Time.deltaTime;

    if (!wandMenuOpen) {
      // Wand mode

      if (actionTime >= ActionCooldown)
        InputAction();

      if (Input.GetButtonDown(InputConstants.ToolSpace))
        ToolWheelToggle();

    } else {
      // Menu mode

      if (Input.GetButtonDown(InputConstants.ToolSpace))
        ToolWheelToggle();
    }
  }

  // Setup tools
  public void Setup() {
    SetTool(ToolType.None);
    wandMenuOpen = false;
    wandCamera.SetCameraMode(!wandMenuOpen);
    UpdateUI();
  }

  // Set wand tool
  public void SetTool(ToolType t) {
    tool = t;
    UpdateToolActions();
  }

  // Open/close tool wheel
  public void ToolWheelToggle() {

    wandMenuOpen = !wandMenuOpen;
    wandCamera.SetCameraMode(!wandMenuOpen);
    UpdateUI();
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

      UpdateToolActions();
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

    UpdateUI();
  }

  // Update tool wheel and guide based on current fields
  private void UpdateUI() {

    // TODO OPTIMIZE: Move finds to start so they arent called every frame

    // Tool wheel
    toolWheel.SetActive(wandMenuOpen);

    // Tool guide

    // Update tool
    GameObject toolUI = toolGuide.transform.Find("Tool/Active").gameObject;
    Text toolText = toolGuide.transform.Find("Tool/Active/Text").gameObject.GetComponent<Text>();
    toolUI.SetActive(tool != ToolType.None);
    toolText.text = tool.ToString();

    // Enable UI main action
    GameObject mainUI = toolGuide.transform.Find("Main/Active").gameObject;
    Text mainText = toolGuide.transform.Find("Main/Active/Text").gameObject.GetComponent<Text>();
    mainUI.SetActive(toolActionMain != ToolAction.None);
    mainText.text = toolActionMain.ToString();

    // Enable UI off action
    GameObject offUI = toolGuide.transform.Find("Off/Active").gameObject;
    Text offText = toolGuide.transform.Find("Off/Active/Text").gameObject.GetComponent<Text>();
    offUI.SetActive(toolActionOff != ToolAction.None);
    offText.text = toolActionOff.ToString();

    // Enable UI off action
    Text spaceText = toolGuide.transform.Find("Space/Active/Text").gameObject.GetComponent<Text>();

    if (!wandMenuOpen)
      spaceText.text = "Tool Wheel";
    else
      spaceText.text = "Garden";
  }
}
