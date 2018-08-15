using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class MonsterRestrictor {
  // AI controller evaluating if state is available

  // Returns bool for if the behavior state is available
  //  Takes this behavior and this monster as pararmeters
  public abstract bool Restrict(MonsterBehaviour thisBehaviour, Monster thisMonster);
}