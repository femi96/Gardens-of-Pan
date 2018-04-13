using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTimeout : MonsterAction {
  // AI controller scripting monster behavior
  //  Waits a duration and ends state

  private MonsterBehavior state;

  private float timeoutTime;

  public ActionTimeout(float timeout) {
    timeoutTime = timeout;
  }

  public void StartAction(MonsterBehavior behavior) {

    state = behavior;
  }

  public void Act() {

    if (state.GetBehaviorTime() >= timeoutTime) {
      state.EndBehavior();
    }
  }
}