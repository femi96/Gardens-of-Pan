﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictorWildOnly : MonsterRestrictor {
  // AI controller evaluating if state is available
  //  Only available if monster is not owned

  public RestrictorWildOnly() {}

  public bool Restrict(MonsterBehavior behavior) {
    return behavior.monster.owned;
  }
}