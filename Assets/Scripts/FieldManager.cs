using UnityEngine;
using System.Collections;

public class FieldManager : MonoBehaviour
{
    public enum FieldState
    {
        EMPTY,
        SEEDED,
        WATERED
    }

    [Header("Settings")]
    public Color emptyColor = Color.white;
    public Color seededColor = Color.yellow; // couleur générique pour semé

    [Header("Crop Settings")]
    public Crop crop; // assignable dans l’inspecteur

    private FieldState currentState = FieldState.EMPTY;
    private Renderer fieldRenderer;

    public FieldGroup group; // assigned by FieldGroupManager

    private void Awake()
    {
        fieldRenderer = GetComponent<Renderer>();
        if (fieldRenderer == null)
            Debug.LogError($"{gameObject.name} requires a Renderer component!");

        UpdateColor();
    }

    public void Action()
    {
        if (group != null && !FieldGroupManager.Instance.CanInteract(this))
            return;

        GoToNextState();
    }


    /// <summary>
    /// Passe au prochain état cyclique
    /// </summary>
    public void GoToNextState()
    {
        currentState = (FieldState)(((int)currentState + 1) % System.Enum.GetNames(typeof(FieldState)).Length);
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (fieldRenderer == null) return;

        switch (currentState)
        {
            case FieldState.EMPTY:
                fieldRenderer.material.color = emptyColor;
                break;
            case FieldState.SEEDED:
                fieldRenderer.material.color = seededColor;
                break;
            case FieldState.WATERED:
                fieldRenderer.material.color = CropData.GetColor(crop); // couleur de maturité du crop
                break;
        }
    }

    public FieldState GetCurrentState()
    {
        return currentState;
    }
}
