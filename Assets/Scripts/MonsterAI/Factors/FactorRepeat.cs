using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactorRepeat : MonsterFactor {
  // AI controller evaluating state priority
  //  Priority count since this was last state, times a factor

  private float m;

  public FactorRepeat(float multiplier) {
    m = multiplier;
  }

  public float GetPriority(MonsterBehaviour behaviour) {
    return behaviour.behavioursSince * m;
  }
}
