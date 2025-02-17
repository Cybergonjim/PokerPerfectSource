using UnityEngine;

public class MaterialApplier : MonoBehaviour
{
    private Color color;

    private Material hoverMaterial;
    private Material otherMaterial;
    private Material originalMaterial;

    private Renderer objectRenderer;

    private Shader shader;

    void Start()
    {
        float H, S, V;

        objectRenderer = GetComponent<Renderer>();
        shader = Shader.Find("Universal Render Pipeline/Lit");

        color = objectRenderer.material.GetColor("_BaseColor");

        Color.RGBToHSV(color, out H, out S, out V);

        originalMaterial = new Material(shader);
        originalMaterial.color = Color.HSVToRGB(H, S, V - 0.1f);
        originalMaterial.SetFloat("_Smoothness", 1);
        objectRenderer.material = originalMaterial;

        hoverMaterial = new Material(shader);
        hoverMaterial.color = Color.HSVToRGB(H, S, V);
        hoverMaterial.SetFloat("_Smoothness", 1);
        objectRenderer.material = hoverMaterial;

        otherMaterial = new Material(shader);
        otherMaterial.color = Color.HSVToRGB(H, S, V + 0.1f);
        otherMaterial.SetFloat("_Smoothness", 1);
        objectRenderer.material = otherMaterial;
    }

    public void ApplyOther()
    {
        objectRenderer.material = otherMaterial;
    }

    public void ApplyHover()
    {
        objectRenderer.material = hoverMaterial;
    }

    public void ApplyOriginal()
    {
        objectRenderer.material = originalMaterial;
    }

}