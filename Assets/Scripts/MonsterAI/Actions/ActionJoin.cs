using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionJoin : MonsterAction {
  // AI controller scripting monster behavior
  //  Monster tries to join garden

  private Monster monster;
  private MonsterBehavior state;
  private Garden garden;

  public ActionJoin() {}

  public void StartAction(MonsterBehavior behavior) {

    state = behavior;
    monster = state.GetMonster();
    garden = state.GetGarden();
  }

  public void Act() {

    if (!monster.IsOwned())
      monster.SetOwned(monster.CanOwn(garden));

    state.EndBehavior();
  }
}