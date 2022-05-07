using System.Collections.Generic;

[System.Serializable]
public class InventoryCategory {
    public string categoryName;
    public List<BlockType> blockList = new();
}
