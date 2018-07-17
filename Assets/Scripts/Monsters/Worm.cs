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
    return board.GetBlockTypeCount(BlockType.Dirt) >= 4 && garden.GetUnitTypeCount(typeof(Worm)) < 2;
  }

  public override bool CanOwn() {
    GardenBoard board = garden.GetBoard();
    return board.GetBlockTypeCount(BlockType.Dirt) >= 8;
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

  public override MonsterBehavior[] Behaviors() {

    List<MonsterBehavior> behaviors = new List<MonsterBehavior>();

    MonsterBehavior wander = new MonsterBehavior("Wander", this);
    wander.actions.Add(new ActionWander());
    wander.factors.Add(new FactorRepeat(1f));
    behaviors.Add(wander);

    MonsterBehavior wait = new MonsterBehavior("Wait", this);
    wait.actions.Add(new ActionTimeout(3f, 6f));
    wait.factors.Add(new FactorRepeat(1f));
    behaviors.Add(wait);

    MonsterBehavior leave = new MonsterBehavior("Leave", this);
    leave.actions.Add(new ActionLeave());
    leave.factors.Add(new FactorTimeout(10f, 30f));
    leave.restrictors.Add(new RestrictorWildOnly());
    behaviors.Add(leave);

    MonsterBehavior join = new MonsterBehavior("Join", this);
    join.actions.Add(new ActionJoin(2f));
    join.factors.Add(new FactorRepeat(10f));
    join.restrictors.Add(new RestrictorWildOnly());
    behaviors.Add(join);

    return behaviors.ToArray();
  }
}
