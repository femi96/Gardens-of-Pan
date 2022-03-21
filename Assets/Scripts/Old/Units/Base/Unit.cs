// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public abstract class Unit : MonoBehaviour {
//   // Handles a unit's state and behavior

//   [Header("Unit Fields")]
//   public Garden garden;
//   public GardenBoard board;

//   public int prefabID;
//   public int unitID;

//   public virtual void Awake() {
//     garden = GameObject.FindWithTag("Garden").GetComponent<Garden>();
//     // board = garden.GetBoard();
//     unitID = this.GetInstanceID();
//   }

//   // Returns name of unit
//   public abstract string GetName();

//   // Returns size of unit
//   public abstract float GetSize();

//   // Returns radius of wand when selecting unit
//   public abstract float GetWandRadius();

//   // Returns base height of hover UI
//   public abstract float GetHoverHeight();

//   // Sets unit's prefab id number
//   public void SetUnitPrefabID(int id) {
//     prefabID = id;
//   }

//   // Returns id of unit
//   public int GetID() {
//     return unitID;
//   }

//   // Returns unit save of unit
//   public virtual UnitSave GetUnitSave() {
//     UnitSave save = new UnitSave(this);
//     return save;
//   }

//   // Sets unit from UnitSave
//   public virtual void SetFromSave(UnitSave save) {
//     unitID = save.unitID;
//   }
// }