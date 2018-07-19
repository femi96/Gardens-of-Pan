using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterBehaviour {
  // AI controller holding AI behaviour state with actions and factors

  public string behaviourName;
  public string behaviourString;

  // Collection variables:
  public Garden garden;
  public Monster monster;
  public List<MonsterAction> actions;
  public List<MonsterFactor> factors;
  public List<MonsterRestrictor> restrictors;

  public int behavioursSince;
  public float behaviourTime;
  public float timeSinceLastEnd;

  // Creates MonsterBehaviour with name name for monster rMonster
  public MonsterBehaviour(string name, Monster rMonster) {

    behaviourName = name;
    behaviourString = name;

    garden = rMonster.garden;
    monster = rMonster;

    actions = new List<MonsterAction>();
    factors = new List<MonsterFactor>();
    restrictors = new List<MonsterRestrictor>();

    timeSinceLastEnd = 0;
  }

  // Starts state with initial fields and starts behaviours
  public void StartBehaviour() {

    behaviourTime = 0f;
    behavioursSince = -1;

    foreach (MonsterAction action in actions) {
      action.SetupAction(this);
    }
  }

  // Ends state
  public void EndBehaviour() {

    timeSinceLastEnd = 0;
    monster.currentBehaviourDone = true;
  }

  // Called per frame, execute every action each frame
  public void BehaviourUpdate() {

    behaviourTime += Time.deltaTime;

    foreach (MonsterAction action in actions) {
      action.Act();
    }
  }

  // Get behaviour priority, based on sum of all factors
  public float GetPriority() {

    float priority = 0;

    foreach (MonsterFactor factor in factors) {
      priority += factor.GetPriority(this);
    }

    return priority;
  }

  // Is behaviour allowed to start
  public bool IsResticted() {
    bool restricted = false;

    foreach (MonsterRestrictor restrictor in restrictors) {
      restricted = restricted || restrictor.Restrict(this);
    }

    return restricted;
  }

  public override string ToString() {
    return behaviourString;
  }
}
