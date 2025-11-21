using UnityEngine;
using System.Collections.Generic;

public static class CropData
{
    // Associe chaque Crop à sa couleur de maturité
    public static readonly Dictionary<Crop, Color> MaturityColor = new Dictionary<Crop, Color>()
    {
        { Crop.Potato, Color.yellow },
        { Crop.Apple, Color.red },
        { Crop.Tomato, new Color(1f, 0.5f, 0f) }, // orange
        { Crop.Carrot, new Color(1f, 0.65f, 0f) }, // orange clair
        { Crop.Corn, Color.yellow }
    };

    // Associe chaque Crop à son prix de vente
    public static readonly Dictionary<Crop, int> SellPrice = new Dictionary<Crop, int>()
    {
        { Crop.Potato, 1 },
        { Crop.Apple, 2 },
        { Crop.Tomato, 3 },
        { Crop.Carrot, 4 },
        { Crop.Corn, 5 }
    };

    // Méthode pratique pour obtenir la couleur
    public static Color GetColor(Crop crop)
    {
        return MaturityColor.ContainsKey(crop) ? MaturityColor[crop] : Color.white;
    }

    // Méthode pratique pour obtenir le prix
    public static int GetPrice(Crop crop)
    {
        return SellPrice.ContainsKey(crop) ? SellPrice[crop] : 0;
    }
}
