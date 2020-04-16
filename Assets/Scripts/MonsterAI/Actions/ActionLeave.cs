using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionLeave : MonsterAction {
  // AI controller scripting monster behaviour
  //  Sets a random destination outside the garden and enables movement towards it

  private SerializableVector3 destination;
  private SerializableVector3 step;
  private bool hasStep;

  private static float destSize = 0.5f;

  // hasDestination is false if destination is unassigned

  public ActionLeave() {}

  public override void SetupAction(MonsterBehaviour behaviour, Monster monster) {
    Garden garden = monster.garden;

    // Pick a random destination
    List<SpawnPoint> spawnPoints = garden.GetBoard().GetSpawnPoints();
    destination = spawnPoints[Random.Range(0, spawnPoints.Count)].GetPosition();
  }

  public override void Act(MonsterBehaviour behaviour, Monster monster) {

    // Setup necessary variables
    Garden garden = monster.garden;
    EntityMover mover = monster.mover;

    // If monster path isnt valid, update path

    // Get next step on path
    if (!hasStep) {
      step = destination;
      hasStep = true;
    }

    // Set next movement to next step
    if (!mover.IsMoving()) {
      mover.MoveStart(step);
    }

    // If reached step, new step
    step.y = monster.transform.position.y;
    float stepDistance = (step - monster.transform.position).magnitude;

    if (stepDistance < destSize) {
      hasStep = false;
      mover.MoveStop();
    }

    // If reached destination, stop behaviour
    destination.y = monster.transform.position.y;
    float destDistance = (destination - monster.transform.position).magnitude;

    if (destDistance < destSize) {
      mover.MoveStop();
      garden.RemoveUnit(monster);
      monster.EndBehaviour();
    }
  }
}