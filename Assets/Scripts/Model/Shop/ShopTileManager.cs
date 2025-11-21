using System.Linq;
using UnityEngine;

public class ShopTileManager : MonoBehaviour
{
    public int SellInventory(Inventory playerInventory)
    {
        int totalMoney = 0;

        // Iterate over a copy of the keys to allow removal
        foreach (Crop crop in playerInventory.cropQuantities.Keys.ToList())
        {
            int quantity = playerInventory.cropQuantities[crop];

            if (quantity > 0 && CropData.SellPrice.TryGetValue(crop, out int price))
            {
                totalMoney += price * quantity;
                playerInventory.RemoveCrop(crop, quantity);
            }
        }

        return totalMoney;
    }
}
