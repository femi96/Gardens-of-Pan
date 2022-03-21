// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [System.Serializable]
// public class MonsterAIMotivationJoin : MonsterAIMotivation {

//   public override float GetPriority(Monster mon) {
//     if (!mon.joined && mon.CanJoin())
//       return 1f;
//     else
//       return -1f;
//   }

//   public override MonsterAIGoal GenerateGoal(Monster mon) {
//     return new MonsterAIGoalJoin();
//   }
// }
