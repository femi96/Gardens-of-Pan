using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockberryPlant : Plant {
  // Blockberry plant

  [Header("Plant Parts")]
  public GameObject produce;
  public GameObject trunk;
  public GameObject[] branch;
  public GameObject[] bush;
  public Transform[] producePoints;

  [Header("Blockberry Bush Fields")]
  public Produce[] activeProduce;
  public float[] activeProduceTime;

  public bool plantLoadedFromFile = false;
  public int[] produceToLoad;

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

  public override float GetHoverHeight() {
    return 0.3f;
  }

  // Plant functions
  public override void PlantAwake() {
    SetGrowthStage(0);

    activeProduce = new Produce[producePoints.Length];
    activeProduceTime = new float[producePoints.Length];

    for (int i = 0; i < producePoints.Length; i++) {
      activeProduceTime[i] = 0f;
    }
  }

  public override void PlantBehavior() {

    if (plantLoadedFromFile) {
      // Load produce
      for (int i = 0; i < produceToLoad.Length; i++) {
        activeProduce[i] = (Produce)garden.GetUnit(produceToLoad[i]);
      }

      plantLoadedFromFile = false;
    }

    if (!grown)
      Grow();

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

  // Plant Behavior when fully grown
  private void Grown() {
    CreateProduce();
    DropProduce();
  }

  private void CreateProduce() {
    produceTime += Time.deltaTime;

    if (produceTime < 8f)
      return;

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

  private void DropProduce() {
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
  private void Grow() {
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
        SetGrowthStage(1);
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
        SetGrowthStage(2);
      }
    }

    if (growthStage == 2) {
      float branchSize = Mathf.Lerp(0.1f, 1f, (growTime - 10f) / 10f);
      branch[0].transform.localScale = new Vector3(branchSize, branchSize, branchSize);
      branch[1].transform.localScale = new Vector3(branchSize, branchSize, branchSize);

      if (growTime >= 20f) {
        SetGrowthStage(3);
      }
    }

    if (growthStage == 3) {
      float bushSize = Mathf.Lerp(0.1f, 1f, (growTime - 20f) / 10f);
      bush[0].transform.localScale = new Vector3(bushSize, bushSize, bushSize);
      bush[1].transform.localScale = new Vector3(bushSize, bushSize, bushSize);

      if (growTime >= 30f) {
        SetGrowthStage(4);
      }
    }
  }

  private void SetGrowthStage(int newStage) {
    growthStage = newStage;

    if (growthStage == 1) {
      trunk.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    if (growthStage == 2) {
      branch[0].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
      branch[1].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    if (growthStage == 3) {
      bush[0].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
      bush[1].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    trunk.SetActive(growthStage >= 1);
    branch[0].SetActive(growthStage >= 2);
    branch[1].SetActive(growthStage >= 2);
    bush[0].SetActive(growthStage >= 3);
    bush[1].SetActive(growthStage >= 3);

    grown = (growthStage >= 4);
  }

  // SAVING/LOADING plant
  // =======================

  public override void SetFromSave(UnitSave save) {
    PlantSave p = (PlantSave)save;
    base.SetFromSave(p);
    activeProduceTime = p.produceTimes;

    plantLoadedFromFile = true;
    produceToLoad = p.produceIds;

    trunk.transform.rotation = p.rotations[0];
    branch[0].transform.rotation = p.rotations[1];
    branch[1].transform.rotation = p.rotations[2];
    bush[0].transform.rotation = p.rotations[3];
    bush[1].transform.rotation = p.rotations[4];
    SetGrowthStage(p.growthStage);
  }

  public override void SetPlantSave(PlantSave save) {
    save.produceTimes = activeProduceTime;

    save.produceIds = new int[activeProduce.Length];

    for (int i = 0; i < save.produceIds.Length; i++) {
      if (activeProduce[i] != null)
        save.produceIds[i] = activeProduce[i].GetID();
      else
        save.produceIds[i] = -1;
    }

    save.rotations = new SerializableQuaternion[5];
    save.rotations[0] = trunk.transform.rotation;
    save.rotations[1] = branch[0].transform.rotation;
    save.rotations[2] = branch[1].transform.rotation;
    save.rotations[3] = bush[0].transform.rotation;
    save.rotations[4] = bush[1].transform.rotation;
  }
}