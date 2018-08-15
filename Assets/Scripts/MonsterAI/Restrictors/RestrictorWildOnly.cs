using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RestrictorWildOnly : MonsterRestrictor {
  // AI controller evaluating if state is available
  //  Only available if monster is not owned

  public RestrictorWildOnly() {}

  public override bool Restrict(MonsterBehaviour behaviour, Monster monster) {
    return monster.owned;
  }
}