using System.Collections.Generic;
using UnityEngine;
using static FieldManager;

public class FieldGroupManager : MonoBehaviour
{
    public static FieldGroupManager Instance;

    private List<FieldGroup> groups = new List<FieldGroup>();

    private void Awake()
    {
        Instance = this;
    }

    public void BuildGroups()
    {
        Debug.Log("[FieldGroupManager] Building field groups...");

        FieldManager[] allFields = FindObjectsOfType<FieldManager>();
        HashSet<FieldManager> visited = new HashSet<FieldManager>();

        foreach (var field in allFields)
        {
            if (visited.Contains(field))
                continue;

            // Create a new group from this field using BFS
            FieldGroup group = FloodFillGroup(field, visited);
            groups.Add(group);
        }

        Debug.Log($"[FieldGroupManager] Created {groups.Count} field groups.");
    }

    private FieldGroup FloodFillGroup(FieldManager start, HashSet<FieldManager> visited)
    {
        FieldGroup group = new FieldGroup(start.crop);
        Queue<FieldManager> queue = new Queue<FieldManager>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            FieldManager current = queue.Dequeue();
            group.fields.Add(current);
            current.group = group; // assign group to field

            // Find adjacent fields
            foreach (FieldManager neighbor in GetAdjacentFields(current))
            {
                if (!visited.Contains(neighbor) && neighbor.crop == start.crop)
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        return group;
    }

    private List<FieldManager> GetAdjacentFields(FieldManager field)
    {
        List<FieldManager> result = new List<FieldManager>();
        Vector3 pos = field.transform.position;

        // Adjacent positions (4-directional)
        Vector3[] offsets = new Vector3[]
        {
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, -1)
        };

        foreach (var offset in offsets)
        {
            Vector3 checkPos = pos + offset;

            Collider[] hits = Physics.OverlapSphere(checkPos, 0.1f);
            foreach (var hit in hits)
            {
                FieldManager fm = hit.GetComponent<FieldManager>();
                if (fm != null)
                    result.Add(fm);
            }
        }

        return result;
    }

    public bool CanInteract(FieldManager field)
    {
        if (field.group == null)
            return true;

        var group = field.group;
        var state = field.GetCurrentState();

        switch (state)
        {
            case FieldState.EMPTY:
                // We want to SEED this field:
                // => every field must be EMPTY or SEEDED
                foreach (var f in group.fields)
                {
                    var s = f.GetCurrentState();
                    if (s == FieldState.WATERED)
                        return false; // cannot seed if ANY field is watered
                }
                return true;

            case FieldState.SEEDED:
                // We want to WATER this field:
                // => every field must be SEEDED or WATERED
                foreach (var f in group.fields)
                {
                    var s = f.GetCurrentState();
                    if (s == FieldState.EMPTY)
                        return false; // cannot water if ANY field is still empty
                }
                return true;

            case FieldState.WATERED:
                // We want to HARVEST this field:
                // => every field must be WATERED or EMPTY
                foreach (var f in group.fields)
                {
                    var s = f.GetCurrentState();
                    if (s == FieldState.SEEDED)
                        return false; // cannot harvest if ANY field is still seeded
                }
                return true;
        }

        return true;
    }
}
