using System.Diagnostics;
using UnityEngine;

public class KnobHighlight : MonoBehaviour
{
    public Renderer knobRenderer;
    public Color highlightColor;

    private Color originalColor;

    void Start()
    {
        originalColor = knobRenderer.material.color;
       
    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.transform.root.name == "Cupboard_B")
            return; // ignore cupboard itself

        UnityEngine.Debug.Log($"[KnobTrigger] ENTER by: {other.name} | root: {other.transform.root.name}");

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
