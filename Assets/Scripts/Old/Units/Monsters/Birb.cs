// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Birb : Monster {
//   // Birb

//   // Unit functions
//   public override string GetName() {
//     return "Birb";
//   }

//   public override float GetSize() {
//     return 1f;
//   }

//   public override float GetWandRadius() {
//     return 0.3f;
//   }

//   public override float GetHoverHeight() {
//     return 0.35f;
//   }

//   // Monster functions
//   public override bool CanVisit() {
//     float blocks = board.GetBlockTypeCount(BlockType.Grass)
//                    + board.GetBlockTypeCount(BlockType.Wet);
//     return blocks >= 4f && garden.GetUnitTypeCount(typeof(Birb)) < 2;
//   }

//   public override bool CanJoin() {
//     float blocks = board.GetBlockTypeCount(BlockType.Grass)
//                    + board.GetBlockTypeCount(BlockType.Wet);
//     return blocks >= 8f;
//   }

//   public override bool CanSpawn() {
//     List<SpawnPoint> spawnPoints = board.GetSpawnPoints();

//     foreach (SpawnPoint spawn in spawnPoints) {
//       if (spawn.GetBlock().GetBlockType() == BlockType.Grass)
//         return true;
//     }

//     return false;
//   }

//   public override SpawnPoint GetSpawn() {
//     List<SpawnPoint> spawnPoints = board.GetSpawnPoints();
//     List<SpawnPoint> validSpawnPoints = new List<SpawnPoint>();

//     foreach (SpawnPoint spawn in spawnPoints) {
//       if (spawn.GetBlock().GetBlockType() == BlockType.Grass)
//         validSpawnPoints.Add(spawn);
//     }

//     int r = Random.Range(0, validSpawnPoints.Count);
//     return validSpawnPoints[r];
//   }

//   public override float GetHappyExternal() {
//     float happyFromBlocks = board.GetBlockTypeCount(BlockType.Grass);
//     happyFromBlocks = happyFromBlocks / 2f;
//     happyFromBlocks = Mathf.Clamp(happyFromBlocks, 0f, 4f);

//     float happyFromUnits = (garden.GetUnitTypeCount(typeof(Birb)) - 1);
//     happyFromUnits = happyFromUnits / 1.5f;
//     happyFromUnits = Mathf.Clamp(happyFromUnits, 0f, 2f);

//     return happyFromBlocks + happyFromUnits;
//   }
// }
