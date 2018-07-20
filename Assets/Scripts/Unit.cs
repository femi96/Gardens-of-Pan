using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour {
  // Game controller that handles a unit's state and behavior

  public Garden garden;

  public int prefabID;

  public virtual void Awake() {
    garden = GameObject.FindWithTag("Garden").GetComponent<Garden>();
  }

  // Returns name of unit
  public abstract string GetName();

  // Returns size of unit
  public abstract float GetSize();

  // Returns radius of wand when selecting unit
  public abstract float GetWandRadius();

  // Returns base height of hover UI
  public abstract float GetHoverHeight();

  // Sets unit's prefab id number
  public void SetUnitPrefabID(int id) {
    prefabID = id;
  }

  // Returns unit save of unit
  public virtual UnitSave GetUnitSave() {
    UnitSave save = new UnitSave();
    save.position = transform.position;
    save.rotation = transform.rotation;
    save.prefabID = prefabID;
    return save;
  }
}