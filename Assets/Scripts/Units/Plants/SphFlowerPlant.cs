using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphFlowerPlant : Plant {
  // Sph flower plant

  [Header("Plant Parts")]
  public GameObject produce;
  public GameObject seed;
  public GameObject stem;
  public GameObject flower;
  public GameObject leaves;

  // Unit functions
  public override string GetName() {
    return "Sph Flower";
  }

  public override float GetSize() {
    return 0.5f;
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
  }

  public override void PlantUpdate() {

    // If not grown, grow
    if (!grown)
      Grow();

    // Add grown behavior of flower looking at sun (or drifting towards w delta)

    // If to old, break
    if (timeActive > 120f) {
      dieTime += Time.deltaTime;
      // Should look old now
    }

    if (dieTime > 5f + growTime * 0.1f)
      Die();
  }

  public override float PlantRadius() {
    return growTime * 0.2f / 30f;
  }

  // Plant Behavior when not grown
  private void Grow() {
    Block block = board.GetBlock(transform.position);
    bool validSurface = block.height == 0;
    bool toClose = IsToClose();

    if (validSurface && !toClose) {
      growTime += Time.deltaTime;
      dieTime = 0;
    } else {
      dieTime += Time.deltaTime;
      // Should change color based on surface type
    }

    // Grow based on growTime
    if (growthStage == 0) {
      if (growTime >= 0f) {
        stem.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        SetGrowthStage(1);
      }
    }

    if (growthStage == 1) {
      float stemSize = Mathf.Lerp(0.1f, 1f, (growTime - 0f) / 20f);
      stem.transform.localScale = new Vector3(stemSize, stemSize, stemSize);

      if (growTime >= 10f) {
        leaves.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        SetGrowthStage(2);
      }
    }

    if (growthStage == 2) {
      float stemSize2 = Mathf.Lerp(0.1f, 1f, (growTime - 0f) / 20f);
      stem.transform.localScale = new Vector3(stemSize2, stemSize2, stemSize2);
      float leafSize = Mathf.Lerp(0.1f, 1f, (growTime - 10f) / 10f);
      leaves.transform.localScale = new Vector3(leafSize, leafSize, leafSize);

      if (growTime >= 20f) {
        flower.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        SetGrowthStage(3);
      }
    }

    if (growthStage == 3) {
      float flowerSize = Mathf.Lerp(0.1f, 1f, (growTime - 20f) / 10f);
      flower.transform.localScale = new Vector3(flowerSize, flowerSize, flowerSize);

      if (growTime >= 30f) {
        SetGrowthStage(4);
      }
    }
  }

  public override void Die() {
    if (grown) {
      garden.TryAddUnit(produce, transform.position + 0.1f * Vector3.up + 0.1f * Random.onUnitSphere, flower.transform.rotation);
      garden.TryAddUnit(seed, transform.position + 0.1f * Vector3.up + 0.1f * Random.onUnitSphere, flower.transform.rotation);
    }

    base.Die();
  }

  private void SetGrowthStage(int newStage) {
    growthStage = newStage;

    if (growthStage == 1) {
      stem.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    if (growthStage == 2) {
      leaves.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    if (growthStage == 3) {
      flower.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    stem.SetActive(growthStage >= 1);
    leaves.SetActive(growthStage >= 2);
    flower.SetActive(growthStage >= 3);

    grown = (growthStage >= 4);
  }

  // SAVING/LOADING plant
  // =======================

  public override void SetFromSave(UnitSave save) {
    PlantSave p = (PlantSave)save;
    base.SetFromSave(p);

    stem.transform.rotation = p.rotations[0];
    flower.transform.rotation = p.rotations[1];
    leaves.transform.rotation = p.rotations[2];
    SetGrowthStage(p.growthStage);
  }

  public override void SetPlantSave(PlantSave save) {
    save.rotations = new SerializableQuaternion[3];
    save.rotations[0] = stem.transform.rotation;
    save.rotations[1] = flower.transform.rotation;
    save.rotations[2] = leaves.transform.rotation;
  }
}
