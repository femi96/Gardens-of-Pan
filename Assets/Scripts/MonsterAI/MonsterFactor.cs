using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MonsterFactor {
  // AI controller evaluating state priority

  // Returns factor value for the behavior state priority
  float GetPriority(MonsterBehavior behavior);
}