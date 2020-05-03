using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterAIMotivationIdle : MonsterAIMotivation {

  public override float GetPriority(Monster mon) {
    return 0.5f;
  }

  public override MonsterAIGoal GenerateGoal(Monster mon) {
    if (Random.Range(0f, 1f) > 0.5f)
      return new MonsterAIGoalWander(4);
    else
      return new MonsterAIGoalWander(3);
  }
}
