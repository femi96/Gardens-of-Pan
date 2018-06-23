using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterBehavior {
  // AI controller holding AI behavior state with actions and factors

  public string behaviorName;

  // Collection variables:
  public Garden garden;
  public Monster monster;
  public List<MonsterAction> actions;
  public List<MonsterFactor> factors;
  public List<MonsterRestrictor> restrictors;

  public int behaviorsSince;
  public float behaviorTime;
  public float timeSinceLastEnd;

  // Creates MonsterBehavior with name name for monster rMonster
  public MonsterBehavior(string name, Monster rMonster) {

    behaviorName = name;

    garden = rMonster.garden;
    monster = rMonster;

    actions = new List<MonsterAction>();
    factors = new List<MonsterFactor>();
    restrictors = new List<MonsterRestrictor>();

    timeSinceLastEnd = 0;
  }

  // Starts state with initial fields and starts behaviors
  public void StartBehavior() {

    behaviorTime = 0f;
    behaviorsSince = -1;

    foreach (MonsterAction action in actions) {
      action.SetupAction(this);
    }
  }

  // Ends state
  public void EndBehavior() {

    timeSinceLastEnd = 0;
    monster.currentBehaviorDone = true;
  }

  // Called per frame, execute every action each frame
  public void BehaviorUpdate() {

    behaviorTime += Time.deltaTime;

    foreach (MonsterAction action in actions) {
      action.Act();
    }
  }

  // Get behavior priority, based on sum of all factors
  public float GetPriority() {

    float priority = 0;

    foreach (MonsterFactor factor in factors) {
      priority += factor.GetPriority(this);
    }

    return priority;
  }

  // Is behavior allowed to start
  public bool IsResticted() {
    bool restricted = false;

    foreach (MonsterRestrictor restrictor in restrictors) {
      restricted = restricted || restrictor.Restrict(this);
    }

    return restricted;
  }

  public override string ToString() {
    return behaviorName + "ing...";
  }
}
