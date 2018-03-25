using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterAction : ScriptableObject {
  // MonsterAction: MonsterAI component scripting monster actions.


  // Executes action, effecting garden.
  public abstract void Act(Monster mon);
}