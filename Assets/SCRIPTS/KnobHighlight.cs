using UnityEngine;

public class KnobHighlight : MonoBehaviour
{
    public Renderer knobRenderer;
    public Color highlightColor = Color.yellow;

    private Color originalColor;

    void Start()
    {
        originalColor = knobRenderer.material.color;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            knobRenderer.material.color = highlightColor;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            knobRenderer.material.color = originalColor;
        }
    }
}
