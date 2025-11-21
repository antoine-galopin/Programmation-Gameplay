using System.Collections.Generic;
using static FieldManager;

public class FieldGroup
{
    public Crop crop;
    public List<FieldManager> fields = new List<FieldManager>();

    // Constructor
    public FieldGroup(Crop crop)
    {
        this.crop = crop;
    }

    public bool AllEmpty()
    {
        foreach (var f in fields)
            if (f.GetCurrentState() != FieldManager.FieldState.EMPTY)
                return false;
        return true;
    }

    public bool AllSeeded()
    {
        // Consider a field "seeded" if it's SEEDED OR WATERED (i.e. not EMPTY)
        foreach (var f in fields)
            if (f.GetCurrentState() == FieldManager.FieldState.EMPTY)
                return false;
        return true;
    }

    public bool AllWatered()
    {
        // All must be exactly WATERED
        foreach (var f in fields)
            if (f.GetCurrentState() != FieldManager.FieldState.WATERED)
                return false;
        return true;
    }
}
