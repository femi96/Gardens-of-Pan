using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MonsterAction {
  // AI controller scripting monster action

  // Initializes action variables, called at the start of the behavior state
  void SetupAction(MonsterBehaviour behaviour);

  // Executes action, effecting garden
  void Act();
}