using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterAIMotivationLeave : MonsterAIMotivation {

  private float time;
  private float timeToLeave = 60f;

  public MonsterAIMotivationLeave() {
    time = Time.time;
  }

  public override float GetPriority(Monster mon) {
    if (!mon.joined)
      return 0.5f * (Time.time - time) / timeToLeave;
    else
      return -1f;
  }

  public override MonsterAIGoal GenerateGoal(Monster mon) {
    return new MonsterAIGoalLeave();
  }
}
