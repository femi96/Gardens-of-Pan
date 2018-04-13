﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionWander : MonsterAction {
  // AI controller scripting monster behavior
  //  Sets a random destination in the garden and enables movement towards it

  private Vector3 destination;
  private Vector3 step;
  private bool hasStep;

  private Monster monster;
  private MonsterMover mover;
  private MonsterBehavior state;
  private Garden garden;

  private float endDistance;

  // hasDestination is false if destination is unassigned

  public ActionWander() {}

  public void StartAction(MonsterBehavior behavior) {

    state = behavior;
    monster = state.GetMonster();
    mover = monster.GetMover();
    garden = state.GetGarden();
    endDistance = mover.radius;

    // Pick a random destination
    float limit = (garden.gardenSize / 2f);
    destination = new Vector3(Random.Range(-limit, limit), 0, Random.Range(-limit, limit));
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

    // If reached destination, stop state
    destination.y = monster.transform.position.y;
    float destDistance = (destination - monster.transform.position).magnitude;

    if (destDistance < endDistance) {
      mover.Stop();
      state.EndBehavior();
    }
  }
}