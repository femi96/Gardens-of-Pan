using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlantSave : UnitSave {

  public bool grown;
  public int growthStage = 0;

  public float timeActive = 0f;
  public float dieTime = 0f;
  public float growTime = 0f;

  public int[] produceIds = new int[0];
  public float[] produceTimes = new float[0];
  public SerializableQuaternion[] rotations = new SerializableQuaternion[0];

  // Creates a PlantSave for a given seed
  public PlantSave(Plant p) : base(p) {
    grown = p.grown;
    growthStage = p.growthStage;
    timeActive = p.timeActive;
    dieTime = p.dieTime;
    growTime = p.growTime;
  }
}