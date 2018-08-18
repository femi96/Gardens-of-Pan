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

  public override float GetHappyExternal() {
    float happyFromBlocks = board.GetBlockTypeCount(BlockType.Dirt);
    happyFromBlocks = happyFromBlocks / 2f;
    happyFromBlocks = Mathf.Clamp(happyFromBlocks, 0f, 4f);

    float happyFromUnits = (garden.GetUnitTypeCount(typeof(Worm)) - 1);
    happyFromUnits = happyFromUnits / 1.5f;
    happyFromUnits = Mathf.Clamp(happyFromUnits, 0f, 2f);

    return happyFromBlocks + happyFromUnits;
  }

  public override MonsterBehaviour[] Behaviours() {

    List<MonsterBehaviour> behaviors = new List<MonsterBehaviour>();

    MonsterBehaviour wander = new MonsterBehaviour("Wander", this);
    wander.actions.Add(new ActionWander());
    wander.factors.Add(new FactorRepeat(1f));
    behaviors.Add(wander);

    MonsterBehaviour wait = new MonsterBehaviour("Wait", this);
    wait.actions.Add(new ActionTimeout(3f, 6f));
    wait.factors.Add(new FactorRepeat(1f));
    behaviors.Add(wait);

    MonsterBehaviour leave = new MonsterBehaviour("Leave", this);
    leave.actions.Add(new ActionLeave());
    leave.factors.Add(new FactorTimeout(10f, 30f));
    leave.restrictors.Add(new RestrictorWildOnly());
    leave.behaviourText = "Leaving...";
    behaviors.Add(leave);

    MonsterBehaviour join = new MonsterBehaviour("Join", this);
    join.actions.Add(new ActionJoin(2f));
    join.factors.Add(new FactorRepeat(10f));
    join.restrictors.Add(new RestrictorWildOnly());
    behaviors.Add(join);

    return behaviors.ToArray();
  }
}
