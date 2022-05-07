using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundType {
    public string name;
    public List<AudioClip> blockPlaceAndDigSound = new();
    public List<AudioClip> blockStepSound = new();
    public List<AudioClip> blockClickSound = new();
    public List<AudioClip> blockBreakSound = new();
}
