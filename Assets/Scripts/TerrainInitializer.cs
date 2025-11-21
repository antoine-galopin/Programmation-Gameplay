using UnityEngine;

public class TerrainInitializer : MonoBehaviour
{
    [Header("Setup")]
    public string configFileName = "Config/tiles";
    public GameObject grassPrefab;
    public GameObject fieldPrefab;
    public GameObject shopPrefab;

    [Header("Parent Object")]
    public Transform tilesParent;

    private void Awake()
    {
        if (tilesParent == null)
        {
            GameObject tempParent = new GameObject("TilesContainer");
            tilesParent = tempParent.transform;
        }

        LoadAndSpawnTiles();

        FieldGroupManager.Instance.BuildGroups();
    }

    private void LoadAndSpawnTiles()
    {
        TextAsset configText = Resources.Load<TextAsset>(configFileName);
        if (configText == null)
        {
            Debug.LogError($"[TerrainInitializer] Config file not found at Resources/{configFileName}");
            return;
        }

        string[] lines = configText.text.Split('\n');

        foreach (string raw in lines)
        {
            string line = raw.Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            TileEntry entry = ParseTile(line);
            if (entry == null)
            {
                Debug.LogWarning($"[TerrainInitializer] Invalid tile entry: {line}");
                continue;
            }

            GameObject prefab;

            if (entry.isGrass)
                prefab = grassPrefab;
            else if (entry.isShop)
                prefab = shopPrefab;
            else
                prefab = fieldPrefab;

            GameObject obj = Instantiate(prefab, entry.position, Quaternion.identity, tilesParent);

            // Assign crop if it's a field
            if (!entry.isGrass && !entry.isShop)
            {
                FieldManager fm = obj.GetComponent<FieldManager>();
                if (fm != null)
                    fm.crop = entry.crop;
            }
        }
    }

    private TileEntry ParseTile(string line)
    {
        string[] parts = line.Split(',');
        if (parts.Length != 4) return null;

        if (!float.TryParse(parts[0], out float x)) return null;
        if (!float.TryParse(parts[1], out float y)) return null;
        if (!float.TryParse(parts[2], out float z)) return null;

        string typeStr = parts[3].Trim();

        if (typeStr.ToLower() == "grass")
        {
            return new TileEntry(new Vector3(x, y, z), true, false, Crop.Potato);
        }
        else if (typeStr.ToLower() == "shop")
        {
            return new TileEntry(new Vector3(x, y, z), false, true, Crop.Potato);
        }
        else
        {
            // Try parsing crop
            if (System.Enum.TryParse(typeStr, true, out Crop crop))
            {
                return new TileEntry(new Vector3(x, y, z), false, false, crop);
            }
            else
            {
                Debug.LogWarning($"[TerrainInitializer] Unknown type or crop '{typeStr}' at line: {line}");
                return null;
            }
        }
    }

    private class TileEntry
    {
        public Vector3 position;
        public bool isGrass;
        public bool isShop;
        public Crop crop;

        public TileEntry(Vector3 pos, bool grass, bool shop, Crop c)
        {
            position = pos;
            isGrass = grass;
            isShop = shop;
            crop = c;
        }
    }
}
