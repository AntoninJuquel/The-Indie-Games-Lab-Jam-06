using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] Color occupiedColor;
    [SerializeField] Color freeColor;
    [SerializeField] Color hoverColor;
    [SerializeField] float intensity;


    // Node type
    BuildingNode buildingNode;

    Color baseColor;
    Renderer render;
    MaterialPropertyBlock block;
    bool isActive = true;
    private void Awake()
    {
        // Finding node type
        buildingNode = GetComponent<BuildingNode>();

        block = new MaterialPropertyBlock();
        render = GetComponent<Renderer>();
        baseColor = render.material.GetColor("_BaseColor");
    }
    private void OnMouseOver()
    {
        if (buildingNode)
            if (buildingNode.status == BuildingNode.Status.occupied)
                block.SetColor("_BaseColor", occupiedColor);
            else
                block.SetColor("_BaseColor", freeColor);

        render.SetPropertyBlock(block);
    }
    public void MouseHoverSettingsButton()
    {
        if (isActive)
        {
            float factor = Mathf.Pow(2, intensity);
            Color color = new Color(hoverColor.r * factor, hoverColor.g * factor, hoverColor.b * factor);
            block.SetColor("_BaseColor", hoverColor);
            block.SetColor("_EmissionColor", color);
            render.SetPropertyBlock(block);
        }
    }

    public void ToggleColor()
    {
        isActive = !isActive;

        if (isActive)
        {
            float factor = Mathf.Pow(2, intensity);
            Color color = new Color(baseColor.r * factor, baseColor.g * factor, baseColor.b * factor);
            block.SetColor("_BaseColor", baseColor);
            block.SetColor("_EmissionColor", color);
        }
        else
        {
            float factor = Mathf.Pow(2, 0);
            Color color = new Color(occupiedColor.r * factor, occupiedColor.g * factor, occupiedColor.b * factor);
            block.SetColor("_BaseColor", occupiedColor);
            block.SetColor("_EmissionColor", color);
        }

        render.SetPropertyBlock(block);
    }
    public void PlayLevel(int levelNumber)
    {
        if (isActive)
            StartCoroutine(ButtonManager.Instance.LoadLevel(levelNumber));
    }
    private void OnMouseExit()
    {
        if (isActive)
        {
            float factor = Mathf.Pow(2, intensity);
            Color color = new Color(baseColor.r * factor, baseColor.g * factor, baseColor.b * factor);

            block.SetColor("_BaseColor", baseColor);
            block.SetColor("_EmissionColor", color);
            render.SetPropertyBlock(block);
        }
    }
}