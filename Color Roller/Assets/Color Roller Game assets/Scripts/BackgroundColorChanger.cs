using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorChanger : MonoBehaviour
{
    public Material BackgroundMaterial;
    public Color[] BackgroundColors;

    public static Color selectedColor;

    private void Start()
    {
        selectedColor = BackgroundColors[Random.Range(0, BackgroundColors.Length)];
        BackgroundMaterial.color = selectedColor;
    }
}
