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
  private MonsterMover mover;
  private MonsterBehavior behavior;
  private Garden garden;

  private float endDistance;

  // hasDestination is false if destination is unassigned

  public ActionLeave() {}

  public void SetupAction(MonsterBehavior behavior) {

    this.behavior = behavior;
    monster = behavior.monster;
    mover = monster.mover;
    garden = behavior.garden;
    endDistance = mover.radius;

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

      mover.Move(step);
    }

    // If reached step, new step
    step.y = monster.transform.position.y;
    float stepDistance = (step - monster.transform.position).magnitude;

    if (stepDistance < endDistance) {
      hasStep = false;
      mover.Stop();
    }

    // If reached destination, stop behavior
    destination.y = monster.transform.position.y;
    float destDistance = (destination - monster.transform.position).magnitude;

    if (destDistance < endDistance) {
      mover.Stop();
      garden.RemoveUnit(monster);
      behavior.EndBehavior();
    }
  }
}