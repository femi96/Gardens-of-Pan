using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class MonsterFactor {
  // AI controller evaluating state priority

  // Returns factor value for the behavior state priority
  //  Takes this behavior and this monster as pararmeters
  public abstract float GetPriority(MonsterBehaviour thisBehaviour, Monster thisMonster);
}