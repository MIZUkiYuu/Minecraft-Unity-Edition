using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sounds", menuName = "ScriptableObjects/Sounds")]
public class Sounds : ScriptableObject {
    public List<AudioClip> bgm;
    public List<SoundType> block = new();
}
