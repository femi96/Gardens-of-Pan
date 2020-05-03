using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterAITaskWait : MonsterAITask {
// Monster waits in place for X seconds

  private float time = 2f;

  public MonsterAITaskWait(float time) {
    this.time = time;
  }

  public override MonsterAITaskStatus Do(Monster mon) {
    if (time <= 0f)
      return MonsterAITaskStatus.Complete;

    time -= Time.deltaTime;
    return MonsterAITaskStatus.Ongoing;
  }
}
