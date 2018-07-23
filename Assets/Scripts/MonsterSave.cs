using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterSave : UnitSave {

  public bool owned = false;

  // public MonsterBehaviour currentBehaviour;
  // public MonsterBehaviour[] behaviours = new MonsterBehaviour[0];
  public bool currentBehaviourDone = true;

  // Creates MonsterSave for a given monster
  public MonsterSave(Monster m) : base(m) {
    owned = m.owned;
    // currentBehaviour = m.currentBehaviour;
    // behaviours = m.behaviours;
    currentBehaviourDone = m.currentBehaviourDone;
  }
}