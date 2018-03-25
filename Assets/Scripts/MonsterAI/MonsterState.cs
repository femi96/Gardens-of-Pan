using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MonsterAI/MonsterState")]
public class MonsterState : ScriptableObject {
  // MonsterState: MonsterAI component holding AI state actions and factors.


  // Collection variables:
  public MonsterAction[] actions;
  public MonsterFactor[] factors;


  // Called each update to run state
  public void UpdateState(Monster mon) {
    DoActions(mon);
  }

  // Execute all actions in this state
  private void DoActions(Monster mon) {
    foreach (MonsterAction action in actions) {
      action.Act(mon);
    }
  }

  // Sum all factors, returning state priority
  public float GetScore(Monster mon) {
    float total = 0;

    foreach (MonsterFactor factor in factors) {
      total += factor.Score(mon);
    }

    return total;
  }
}
