using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionWander : MonsterAction {
  // AI controller scripting monster behaviour
  //  Sets a random destination in the garden and enables movement towards it

  private Vector3 destination;
  private Vector3 step;
  private bool hasStep;

  private Monster monster;
  private MonsterBehaviour behaviour;
  private EntityMover mover;
  private Garden garden;

  private static float destSize = 0.5f;

  // hasDestination is false if destination is unassigned

  public ActionWander() {}

  public override void SetupAction(MonsterBehaviour behaviour) {

    this.behaviour = behaviour;
    monster = behaviour.monster;
    mover = monster.mover;
    garden = behaviour.garden;

    // Pick a random destination
    float limit = (garden.gardenSize / 2f);
    destination = new Vector3(Random.Range(-limit, limit), 0, Random.Range(-limit, limit));
  }

  public override void Act() {

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

    // If reached destination, stop behaviour
    destination.y = monster.transform.position.y;
    float destDistance = (destination - monster.transform.position).magnitude;

    if (destDistance < destSize) {
      mover.MoveStop();
      behaviour.EndBehaviour();
    }
  }
}