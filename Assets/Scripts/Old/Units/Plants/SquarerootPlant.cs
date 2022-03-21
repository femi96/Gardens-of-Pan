// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class SquarerootPlant : Plant {
//   // Squareroot plant

//   [Header("Plant Parts")]
//   public GameObject produce;
//   public GameObject root;

//   // Unit functions
//   public override string GetName() {
//     return "Squareroot";
//   }

//   public override float GetSize() {
//     return 0.3f;
//   }

//   public override float GetWandRadius() {
//     return 0.15f;
//   }

//   public override float GetHoverHeight() {
//     return 0.3f;
//   }

//   // Plant functions
//   public override void PlantAwake() {
//     root.SetActive(false);
//   }

//   public override void PlantUpdate() {

//     // If not grown, grow
//     if (!grown)
//       Grow();

//     // If grown, create produce
//     if (grown) {
//       if (garden.TryAddUnit(produce, transform.position + 0.1f * Vector3.up + 0.02f * Random.onUnitSphere, root.transform.rotation))
//         Die();
//     }

//     if (dieTime > 5f + growTime * 0.1f)
//       Die();
//   }

//   public override float PlantRadius() {
//     return growTime * 0.2f / 15f;
//   }

//   // Plant Behavior when not grown
//   private void Grow() {
//     Block block = board.GetBlock(transform.position);
//     bool validSurface = block.height == 0;
//     bool toClose = IsToClose();

//     if (validSurface && !toClose) {
//       growTime += Time.deltaTime;
//       dieTime = 0;
//     } else {
//       dieTime += Time.deltaTime;
//       // Should change color based on surface type
//     }

//     // Grow based on growTime
//     if (growthStage == 0) {
//       if (growTime >= 0f) {
//         root.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
//         SetGrowthStage(1);
//       }
//     }

//     if (growthStage == 1) {
//       float rootSize = Mathf.Lerp(0.1f, 1f, (growTime - 0f) / 15f);
//       root.transform.localScale = new Vector3(rootSize, rootSize, rootSize);

//       if (growTime >= 20f) {
//         SetGrowthStage(2);
//       }
//     }
//   }

//   private void SetGrowthStage(int newStage) {
//     growthStage = newStage;

//     if (growthStage == 1) {
//       root.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
//     }

//     root.SetActive(growthStage >= 1);

//     grown = (growthStage >= 2);
//   }

//   // SAVING/LOADING plant
//   // =======================

//   public override void SetFromSave(UnitSave save) {
//     PlantSave p = (PlantSave)save;
//     base.SetFromSave(p);

//     root.transform.rotation = p.rotations[0];
//     SetGrowthStage(p.growthStage);
//   }

//   public override void SetPlantSave(PlantSave save) {
//     save.rotations = new SerializableQuaternion[1];
//     save.rotations[0] = root.transform.rotation;
//   }
// }
