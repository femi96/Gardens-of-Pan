using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLeave : MonsterAction {
  // AI controller scripting monster behavior
  //  Sets a random destination outside the garden and enables movement towards it

  private Vector3 destination;
  private Vector3 step;
  private bool hasStep;

  private Monster monster;
  private MonsterBehavior behavior;
  private EntityMover mover;
  private Garden garden;

  private static float destSize = 0.5f;

  // hasDestination is false if destination is unassigned

  public ActionLeave() {}

  public void SetupAction(MonsterBehavior behavior) {

    this.behavior = behavior;
    monster = behavior.monster;
    mover = monster.mover;
    garden = behavior.garden;

    // Pick a random destination
    List<SpawnPoint> spawnPoints = garden.GetBoard().GetSpawnPoints();
    destination = spawnPoints[Random.Range(0, spawnPoints.Count)].GetPosition();
  }

  public void Act() {

    // If monster path isnt valid, update path
    // TODO

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

    // If reached destination, stop behavior
    destination.y = monster.transform.position.y;
    float destDistance = (destination - monster.transform.position).magnitude;

    if (destDistance < destSize) {
      mover.MoveStop();
      garden.RemoveUnit(monster);
      behavior.EndBehavior();
    }
  }
}