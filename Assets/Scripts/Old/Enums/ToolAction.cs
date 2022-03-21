using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Global ToolAction Enum
public enum ToolAction {
  None,
  Dig, Fill, Soften,
  Grass, Remove,
  Wet, Dry,
  Heat, Chill,
  Plant_Seed, Swap_Seed,

  GroundRaise, GroundLower,
  WaterDig, WaterFill,
  CoverClear, CoverGrass, CoverSand,
}