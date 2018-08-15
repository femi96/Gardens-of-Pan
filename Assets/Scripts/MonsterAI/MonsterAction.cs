using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class MonsterAction {
  // AI controller scripting monster action

  // Initializes action variables, called at the start of the behavior state
  //  Takes this behavior and this monster as pararmeters
  public abstract void SetupAction(MonsterBehaviour thisBehaviour, Monster thisMonster);

  // Executes action, effecting garden
  //  Takes this behavior and this monster as pararmeters
  public abstract void Act(MonsterBehaviour thisBehaviour, Monster thisMonster);
}