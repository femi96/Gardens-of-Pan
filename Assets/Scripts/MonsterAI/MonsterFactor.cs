using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterFactor {
  // AI controller evaluating state priority

  // Returns factor value for the behavior state priority
  public abstract float GetPriority(MonsterBehaviour behaviour);
}