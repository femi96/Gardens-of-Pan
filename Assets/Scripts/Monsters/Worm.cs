﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Monster {
  // Worm

  public override string GetName() {
    return "Earthworm";
  }

  public override float GetSize() {
    return 1f;
  }

  public override float GetWandRadius() {
    return 0.3f;
  }

  public override bool CanVisit(Garden garden) {
    GardenBoard board = garden.GetBoard();

    return board.GetBlockTypeCount(BlockType.Dirt) >= 4 && garden.GetUnitTypeCount(typeof(Worm)) < 2;
  }

  public override bool CanOwn(Garden garden) {
    GardenBoard board = garden.GetBoard();

    return board.GetBlockTypeCount(BlockType.Dirt) >= 8;
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

    int r = Random.Range(0, validSpawnPoints.Count);
    return validSpawnPoints[r];
  }

  public override void UpdateBehaviors() {

    Garden g = GameObject.Find("Garden").GetComponent<Garden>();
    List<MonsterBehavior> behaviors = new List<MonsterBehavior>();

    MonsterBehavior[] uniqueBehaviors = new MonsterBehavior[] {

      new MonsterBehavior("Wander", g, this,
      new MonsterAction[] {
        new ActionWander()
      },
      new MonsterFactor[] {
        new FactorRepeat(1f)
      }),

      new MonsterBehavior("Wait", g, this,
      new MonsterAction[] {
        new ActionTimeout(3f, 6f)
      },
      new MonsterFactor[] {
        new FactorRepeat(1f)
      }),
    };

    behaviors.AddRange(uniqueBehaviors);

    if (!IsOwned()) {

      MonsterBehavior[] wildBehaviors = WildBehaviors(this);
      behaviors.AddRange(wildBehaviors);
    }

    SetBehaviors(behaviors.ToArray());
  }
}
