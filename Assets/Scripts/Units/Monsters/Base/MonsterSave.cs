using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterSave : UnitSave {

  public bool joined = false;

  public bool hasGoal = false;
  public MonsterAIGoal goal;

  public bool hasTask = false;
  public MonsterAITask task;

  // Creates MonsterSave for a given monster
  public MonsterSave(Monster m) : base(m) {
    joined = m.joined;

    hasGoal = m.hasGoal;
    goal = m.goal;

    hasTask = m.hasTask;
    task = m.task;
  }
}