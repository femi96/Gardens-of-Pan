using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Monster {
  // Worm

  // Unit functions
  public override string GetName() {
    return "Worm";
  }

  public override float GetSize() {
    return 1f;
  }

  public override float GetWandRadius() {
    return 0.3f;
  }

  public override float GetHoverHeight() {
    return 0.25f;
  }

  // Monster functions
  public override bool CanVisit() {
    GardenBoard board = garden.GetBoard();
    return board.GetBlockTypeCount(BlockType.Dirt) >= 4f && garden.GetUnitTypeCount(typeof(Worm)) < 2;
  }

  public override bool CanJoin() {
    GardenBoard board = garden.GetBoard();
    return board.GetBlockTypeCount(BlockType.Dirt) >= 8f;
  }

  public override bool CanSpawn() {
    List<SpawnPoint> spawnPoints =  board.GetSpawnPoints();

    foreach (SpawnPoint spawn in spawnPoints) {
      if (spawn.GetBlock().GetBlockType() == BlockType.Dirt)
        return true;
    }

    return false;
  }

  public override SpawnPoint GetSpawn() {
    List<SpawnPoint> spawnPoints = garden.GetBoard().GetSpawnPoints();
    List<SpawnPoint> validSpawnPoints  =  new List<SpawnPoint>();

    foreach (SpawnPoint spawn in spawnPoints) {
      if (spawn.GetBlock().GetBlockType() == BlockType.Dirt)
        validSpawnPoints.Add(spawn);
    }

    int r = Random.Range(0, validSpawnPoints.Count);
    return validSpawnPoints[r];
  }

  public override float GetHappyExternal() {
    float happyFromBlocks = board.GetBlockTypeCount(BlockType.Dirt);
    happyFromBlocks = happyFromBlocks / 2f;
    happyFromBlocks = Mathf.Clamp(happyFromBlocks, 0f, 4f);

    float happyFromUnits = (garden.GetUnitTypeCount(typeof(Worm)) - 1);
    happyFromUnits = happyFromUnits / 1.5f;
    happyFromUnits = Mathf.Clamp(happyFromUnits, 0f, 2f);

    return happyFromBlocks + happyFromUnits;
  }
}
