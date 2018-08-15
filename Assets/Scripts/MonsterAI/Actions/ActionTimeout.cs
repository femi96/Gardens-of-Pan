using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionTimeout : MonsterAction {
  // AI controller scripting monster behaviour
  //  Waits a duration and ends state

  private float timeout;

  public ActionTimeout(float timeout) {
    this.timeout = timeout;
  }

  public ActionTimeout(float timeoutMin, float timeoutMax) {
    timeout = Random.Range(timeoutMin, timeoutMax);
  }

  public override void SetupAction(MonsterBehaviour behaviour, Monster monster) {}

  public override void Act(MonsterBehaviour behaviour, Monster monster) {

    if (behaviour.behaviourTime >= timeout) {
      monster.EndBehaviour();
    }
  }
}