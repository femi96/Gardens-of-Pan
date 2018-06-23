using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTimeout : MonsterAction {
  // AI controller scripting monster behavior
  //  Waits a duration and ends state

  private MonsterBehavior behavior;

  private float timeout;

  public ActionTimeout(float timeout) {
    this.timeout = timeout;
  }

  public ActionTimeout(float timeoutMin, float timeoutMax) {
    timeout = Random.Range(timeoutMin, timeoutMax);
  }

  public void SetupAction(MonsterBehavior behavior) {

    this.behavior = behavior;
  }

  public void Act() {

    if (behavior.behaviorTime >= timeout) {
      behavior.EndBehavior();
    }
  }
}