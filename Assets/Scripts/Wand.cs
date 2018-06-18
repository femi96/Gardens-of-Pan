using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;

public class Wand : MonoBehaviour {
  //  Controller that handles the player wand's behaviors:
  //    MOVEMENT
  //    SCREENSHOT

  // Assigned in Editor:
  public Garden garden;
  public GardenBoard gardenBoard;

  // Movement:
  private float radius = 0.2f;
  private float speed = 4f;

  private Unit targetUnit;

  // Timing:
  private float moveTime = 0f;
  private const float followCooldown = 0.25f;

  // Wand shape:
  private GameObject wandShape;
  private Transform[] wandShapePieces;

  // UI hover:
  public GameObject unitHoverUI;
  private Text unitHoverName;
  private Text unitHoverBehavior;
  private Image unitHoverBackground;

  private readonly Color colorHoverDefault = new Color(0f, 0f, 0f, 0.7f);
  private readonly Color colorHoverWild = new Color(0.5f, 0f, 0f, 0.7f);

  // UI screenshot:
  public GameObject screenshotUI;
  private Text screenshotText;
  private float screenshotTime = 10f;
  private const float screenshotCooldown = 3f;

  // All public variables are assigned in editor

  void Awake() {

    wandShape = transform.Find("WandShape").gameObject;
    wandShapePieces = wandShape.GetComponentsInChildren<Transform>();
    wandShapePieces = wandShapePieces.Skip(1).ToArray();

    unitHoverName = unitHoverUI.transform.Find("Name").GetComponent<Text>();
    unitHoverBehavior = unitHoverUI.transform.Find("Behavior").GetComponent<Text>();
    unitHoverBackground = unitHoverUI.transform.Find("Background").GetComponent<Image>();

    screenshotText = screenshotUI.transform.Find("Text").GetComponent<Text>();
  }

  void Update() {

    screenshotTime += Time.deltaTime;
    ScreenCapture();

    moveTime += Time.deltaTime;
    MoveWand();

    // Only follow if not moving
    if (moveTime > followCooldown) {
      FollowUnit();
    } else {
      targetUnit = null;
    }

    UpdateTagUI();
    UpdateWandShape();
  }

  // Take a screenshot and save it
  private void ScreenCapture() {

    if (Input.GetButtonDown(InputConstants.ScreenCapture)) {
      string filePath = Application.dataPath + "/Screenshots/";
      string fileName = "Screenshot_" + garden.gardenName + System.DateTime.Now.ToString("_yyyy-MM-dd-HH-mm-ss") + ".png";

      if (!Directory.Exists(filePath))
        Directory.CreateDirectory(filePath);

      screenshotText.text = "Screenshot saved as " + fileName;
      screenshotTime = 0f;
      UnityEngine.ScreenCapture.CaptureScreenshot(filePath + fileName);
    }

    screenshotUI.SetActive(screenshotTime <= screenshotCooldown && screenshotTime > 0.05f);
  }

  // Move wand each frame
  private void MoveWand() {

    // Transform input direction based on camera forward
    float spd = speed * Time.deltaTime;

    Vector3 moveDirection = Camera.main.transform.forward;
    moveDirection.y = 0;

    float x = Input.GetAxis(InputConstants.WandX);
    float z = Input.GetAxis(InputConstants.WandZ);

    moveDirection = Vector3.Normalize(moveDirection);   // Don't normalize your inputs
    Vector3 moveDirectionF = moveDirection * z;         // Project z onto forward direction vector
    Vector3 moveDirectionR = new Vector3(moveDirection.z, 0, -moveDirection.x); // Create right vector
    moveDirectionR *= x;                                // Project x onto right direction vector

    moveDirection = moveDirectionF + moveDirectionR;
    moveDirection *= spd;

    // Update move timer
    if (x != 0 || z != 0) {
      moveTime = 0;
    }

    // Apply move direction to transform
    transform.Translate(moveDirection.x, 0, moveDirection.z);

    // Limit movement
    LimitWandPosition();
  }

  // Limit movement based on garden size
  private void LimitWandPosition() {

    float limit = (garden.gardenSize / 2f) - radius;

    float x = transform.position.x;
    float y = transform.position.y;
    float z = transform.position.z;

    if (x < -limit) { transform.position = new Vector3(-limit, y, z); }

    if (x > limit) { transform.position = new Vector3(limit, y, z); }

    x = transform.position.x;

    if (z < -limit) { transform.position = new Vector3(x, y, -limit); }

    if (z > limit) { transform.position = new Vector3(x, y, limit); }

    z = transform.position.z;

  }

  // If not moving, follow a unit
  private void FollowUnit() {

    // Find new unit to follow
    if (targetUnit == null) {

      RaycastHit hit;

      if (Physics.Raycast(transform.position + Vector3.up * 2, -Vector3.up, out hit, 4)) {
        if (hit.transform.gameObject.GetComponent<Unit>() != null) {
          targetUnit = hit.transform.gameObject.GetComponent<Unit>();
        }
      }

      // Or follow unit
    } else {

      float deltaX = targetUnit.transform.position.x - transform.position.x;
      float deltaZ = targetUnit.transform.position.z - transform.position.z;
      Vector3 deltaV = new Vector3(deltaX, 0, deltaZ);
      transform.position += deltaV;

      LimitWandPosition();
    }
  }

  // Update UI for hovered unit's tag
  private void UpdateTagUI() {
    if (targetUnit != null) {
      unitHoverUI.SetActive(true);
      unitHoverName.text = targetUnit.GetName();
      unitHoverBehavior.text = "";

      unitHoverBackground.color = colorHoverDefault;

      if (targetUnit is Monster) {
        Monster targetMon = (Monster)targetUnit;
        unitHoverBehavior.text = targetMon.currentBehavior.ToString();

        if (!targetMon.IsOwned())
          unitHoverBackground.color = colorHoverWild;
      }

      Vector3 screenPos = Camera.main.WorldToScreenPoint(targetUnit.transform.position + (Vector3.up * 0.5f));
      unitHoverUI.transform.position = screenPos;
    } else {
      unitHoverUI.SetActive(false);
    }
  }

  // Updates wand shape based on units
  private void UpdateWandShape() {

    // Update shape scale based on targetUnit
    Vector3 newScale;

    if (targetUnit != null) {
      float r = targetUnit.GetWandRadius();
      newScale = new Vector3(r * 2, 1, r * 2);
    } else {
      newScale = Vector3.one;
    }

    wandShape.transform.localScale = newScale;

    // Update each shape piece height to match ground
    foreach (Transform piece in wandShapePieces) {
      Vector3 collide = new Vector3(0, 0, 0);
      RaycastHit wandHit;

      if (Physics.Raycast(piece.transform.position + Vector3.up * 0.5f, -Vector3.up, out wandHit, 2f, LayerConstants.WaterLayer)) {
        collide.y = 0.05f - wandHit.distance + 0.55f;
        piece.transform.position += collide;
      } else if (Physics.Raycast(piece.transform.position + Vector3.up * 0.5f, -Vector3.up, out wandHit, 2f, LayerConstants.GroundLayer)) {
        collide.y = 0.05f - wandHit.distance + 0.55f;
        piece.transform.position += collide;
      }
    }

    // Rotate wand shape
    wandShape.transform.Rotate(10 * Vector3.up * Time.deltaTime);
  }
}
