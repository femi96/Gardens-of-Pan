using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Wand : MonoBehaviour {
	// Wand: Controller that handles the player wand
	

	// Assigned in Editor:
	public Garden garden;
	public Chunk chunk;

	// Wand shape variables:
	[Header("Wand Shape")]
	public GameObject wandShape;
	private Transform[] wandShapePieces;

	// Movement variables:
	[Header("Movement")]
	public float radius = 0.2f;
	public float speed = 4f;

	public GameObject targetUnit;

	private int gLayer = 1 << 8;
	private int wLayer = 1 << 9;

	// Tools variables:
	[Header("Tools")]
	public Text toolText;
	public Text swapLText;
	public Text swapRText;

	public GameObject act1UI;
	public Text act1Text;
	public GameObject act2UI;
	public Text act2Text;

	private string[] tools;
	private int toolIndex;

	private string tool1;
	private string tool2;

	// Effect variables:
	[Header("Effects")]
	public Transform effectContainer;

	public GameObject dirtCloud;
	public GameObject grassCloud;
	public GameObject sandCloud;

	// Timer variables:
	private float timeSinceMove;
	private float timeSinceSwap;
	private float timeSinceAction;

	private float timeAction = 0.1f;


	// Unity MonoBehavior Functions:
	void Start() {

		// Setup wand shape
		wandShapePieces = wandShape.GetComponentsInChildren<Transform>();
		wandShapePieces = wandShapePieces.Skip(1).ToArray(); 

		// Setup tools
		toolIndex = 0;
		tools = new string[]{"Shovel", "Grass Seed", "Sand Bag"};
	}

	void Update() {

		// Shape
		UpdateWandShape();

		// Move
		MoveWand();
		FollowUnit();

		// Tool
		ToolSwap();
		ToolAction();
		if(timeSinceAction > timeAction) { UpdateToolStrings(); }
	}

	// Updates wand shape based on units
	private void UpdateWandShape() {

		// Update shape scale based on targetUnit
		Vector3 newScale;
		if(targetUnit != null) {
			float r = targetUnit.GetComponent<Unit>().wandRadius;
			newScale = new Vector3(r*2, 1, r*2);
		} else {
			newScale = Vector3.one;
		}
		wandShape.transform.localScale = newScale;

		// Update each shape piece height to match ground
		foreach(Transform piece in wandShapePieces) {
			Vector3 collide = new Vector3(0, 0, 0);
			RaycastHit wandHit;
			if(Physics.Raycast(piece.transform.position + Vector3.up*0.5f, -Vector3.up, out wandHit, 2f, wLayer)) {
				collide.y = 0.05f - wandHit.distance + 0.55f;
				piece.transform.position += collide;
			} else if(Physics.Raycast(piece.transform.position + Vector3.up*0.5f, -Vector3.up, out wandHit, 2f, gLayer)) {
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

		moveDirection = Vector3.Normalize(moveDirection);							// Don't normalize your inputs
		Vector3 moveDirectionF = moveDirection * z;									// Project z onto forward direction vector
		Vector3 moveDirectionR = new Vector3(moveDirection.z, 0, -moveDirection.x);	// Create right vector
		moveDirectionR *= x;														// Project x onto right direction vector
		
		moveDirection = moveDirectionF + moveDirectionR;
		moveDirection *= spd;

		// Update movme timer
		timeSinceMove += Time.deltaTime;
		if(x != 0 || z != 0) {
			timeSinceMove = 0;
		}

		// Apply move direction to transform
		transform.Translate(moveDirection.x, 0, moveDirection.z);

		// Limit movement based on garden size
		y = transform.position.y;
		x = transform.position.x;
		z = transform.position.z;
		if(x < -limit) { transform.position = new Vector3(-limit, y, z); }
		if(x > limit) { transform.position = new Vector3(limit, y, z); }
		x = transform.position.x;

		if(z < -limit) { transform.position = new Vector3(x, y, -limit); }
		if(z > limit) { transform.position = new Vector3(x, y, limit); }
		z = transform.position.z;
	}

	// If not moving, follow a unit
	private void FollowUnit() {

		// Only follow if not moving
		if(timeSinceMove > 0.25f) {

			// Find new unit to follow
			if(targetUnit == null) {
				RaycastHit hit;
				if(Physics.Raycast(transform.position + Vector3.up*2, -Vector3.up, out hit, 4)) {
					if(hit.transform.gameObject.GetComponent<Unit>() != null) {
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

	// Change tool index on input
	private void ToolSwap() {
		timeSinceSwap += Time.deltaTime;
		if(timeSinceSwap > 0.25f && timeSinceAction > timeAction) {
			if(Input.GetAxis("SwapLeft") > 0) { toolIndex -= 1; timeSinceSwap = 0; }
			if(Input.GetAxis("SwapRight") > 0) { toolIndex += 1; timeSinceSwap = 0; }
			if(toolIndex >= tools.Length) { toolIndex = 0; }
			if(toolIndex < 0) { toolIndex = tools.Length - 1; }
		}
	}

	// Act if tool input
	private void ToolAction() {
		timeSinceAction += Time.deltaTime;
		if(timeSinceAction > timeAction) {
			int i = 0;
			if(Input.GetAxis("Click2") > 0) { i = 2; timeSinceAction = 0; }
			if(Input.GetAxis("Click1") > 0) { i = 1; timeSinceAction = 0; }
			HandleAction(i);
		}
	}

	// Apply inpute to effect garden
	private void HandleAction(int input) {

		// Get tool action from input
		string t = "";
		if(input == 1) { t = tool1; }
		if(input == 2) { t = tool2; }
		if(t == "") { return; }

		// Apply tool action to appropriate effect
		Vector3 v = transform.position;
		BlockType gotType = chunk.GetType(v);
		switch(t) {
			case "Dig":
				if(gotType == BlockType.Dirt || gotType == BlockType.Grass || gotType == BlockType.Sand)
					chunk.SetType(v, BlockType.Water);
					Instantiate(dirtCloud, transform.position, Quaternion.identity, effectContainer);
				break;
			case "Flatten":
				if(gotType == BlockType.Rough)
					chunk.SetType(v, BlockType.Dirt);
					Instantiate(dirtCloud, transform.position, Quaternion.identity, effectContainer);
				break;
			case "Fill":
				if(gotType == BlockType.Water)
					chunk.SetType(v, BlockType.Dirt);
				break;
			case "Plant Grass":
				if(gotType == BlockType.Dirt || gotType == BlockType.Sand)
					chunk.SetType(v, BlockType.Grass);
					Instantiate(grassCloud, transform.position, Quaternion.identity, effectContainer);
				break;
			case "Remove":
				if(gotType == BlockType.Grass || gotType == BlockType.Sand)
					chunk.SetType(v, BlockType.Dirt);
				break;
			case "Pour Sand":
				if(gotType == BlockType.Dirt || gotType == BlockType.Grass)
					chunk.SetType(v, BlockType.Sand);
					Instantiate(sandCloud, transform.position, Quaternion.identity, effectContainer);
				break;
		}
	}

	// Update UI and strings based on index
	private void UpdateToolStrings() {

		// Update index, left, and right
		int i;
		i = toolIndex - 1;
		if(i < 0) { i = tools.Length - 1; }
		swapLText.text = tools[i];

		i = toolIndex + 1;
		if(i == tools.Length) { i = 0; }
		swapRText.text = tools[i];

		i = toolIndex;
		toolText.text = tools[i];

		// Update tool based on block differences
		BlockType t;
		switch(tools[i]) {
			case "Shovel":
				tool1 = "Dig";
				tool2 = "Fill";
				t = chunk.GetType(transform.position);
				if(t == BlockType.Rough) { tool1 = "Flatten"; tool2 = ""; }
				break;
			case "Grass Seed":
				tool1 = "Plant Grass";
				tool2 = "Remove";
				t = chunk.GetType(transform.position);
				if(t == BlockType.Rough || t == BlockType.Water) { tool1 = ""; tool2 = ""; }
				break;
			case "Sand Bag":
				tool1 = "Pour Sand";
				tool2 = "Remove";
				t = chunk.GetType(transform.position);
				if(t == BlockType.Rough || t == BlockType.Water) { tool1 = ""; tool2 = ""; }
				break;
		}

		// Enable UI if needs to display a tool action
		act1UI.SetActive(true);
		act1Text.text = tool1;
		act2UI.SetActive(true);
		act2Text.text = tool2;

		if(tool1 == "") { act1Text.text = "None"; act1UI.SetActive(false); }
		if(tool2 == "") { act2Text.text = "None"; act2UI.SetActive(false); }
	}
}
