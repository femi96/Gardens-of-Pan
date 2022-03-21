// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public enum MonsterAITaskStatus {
//   Ongoing,
//   Complete,
//   Failed
// }

// [System.Serializable]
// public abstract class MonsterAITask {
//   // Executes task, returns task status
//   //   All steps in task should be transactional these are interruptable
//   public abstract MonsterAITaskStatus Do(Monster mon);

//   public override string ToString() {
//     string val = this.GetType().Name;
//     val = val.Substring(13, val.Length - 13);
//     return val;
//   }
// }
