using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Wand : MonoBehaviour {
  //  Controller that handles the player wand's inputs and behaviors

  // Assigned in Editor:
  public Garden garden;
  public GardenBoard gardenBoard;

  // Movement:
  private float radius = 0.2f;
  private float speed = 4f;

  private const int gLayer = 1 << 8;
  private const int wLayer = 1 << 9;

  private Unit targetUnit;

  // Timing:
  private float moveTime;
  private const float followCooldown = 0.25f;

  // Wand shape:
  private GameObject wandShape;
  private Transform[] wandShapePieces;

  // UI:
  public GameObject unitHoverUI;
  private Text unitHoverName;
  private Image unitHoverBackground;

  // All public variables are assigned in editor

  void Awake() {

    wandShape = transform.Find("WandShape").gameObject;
    wandShapePieces = wandShape.GetComponentsInChildren<Transform>();
    wandShapePieces = wandShapePieces.Skip(1).ToArray();

    unitHoverName = unitHoverUI.transform.Find("Name").GetComponent<Text>();
    unitHoverBackground = unitHoverUI.transform.Find("Background").GetComponent<Image>();
  }

  void Update() {

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

  // Move wand each frame
  private void MoveWand() {

    // Transform input direction based on camera forward
    float limit = (garden.gardenSize / 2f) - radius;
    float spd = speed * Time.deltaTime;

    Vector3 moveDirection = Camera.main.transform.forward;
    moveDirection.y = 0;

    float y;
    float x = Input.GetAxis("Horizontal");
    float z = Input.GetAxis("Vertical");

    moveDirection = Vector3.Normalize(moveDirection);   // Don't normalize your inputs
    Vector3 moveDirectionF = moveDirection * z;         // Project z onto forward direction vector
    Vector3 moveDirectionR = new Vector3(moveDirection.z, 0, -moveDirection.x); // Create right vector
    moveDirectionR *= x;                                // Project x onto right direction vector

    moveDirection = moveDirectionF + moveDirectionR;
    moveDirection *= spd;

    // Update move timer
    moveTime += Time.deltaTime;

    if (x != 0 || z != 0) {
      moveTime = 0;
    }

    // Apply move direction to transform
    transform.Translate(moveDirection.x, 0, moveDirection.z);

    // Limit movement based on garden size
    y = transform.position.y;
    x = transform.position.x;
    z = transform.position.z;

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
    }
  }

  // Update UI for hovered unit's tag
  private void UpdateTagUI() {
    if (targetUnit != null) {
      unitHoverUI.SetActive(true);
      unitHoverName.text = targetUnit.GetName();

      unitHoverBackground.color = Color.black;

      if (targetUnit is Monster) {
        Monster targetMon = (Monster)targetUnit;

        if (!targetMon.IsOwned())
          unitHoverBackground.color = Color.red;
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

      if (Physics.Raycast(piece.transform.position + Vector3.up * 0.5f, -Vector3.up, out wandHit, 2f, wLayer)) {
        collide.y = 0.05f - wandHit.distance + 0.55f;
        piece.transform.position += collide;
      } else if (Physics.Raycast(piece.transform.position + Vector3.up * 0.5f, -Vector3.up, out wandHit, 2f, gLayer)) {
        collide.y = 0.05f - wandHit.distance + 0.55f;
        piece.transform.position += collide;
      }
    }

    // Rotate wand shape
    wandShape.transform.Rotate(10 * Vector3.up * Time.deltaTime);
  }
}
