using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterSave : UnitSave {

  public bool owned = false;

  public bool currentBehaviourDone = true;
  public MonsterBehaviour[] behaviours = new MonsterBehaviour[0];

  public int currentBehaviourIndex = 0;

  // Creates MonsterSave for a given monster
  public MonsterSave(Monster m) : base(m) {
    owned = m.owned;
    behaviours = m.behaviours;
    currentBehaviourDone = m.currentBehaviourDone;

    for (int i = 0; i < m.behaviours.Length; i++) {
      if (m.currentBehaviour == m.behaviours[i])
        currentBehaviourIndex = i;
    }
  }
}