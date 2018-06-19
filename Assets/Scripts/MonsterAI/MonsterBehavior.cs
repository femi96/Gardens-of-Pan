﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterBehavior {
  // AI controller holding AI behavior state with actions and factors

  public string behaviorName;

  // Collection variables:
  private Garden garden;
  private Monster monster;
  private List<MonsterAction> actions;
  private List<MonsterFactor> factors;

  private float behaviorTime;
  private int behaviorsSince;
  public float timeSinceLastEnd;

  // Creates MonsterBehavior with TODO
  public MonsterBehavior(string name, Garden g, Monster m, MonsterAction[] actions, MonsterFactor[] factors) {

    behaviorName = name;

    garden = g;
    monster = m;

    this.actions = new List<MonsterAction>();

    foreach (MonsterAction a in actions) {
      this.actions.Add(a);
    }

    this.factors = new List<MonsterFactor>();

    foreach (MonsterFactor f in factors) {
      this.factors.Add(f);
    }

    timeSinceLastEnd = 0;
  }

  // Starts state with initial fields and starts behaviors
  public void StartBehavior() {

    behaviorTime = 0f;
    behaviorsSince = -1;

    foreach (MonsterAction action in actions) {
      action.StartAction(this);
    }
  }

  // Ends state
  public void EndBehavior() {

    timeSinceLastEnd = 0;
    monster.BehaviorDone();
  }

  // Execute per frame state operations
  public void BehaviorUpdate() {

    behaviorTime += Time.deltaTime;
    Act();
  }

  // Execute all actions in this state
  private void Act() {

    foreach (MonsterAction action in actions) {
      action.Act();
    }
  }

  // Sum all factors, returning state priority
  public float GetFactorTotal() {

    float total = 0;

    foreach (MonsterFactor factor in factors) {
      total += factor.GetScore(this);
    }

    behaviorsSince += 1;
    return total;
  }

  // Returns state's monster's garden
  public Garden GetGarden() {
    return garden;
  }

  // Returns state's monster
  public Monster GetMonster() {
    return monster;
  }

  // Returns states since this was the current state
  public int GetBehaviorsSince() {
    return behaviorsSince;
  }

  // Returns time since this state started
  public float GetBehaviorTime() {
    return behaviorTime;
  }

  // Returns DateTime of when this behavior ended
  public float GetTimeSinceLastEnd() {
    return timeSinceLastEnd;
  }

  public override string ToString() {
    return behaviorName + "ing...";
  }
}
