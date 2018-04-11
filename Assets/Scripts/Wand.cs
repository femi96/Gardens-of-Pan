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

  // Wand shape variables:
  private GameObject wandShape;
  private Transform[] wandShapePieces;

  // Movement variables:
  private float radius = 0.2f;
  private float speed = 4f;

  private GameObject targetUnit;

  private const int gLayer = 1 << 8;
  private const int wLayer = 1 << 9;

  // Timer variables:
  private float timeSinceMove;

  public GameObject hoverUI;


  // Unity MonoBehavior Functions:
  void Start() {

    // Setup wand shape
    wandShape = transform.Find("WandShape").gameObject;
    wandShapePieces = wandShape.GetComponentsInChildren<Transform>();
    wandShapePieces = wandShapePieces.Skip(1).ToArray();
  }

  void Update() {

    // Shape
    UpdateWandShape();

    // Move
    MoveWand();
    FollowUnit();

    // Tool

    UpdateTagUI();
  }

  // Updates wand shape based on units
  private void UpdateWandShape() {

    // Update shape scale based on targetUnit
    Vector3 newScale;

    if (targetUnit != null) {
      float r = targetUnit.GetComponent<Unit>().GetWandRadius();
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

    moveDirection = Vector3.Normalize(moveDirection);             // Don't normalize your inputs
    Vector3 moveDirectionF = moveDirection * z;                 // Project z onto forward direction vector
    Vector3 moveDirectionR = new Vector3(moveDirection.z, 0, -moveDirection.x); // Create right vector
    moveDirectionR *= x;                            // Project x onto right direction vector

    moveDirection = moveDirectionF + moveDirectionR;
    moveDirection *= spd;

    // Update movme timer
    timeSinceMove += Time.deltaTime;

    if (x != 0 || z != 0) {
      timeSinceMove = 0;
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

    // Only follow if not moving
    if (timeSinceMove > 0.25f) {

      // Find new unit to follow
      if (targetUnit == null) {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up * 2, -Vector3.up, out hit, 4)) {
          if (hit.transform.gameObject.GetComponent<Unit>() != null) {
            targetUnit = hit.transform.gameObject;
          }
        }

        // Or follow unit
      } else {
        float newX = targetUnit.transform.position.x - transform.position.x;
        float newZ = targetUnit.transform.position.z - transform.position.z;
        Vector3 newV = new Vector3(newX, 0, newZ);
        transform.position += newV;
      }
    } else {
      targetUnit = null;
    }
  }

  // Update UI for hovered unit's tag
  private void UpdateTagUI() {
    if (targetUnit != null) {
      hoverUI.GetComponent<Text>().text = targetUnit.GetComponent<Unit>().GetName();
      Vector3 screenPos = Camera.main.WorldToScreenPoint(targetUnit.transform.position + (Vector3.up * 0.5f));
      hoverUI.transform.position = screenPos;
    } else {
      hoverUI.GetComponent<Text>().text = "";
    }
  }
}
