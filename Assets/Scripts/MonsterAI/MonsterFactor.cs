using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterFactor : ScriptableObject {
  // MonsterFactor: MonsterAI component deciding state priority.


  // Returns factor value for state priority
  public abstract float Score(Monster mon);
}
