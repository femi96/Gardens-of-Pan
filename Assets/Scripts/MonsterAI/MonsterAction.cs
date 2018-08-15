using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterAction {
  // AI controller scripting monster action

  // Initializes action variables, called at the start of the behavior state
  public abstract void SetupAction(MonsterBehaviour behaviour);

  // Executes action, effecting garden
  public abstract void Act();
}