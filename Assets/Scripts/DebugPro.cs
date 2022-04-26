using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPro : MonoBehaviour {

    [UnityEditor.MenuItem("Debug/Log")]
    public static void Log() {
        Debug.Log((-15 >> 4) * 16);
    }
}
