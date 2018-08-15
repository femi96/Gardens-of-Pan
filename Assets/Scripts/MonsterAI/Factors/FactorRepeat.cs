using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FactorRepeat : MonsterFactor {
  // AI controller evaluating state priority
  //  Priority count since this was last state, times a factor

  private float m;

  public FactorRepeat(float multiplier) {
    m = multiplier;
  }

  public override float GetPriority(MonsterBehaviour behaviour, Monster monster) {
    return behaviour.behavioursSince * m;
  }
}
