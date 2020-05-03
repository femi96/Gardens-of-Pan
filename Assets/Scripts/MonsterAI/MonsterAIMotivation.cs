using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterAIMotivation {
  // Get priority of this motivation based on agent and world factors
  public abstract float GetPriority(Monster mon);
  // Generates goal that aligns with this motivation
  public abstract MonsterAIGoal GenerateGoal(Monster mon);

  public override string ToString() {
    string val = this.GetType().Name;
    val = val.Substring(19, val.Length - 19);
    return val;
  }
}
