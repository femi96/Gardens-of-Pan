using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Monster {
  // Worm: Class of Worm

  public override float GetSize() {
    return 1f;
  }

  public override float GetWandRadius() {
    return 0.4f;
  }

  public override bool CanVisit(Garden garden) {
    return true;
  }

  public override bool CanSpawn(GardenBoard board) {
    List<SpawnPoint> spawnPoints =  board.GetSpawnPoints();

    foreach (SpawnPoint spawn in spawnPoints) {
      if (spawn.GetBlock().GetBlockType() == BlockType.Dirt) return true;
    }

    return false;
  }

  public override SpawnPoint GetSpawn(GardenBoard board) {
    List<SpawnPoint> spawnPoints =  board.GetSpawnPoints();
    List<SpawnPoint> validSpawnPoints  =  new List<SpawnPoint>();

    foreach (SpawnPoint spawn in spawnPoints) {
      if (spawn.GetBlock().GetBlockType() == BlockType.Dirt) validSpawnPoints.Add(spawn);
    }

    return validSpawnPoints[0];
  }
}
