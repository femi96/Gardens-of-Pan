﻿// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class SquarerootSeed : Seed {
//   // Squareroot seed

//   public GameObject plant;

//   // Unit functions
//   public override string GetName() {
//     return "Squareroot Seed";
//   }

//   public override float GetSize() {
//     return 0.1f;
//   }

//   public override float GetWandRadius() {
//     return 0.1f;
//   }

//   public override float GetHoverHeight() {
//     return 0.1f;
//   }

//   // Seed functions
//   public override void SeedUpdate() {
//     Block block = board.GetBlock(transform.position);
//     bool validSurface = block.height == 0;

//     // If can plant, plant
//     if (timeActive > 5f && validSurface)
//       Plant();

//     // If to old, break
//     if (timeActive > 120f)
//       Break();
//   }

//   public override void Plant() {
//     bool planted = garden.TryAddUnit(plant, transform.position, Quaternion.identity);

//     if (planted)
//       Break();
//   }
// }
