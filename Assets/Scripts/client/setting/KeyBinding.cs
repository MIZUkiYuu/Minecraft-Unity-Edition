using UnityEngine;

[CreateAssetMenu(fileName ="KeyBinding", menuName = "ScriptableObjects/KeyBinding")]
public class KeyBinding : ScriptableObject
{
    [Header("---- Movement ----")]
    public string moveForward;
    public string moveBack;
    public string moveLeft;
    public string moveRight;
    public string moveUp;
    public string moveDown;
    public string jump;
    public string sprint;
    public string sneak;

    [Header("---- Operate ----")]
    public string attack;
    public string use;
    public string pick;

    [Header("---- Inventory ----")]
    public string dropItem;
    public string inventory;
    public string swapItem;

    [Header("---- Toolbar ----")]
    public string saveToolbar;
    public string loadToolbar;

    [Header("---- Multiplayer ----")]
    public string playerList;
    public string chat;
    public string command;

    [Header("---- View ----")]
    public string zoomView;

    [Header("---- Map ----")]
    public string minimap;
    public string worldmap;

    [Header("---- Miscellaneous ----")]
    public string advancement;
    public string screenshot;
    public string fullscreen;
    public string perspective;
    public string debugHUD;
}
