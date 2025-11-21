using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public Dictionary<Crop, int> cropQuantities = new Dictionary<Crop, int>();
    public int maxCropCapacity = -1; // -1 means no limit

    public Inventory(int maxCapacity = -1)
    {
        maxCropCapacity = maxCapacity;
    }

    /// <summary>
    /// Tries to add a certain quantity of a crop to the inventory.
    /// Returns the quantity actually added.
    /// </summary>
    public int AddCrop(Crop crop, int quantity = 1)
    {
        if (quantity <= 0) return 0;

        // Initialize the crop entry if not present
        if (!cropQuantities.ContainsKey(crop))
            cropQuantities[crop] = 0;

        // Check capacity
        if (maxCropCapacity == -1)
        {
            cropQuantities[crop] += quantity;
            return quantity;
        }
        else
        {
            int current = cropQuantities[crop];
            int spaceLeft = maxCropCapacity - current;
            int toAdd = Mathf.Min(spaceLeft, quantity);

            if (toAdd <= 0) return 0;

            cropQuantities[crop] += toAdd;
            return toAdd;
        }
    }

    /// <summary>
    /// Removes a certain quantity of a crop from the inventory.
    /// Returns the quantity actually removed.
    /// </summary>
    public int RemoveCrop(Crop crop, int quantity = 1)
    {
        if (quantity <= 0) return 0;

        if (!cropQuantities.ContainsKey(crop) || cropQuantities[crop] <= 0)
            return 0;

        int current = cropQuantities[crop];
        int toRemove = Mathf.Min(current, quantity);

        cropQuantities[crop] -= toRemove;

        return toRemove;
    }

    /// <summary>
    /// Returns the current quantity of a crop
    /// </summary>
    public int GetQuantity(Crop crop)
    {
        if (!cropQuantities.ContainsKey(crop)) return 0;
        return cropQuantities[crop];
    }
}
