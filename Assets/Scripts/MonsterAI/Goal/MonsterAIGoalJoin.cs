using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterAIGoalJoin : MonsterAIGoal {

  private int taskCount = 0;

  public MonsterAIGoalJoin() {}

  public override bool IsDone() {
    return taskCount == 1;
  }

  public override MonsterAITask Driver(Monster mon) {
    if (taskCount == 0) {
      taskCount += 1;
      return new MonsterAITaskJoin();
    }

    return new MonsterAITaskNone();
  }
}
