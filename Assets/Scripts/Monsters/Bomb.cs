using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Monster {
  // Bomb

  public override string GetName() {
    return "Bomb";
  }

  public override float GetSize() {
    return 1f;
  }

  public override float GetWandRadius() {
    return 0.3f;
  }

  public override bool CanVisit(Garden garden) {
    GardenBoard board = garden.GetBoard();

    return board.GetBlockTypeCount(BlockType.Scorch) >= 4 && garden.GetUnitTypeCount(typeof(Bomb)) < 2;
  }

  public override bool CanOwn(Garden garden) {
    GardenBoard board = garden.GetBoard();

    return board.GetBlockTypeCount(BlockType.Scorch) >= 8;
  }

  public override bool CanSpawn(GardenBoard board) {
    List<SpawnPoint> spawnPoints =  board.GetSpawnPoints();

    foreach (SpawnPoint spawn in spawnPoints) {
      if (spawn.GetBlock().GetBlockType() == BlockType.Scorch)
        return true;
    }

    return false;
  }

  public override SpawnPoint GetSpawn(GardenBoard board) {
    List<SpawnPoint> spawnPoints =  board.GetSpawnPoints();
    List<SpawnPoint> validSpawnPoints  =  new List<SpawnPoint>();

    foreach (SpawnPoint spawn in spawnPoints) {
      if (spawn.GetBlock().GetBlockType() == BlockType.Scorch)
        validSpawnPoints.Add(spawn);
    }

    int r = Random.Range(0, validSpawnPoints.Count);
    return validSpawnPoints[r];
  }

  public override MonsterBehavior[] NormalBehaviors() {

    Garden g = GameObject.Find("Garden").GetComponent<Garden>();

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

    return uniqueBehaviors;
  }

  public override MonsterBehavior[] WildBehaviors() {

    Garden g = GameObject.Find("Garden").GetComponent<Garden>();
    MonsterBehavior[] wildBehaviors = new MonsterBehavior[] {

      new MonsterBehavior("Leave", g, this,
      new MonsterAction[] {
        new ActionLeave()
      },
      new MonsterFactor[] {
        new FactorTimeout(10f, 30f)
      }),

      new MonsterBehavior("Join", g, this,
      new MonsterAction[] {
        new ActionJoin(2f)
      },
      new MonsterFactor[] {
        new FactorRepeat(10f)
      }),
    };

    return wildBehaviors;
  }
}
