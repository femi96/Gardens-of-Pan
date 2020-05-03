using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterAIGoalLeave : MonsterAIGoal {

  private int taskCount = 0;

  public MonsterAIGoalLeave() {}

  public override bool IsDone() {
    return taskCount == 2;
  }

  private Vector3 GetLeaveDest(Monster mon) {
    List<SpawnPoint> spawnPoints = mon.garden.GetBoard().GetSpawnPoints();
    Vector3 dest = spawnPoints[Random.Range(0, spawnPoints.Count)].GetPosition();
    return dest;
  }

  public override MonsterAITask Driver(Monster mon) {
    if (taskCount == 0) {
      taskCount += 1;
      Vector3 dest = GetLeaveDest(mon);
      return new MonsterAITaskGoTo(dest);
    }

    if (taskCount == 1) {
      taskCount += 1;
      return new MonsterAITaskLeave();
    }

    return new MonsterAITaskNone();
  }
}
