using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterRestrictor {
  // AI controller evaluating if state is available

  // Returns bool for if the behavior state is available
  public abstract bool Restrict(MonsterBehaviour behaviour);
}