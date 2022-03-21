// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Bomb : Monster {
//   // Bomb

//   // Unit functions
//   public override string GetName() {
//     return "Bomb";
//   }

//   public override float GetSize() {
//     return 1f;
//   }

//   public override float GetWandRadius() {
//     return 0.3f;
//   }

//   public override float GetHoverHeight() {
//     return 0.3f;
//   }

//   // Monster functions
//   public override bool CanVisit() {
//     GardenBoard board = garden.GetBoard();
//     float blocks = board.GetBlockTypeCount(BlockType.Sand);
//     return blocks >= 4f && garden.GetUnitTypeCount(typeof(Bomb)) < 2;
//   }

//   public override bool CanJoin() {
//     GardenBoard board = garden.GetBoard();
//     return board.GetBlockTypeCount(BlockType.Sand) >= 8f;
//   }

//   public override bool CanSpawn() {
//     List<SpawnPoint> spawnPoints = board.GetSpawnPoints();

//     foreach (SpawnPoint spawn in spawnPoints) {
//       if (spawn.GetBlock().GetBlockType() == BlockType.Sand)
//         return true;
//     }

//     return false;
//   }

//   public override SpawnPoint GetSpawn() {
//     List<SpawnPoint> spawnPoints = garden.GetBoard().GetSpawnPoints();
//     List<SpawnPoint> validSpawnPoints  =  new List<SpawnPoint>();

//     foreach (SpawnPoint spawn in spawnPoints) {
//       if (spawn.GetBlock().GetBlockType() == BlockType.Sand)
//         validSpawnPoints.Add(spawn);
//     }

//     int r = Random.Range(0, validSpawnPoints.Count);
//     return validSpawnPoints[r];
//   }

//   public override float GetHappyExternal() {
//     float happyFromBlocks = board.GetBlockTypeCount(BlockType.Sand)
//                             - board.GetBlockTypeCount(BlockType.Water)
//                             - board.GetBlockTypeCount(BlockType.Wet)
//                             - board.GetBlockTypeCount(BlockType.Mud);
//     happyFromBlocks = happyFromBlocks / 2f;
//     happyFromBlocks = Mathf.Clamp(happyFromBlocks, 0f, 4f);

//     float happyFromUnits = (garden.GetUnitTypeCount(typeof(Bomb)) - 1);
//     happyFromUnits = happyFromUnits / 1.5f;
//     happyFromUnits = Mathf.Clamp(happyFromUnits, 0f, 2f);

//     return happyFromBlocks + happyFromUnits;
//   }
// }
