using UnityEngine;

public class KeyHighlight : MonoBehaviour
{
    public Renderer keyRenderer;

    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    void Start()
    {
        keyRenderer.material.color = normalColor;
    }

    public void Highlight()
    {
        keyRenderer.material.color = highlightColor;
    }

    public void RemoveHighlight()
    {
        keyRenderer.material.color = normalColor;
    }
}