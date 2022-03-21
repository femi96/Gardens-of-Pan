using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WandTools : MonoBehaviour {
  // Handles the player wand's tool actions and menus

  // Assigned in Editor:
  public Garden garden;
  public GardenBoard gardenBoard;
  public WandCamera wandCamera;

  public bool wandMenuOpen = false;

  private ToolType tool;
  private ToolAction toolActionMain;
  private ToolAction toolActionOff;

  // Timing
  private float actionTime;
  private const float ActionCooldown = 0.1f;

  // UI
  public GameObject toolGuide;
  public GameObject toolWheel;
  public GameObject unitHoverUI;

  private bool hasSetUIPointers = false;

  private GameObject toolUI;
  private Text toolText;
  private GameObject mainUI;
  private Text mainText;
  private GameObject offUI;
  private Text offText;
  private Text spaceText;

  // Effect
  public Transform effectContainer;

  public GameObject dirtCloud;
  public GameObject grassCloud;

  // Seeds
  public GameObject[] seedPrefabs;
  public int seedIndex;

  // All public variables are assigned in editor

  void Awake() {}

  void Start() {}

  void Update() {

    actionTime += Time.deltaTime;

    if (!wandMenuOpen) {
      // Wand mode

      if (actionTime >= ActionCooldown)
        InputAction();

      if (Input.GetButtonDown(PanInputs.ToolSpace))
        ToolWheelToggle();

    } else {
      // Menu mode

      if (Input.GetButtonDown(PanInputs.ToolSpace))
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

  private void SetupUIPointers() {
    toolUI = toolGuide.transform.Find("Tool/Active").gameObject;
    toolText = toolGuide.transform.Find("Tool/Active/Text").gameObject.GetComponent<Text>();

    mainUI = toolGuide.transform.Find("Main/Active").gameObject;
    mainText = toolGuide.transform.Find("Main/Active/Text").gameObject.GetComponent<Text>();

    offUI = toolGuide.transform.Find("Off/Active").gameObject;
    offText = toolGuide.transform.Find("Off/Active/Text").gameObject.GetComponent<Text>();

    spaceText = toolGuide.transform.Find("Space/Active/Text").gameObject.GetComponent<Text>();

    hasSetUIPointers = true;
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

    if (Input.GetAxis(PanInputs.ToolUseOff) > 0)
      t = toolActionOff;

    if (Input.GetAxis(PanInputs.ToolUseMain) > 0)
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

    // TODO: Expand area of effect

    case ToolAction.Dig:
      ApplyDig(v);
      break;

    case ToolAction.Fill:
      ApplyFill(v);
      break;

    case ToolAction.Soften:
      Instantiate(dirtCloud, transform.position, Quaternion.identity, effectContainer);
      gardenBoard.ApplyAction(v, a);
      break;

    case ToolAction.Grass:
      Instantiate(grassCloud, transform.position, Quaternion.identity, effectContainer);
      gardenBoard.ApplyAction(v, a);
      break;

    case ToolAction.Remove:
      gardenBoard.ApplyAction(v, a);
      break;

    case ToolAction.Wet:
      gardenBoard.ApplyAction(v, a);
      break;

    case ToolAction.Dry:
      gardenBoard.ApplyAction(v, a);
      break;

    case ToolAction.Heat:
      gardenBoard.ApplyAction(v, a);
      break;

    case ToolAction.Chill:
      gardenBoard.ApplyAction(v, a);
      break;

    case ToolAction.Plant_Seed:
      // garden.TryAddUnit(seedPrefabs[seedIndex], transform.position, Quaternion.identity);
      break;

    case ToolAction.Swap_Seed:
      seedIndex = (seedIndex + 1) % seedPrefabs.Length;
      break;

    default:
      break;
    }
  }

  // Update UI and strings based on index
  private void UpdateToolActions() {

    Block b = gardenBoard.GetBlock(transform.position);

    toolActionMain = ToolAction.None;
    toolActionOff = ToolAction.None;

    switch (tool) {

    case ToolType.Shovel:
      if (b.type == BlockType.Rough) {
        toolActionMain = ToolAction.Soften;
        break;
      }

      if (b.height > -2)
        toolActionMain = ToolAction.Dig;

      if (b.height < 0)
        toolActionOff = ToolAction.Fill;

      break;

    case ToolType.GrassSeed:
      toolActionMain = ToolAction.Grass;
      toolActionOff = ToolAction.Remove;
      break;

    case ToolType.HumidityPump:
      toolActionMain = ToolAction.Wet;
      toolActionOff = ToolAction.Dry;
      break;

    case ToolType.TemperatureControl:
      toolActionMain = ToolAction.Heat;
      toolActionOff = ToolAction.Chill;
      break;

    case ToolType.Seed:
      toolActionMain = ToolAction.Plant_Seed;
      toolActionOff = ToolAction.Swap_Seed;
      break;

    default:
      break;
    }

    UpdateUI();
  }

  // Update tool wheel and guide based on current fields
  private void UpdateUI() {

    // Tool wheel
    toolWheel.SetActive(wandMenuOpen);

    // Tool guide
    if (!hasSetUIPointers)
      SetupUIPointers();

    // Update tool
    toolUI.SetActive(tool != ToolType.None);
    toolText.text = tool.ToString().Replace('_', ' ');

    // Enable UI main action
    mainUI.SetActive(toolActionMain != ToolAction.None);
    mainText.text = toolActionMain.ToString().Replace('_', ' ');

    // Enable UI off action
    offUI.SetActive(toolActionOff != ToolAction.None);
    offText.text = toolActionOff.ToString().Replace('_', ' ');

    // Enable UI off action
    if (!wandMenuOpen)
      spaceText.text = "Tool Wheel";
    else
      spaceText.text = "Garden";
  }

  private void ApplyDig(Vector3 v) {
    Instantiate(dirtCloud, v, Quaternion.identity, effectContainer);

    Block z = gardenBoard.GetBlock(v);

    Block f = gardenBoard.GetBlockNeighbor(v, Vector3.forward);
    Block r = gardenBoard.GetBlockNeighbor(v, Vector3.right);
    Block b = gardenBoard.GetBlockNeighbor(v, Vector3.back);
    Block l = gardenBoard.GetBlockNeighbor(v, Vector3.left);

    BlockDir digDir = BlockDir.None;

    // If board conditions are right, then do a half dig instead
    if (z.halfDir == BlockDir.None) {
      if (f != null && r != null && b != null && l != null) {

        bool fsame = f.height == z.height && f.halfDir == BlockDir.None;
        bool rsame = r.height == z.height && r.halfDir == BlockDir.None;
        bool bsame = b.height == z.height && b.halfDir == BlockDir.None;
        bool lsame = l.height == z.height && l.halfDir == BlockDir.None;

        bool flow = f.height < z.height || (f.height == z.height && f.halfDir != BlockDir.None);
        bool rlow = r.height < z.height || (r.height == z.height && r.halfDir != BlockDir.None);
        bool blow = b.height < z.height || (b.height == z.height && b.halfDir != BlockDir.None);
        bool llow = l.height < z.height || (l.height == z.height && l.halfDir != BlockDir.None);

        if (fsame && rsame && blow && llow)
          digDir = BlockDir.Forward;

        if (rsame && bsame && llow && flow)
          digDir = BlockDir.Right;

        if (bsame && lsame && flow && rlow)
          digDir = BlockDir.Back;

        if (lsame && fsame && rlow && blow)
          digDir = BlockDir.Left;
      }
    }

    // Undo half for neighbors
    if (f != null && f.height == z.height && f.halfDir != BlockDir.None)
      gardenBoard.ApplyActionNeighbor(v, Vector3.forward, ToolAction.Dig);

    if (r != null && r.height == z.height && r.halfDir != BlockDir.None)
      gardenBoard.ApplyActionNeighbor(v, Vector3.right, ToolAction.Dig);

    if (b != null && b.height == z.height && b.halfDir != BlockDir.None)
      gardenBoard.ApplyActionNeighbor(v, Vector3.back, ToolAction.Dig);

    if (l != null && l.height == z.height && l.halfDir != BlockDir.None)
      gardenBoard.ApplyActionNeighbor(v, Vector3.left, ToolAction.Dig);

    // Apply to block
    if (digDir == BlockDir.None)
      gardenBoard.ApplyAction(v, ToolAction.Dig);
    else
      gardenBoard.ApplyHalfDig(v, digDir);
  }

  private void ApplyFill(Vector3 v) {
    Instantiate(dirtCloud, v, Quaternion.identity, effectContainer);

    Block z = gardenBoard.GetBlock(v);

    Block f = gardenBoard.GetBlockNeighbor(v, Vector3.forward);
    Block r = gardenBoard.GetBlockNeighbor(v, Vector3.right);
    Block b = gardenBoard.GetBlockNeighbor(v, Vector3.back);
    Block l = gardenBoard.GetBlockNeighbor(v, Vector3.left);

    gardenBoard.ApplyAction(v, ToolAction.Fill);

    // Undo half for neighbors
    if (f != null && f.height == z.height && f.halfDir != BlockDir.None)
      gardenBoard.ApplyActionNeighbor(v, Vector3.forward, ToolAction.Fill);

    if (r != null && r.height == z.height && r.halfDir != BlockDir.None)
      gardenBoard.ApplyActionNeighbor(v, Vector3.right, ToolAction.Fill);

    if (b != null && b.height == z.height && b.halfDir != BlockDir.None)
      gardenBoard.ApplyActionNeighbor(v, Vector3.back, ToolAction.Fill);

    if (l != null && l.height == z.height && l.halfDir != BlockDir.None)
      gardenBoard.ApplyActionNeighbor(v, Vector3.left, ToolAction.Fill);
  }
}
