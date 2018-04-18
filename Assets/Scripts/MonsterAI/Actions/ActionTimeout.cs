using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTimeout : MonsterAction {
  // AI controller scripting monster behavior
  //  Waits a duration and ends state

  private MonsterBehavior state;

  private float timeout;

  public ActionTimeout(float timeout) {
    this.timeout = timeout;
  }

  public ActionTimeout(float timeoutMin, float timeoutMax) {
    timeout = Random.Range(timeoutMin, timeoutMax);
  }

  public void StartAction(MonsterBehavior behavior) {

    state = behavior;
  }

  public void Act() {

    if (state.GetBehaviorTime() >= timeout) {
      state.EndBehavior();
    }
  }
}