using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterAITaskLeave : MonsterAITask {

  private float time = 0f;

  public override MonsterAITaskStatus Do(Monster mon) {
    time += Time.deltaTime;

    if (time > 5f) {
      if (!mon.joined)
        mon.garden.RemoveUnit(mon);

      return MonsterAITaskStatus.Complete;
    }

    return MonsterAITaskStatus.Ongoing;
  }
}
