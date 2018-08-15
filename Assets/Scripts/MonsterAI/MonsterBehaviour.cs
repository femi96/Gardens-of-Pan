using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterBehaviour {
  // AI controller holding AI behaviour state with actions and factors

  public string behaviourName;
  public string behaviourText;

  // Collection variables:
  public List<MonsterAction> actions;
  public List<MonsterFactor> factors;
  public List<MonsterRestrictor> restrictors;

  public int behavioursSince;
  public float behaviourTime;
  public float timeSinceLastEnd;

  // Creates MonsterBehaviour with name name for monster rMonster
  public MonsterBehaviour(string name, Monster rMonster) {

    behaviourName = name;
    behaviourText = name + "ing...";

    actions = new List<MonsterAction>();
    factors = new List<MonsterFactor>();
    restrictors = new List<MonsterRestrictor>();

    timeSinceLastEnd = 0;
  }

  // Starts state with initial fields and starts behaviours
  public void StartBehaviour(Monster thisMonster) {

    behaviourTime = 0f;
    behavioursSince = -1;

    foreach (MonsterAction action in actions) {
      action.SetupAction(this, thisMonster);
    }
  }

  // Called per frame, execute every action each frame
  public void BehaviourUpdate(Monster thisMonster) {

    behaviourTime += Time.deltaTime;

    foreach (MonsterAction action in actions) {
      action.Act(this, thisMonster);
    }
  }

  // Get behaviour priority, based on sum of all factors
  public float GetPriority(Monster thisMonster) {

    float priority = 0;

    foreach (MonsterFactor factor in factors) {
      priority += factor.GetPriority(this, thisMonster);
    }

    return priority;
  }

  // Is behaviour allowed to start
  public bool IsResticted(Monster thisMonster) {
    bool restricted = false;

    foreach (MonsterRestrictor restrictor in restrictors) {
      restricted = restricted || restrictor.Restrict(this, thisMonster);
    }

    return restricted;
  }

  public override string ToString() {
    return behaviourText;
  }
}
