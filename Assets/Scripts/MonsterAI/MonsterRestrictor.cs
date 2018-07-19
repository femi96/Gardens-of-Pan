using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MonsterRestrictor {
  // AI controller evaluating if state is available

  // Returns bool for if the behavior state is available
  bool Restrict(MonsterBehaviour behaviour);
}