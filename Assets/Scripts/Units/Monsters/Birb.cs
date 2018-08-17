using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birb : Monster {
  // Birb

  // Unit functions
  public override string GetName() {
    return "Birb";
  }

  public override float GetSize() {
    return 1f;
  }

  public override float GetWandRadius() {
    return 0.3f;
  }

  public override float GetHoverHeight() {
    return 0.35f;
  }

  // Monster functions
  public override bool CanVisit() {
    int blocks = board.GetBlockTypeCount(BlockType.Grassland)
                 + board.GetBlockTypeCount(BlockType.Wetland);
    return blocks >= 4 && garden.GetUnitTypeCount(typeof(Birb)) < 2;
  }

  public override bool CanOwn() {
    return board.GetBlockTypeCount(BlockType.Grassland) >= 8;
  }

  public override bool CanSpawn() {
    List<SpawnPoint> spawnPoints = board.GetSpawnPoints();

    foreach (SpawnPoint spawn in spawnPoints) {
      if (spawn.GetBlock().GetBlockType() == BlockType.Grassland)
        return true;
    }

    return false;
  }

  public override SpawnPoint GetSpawn() {
    List<SpawnPoint> spawnPoints = board.GetSpawnPoints();
    List<SpawnPoint> validSpawnPoints = new List<SpawnPoint>();

    foreach (SpawnPoint spawn in spawnPoints) {
      if (spawn.GetBlock().GetBlockType() == BlockType.Grassland)
        validSpawnPoints.Add(spawn);
    }

    int r = Random.Range(0, validSpawnPoints.Count);
    return validSpawnPoints[r];
  }

  public override float GetHappyExternal() {
    return 0f;
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
