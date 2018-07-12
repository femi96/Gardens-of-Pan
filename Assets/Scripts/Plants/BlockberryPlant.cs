using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockberryPlant : Plant {
  // Blockberry plant

  public GameObject produce;

  [Header("Plant Parts")]
  public GameObject trunk;
  public GameObject[] branch;
  public GameObject[] bush;

  public Transform[] producePoints;
  public Produce[] activeProduce;
  public float[] activeProduceTime;

  private int growthStage = 0;

  private float dieTime = 0f;
  private float growTime = 0f;
  private float produceTime = 0f;

  // Unit functions
  public override string GetName() {
    return "Blockberry Bush";
  }

  public override float GetSize() {
    return 1f;
  }

  public override float GetWandRadius() {
    return 0.15f;
  }

  // Plant functions
  public override void PlantAwake() {
    trunk.SetActive(false);
    branch[0].SetActive(false);
    branch[1].SetActive(false);
    bush[0].SetActive(false);
    bush[1].SetActive(false);

    activeProduce = new Produce[producePoints.Length];
    activeProduceTime = new float[producePoints.Length];

    for (int i = 0; i < producePoints.Length; i++) {
      activeProduceTime[i] = 0f;
    }
  }

  public override void PlantBehavior() {

    // If not grown, grow
    if (!grown)
      NotGrown();

    // If grown, create produce
    if (grown)
      Grown();

    // If to old, break
    if (timeActive > 600f) {
      dieTime += Time.deltaTime;
      // Should look old now
    }

    if (dieTime > 5f + growTime * 0.1f)
      Die();
  }

  public override float PlantRadius() {
    if (grown)
      return 0.5f;

    return growTime * 0.5f / 30f;
  }

  // Plant Behavior when fully grown
  private void Grown() {

    // Create produce
    produceTime += Time.deltaTime;

    if (produceTime > 8f) {
      // Get random empty produce point
      int produceIndex = 0;
      List<int> emptyIndices = new List<int>();

      for (int i = 0; i < producePoints.Length; i++)
        if (activeProduce[i] == null)
          emptyIndices.Add(i);

      if (emptyIndices.Count > 0)
        produceIndex = emptyIndices[Random.Range(0, emptyIndices.Count)];

      // If empty, add produce
      if (activeProduce[produceIndex] == null) {
        Transform point = producePoints[produceIndex];

        if (garden.TryAddUnit(produce, point.position, point.rotation)) {
          activeProduce[produceIndex] = (Produce)garden.GetLastUnit();
          activeProduce[produceIndex].held = true;
          activeProduceTime[produceIndex] = 0f;
          produceTime -= 8f;
        } else {
          produceTime -= 8f;
        }
      } else {
        produceTime -= 4f;
      }
    }

    // Drop produce
    for (int i = 0; i < producePoints.Length; i++) {
      activeProduceTime[i] += Time.deltaTime;

      if (activeProduceTime[i] >= 60f) {
        activeProduceTime[i] -= 60f;

        if (activeProduce[i] != null) {
          activeProduce[i].held = false;
          activeProduce[i] = null;
        }
      }
    }
  }

  // Plant Behavior when not grown
  private void NotGrown() {
    BlockType surfaceType = board.GetBlock(transform.position).GetBlockType();
    bool validSurface = BlockTypes.InGroup(surfaceType, BlockTypes.DepthGround);
    bool toClose = IsToClose();

    if (validSurface && !toClose) {
      growTime += Time.deltaTime;
      dieTime = 0;
    } else {
      dieTime += Time.deltaTime;
      // Should look bad based on surface type
    }

    // Grow based on growTime
    if (growthStage == 0) {
      if (growTime >= 0f) {
        trunk.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        trunk.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        trunk.SetActive(true);
        growthStage = 1;
      }
    }

    if (growthStage == 1) {
      float trunkSize = Mathf.Lerp(0.1f, 1f, (growTime - 0f) / 10f);
      trunk.transform.localScale = new Vector3(trunkSize, trunkSize, trunkSize);

      if (growTime >= 10f) {
        float rot0 = Random.Range(0f, 180f);
        float rot1 = rot0 + Random.Range(120f, 240f);
        branch[0].transform.rotation = Quaternion.Euler(0, rot0, 0);
        branch[1].transform.rotation = Quaternion.Euler(0, rot1, 0);
        branch[0].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        branch[1].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        branch[0].SetActive(true);
        branch[1].SetActive(true);
        growthStage = 2;
      }
    }

    if (growthStage == 2) {
      float branchSize = Mathf.Lerp(0.1f, 1f, (growTime - 10f) / 10f);
      branch[0].transform.localScale = new Vector3(branchSize, branchSize, branchSize);
      branch[1].transform.localScale = new Vector3(branchSize, branchSize, branchSize);

      if (growTime >= 20f) {
        bush[0].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        bush[1].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        bush[0].SetActive(true);
        bush[1].SetActive(true);
        growthStage = 3;
      }
    }

    if (growthStage == 3) {
      float bushSize = Mathf.Lerp(0.1f, 1f, (growTime - 20f) / 10f);
      bush[0].transform.localScale = new Vector3(bushSize, bushSize, bushSize);
      bush[1].transform.localScale = new Vector3(bushSize, bushSize, bushSize);

      if (growTime >= 30f) {
        grown = true;
      }
    }
  }

  public override void Die() {
    // Drop produce on death
    for (int i = 0; i < activeProduce.Length; i++) {
      if (activeProduce[i] != null) {
        activeProduce[i].held = false;
        activeProduce[i] = null;
      }
    }

    base.Die();
  }
}