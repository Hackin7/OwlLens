using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentsHighlight : MonoBehaviour
{
    public GameObject[] components;
    private Dictionary<string, Material> originalMaterials = new Dictionary<string, Material> { };

    private int state = 0; // 0 is cleared, 1 is highlighted
    public Material highlightMaterial;
    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<MeshRenderer>().sharedMaterial == highlightMaterial && state == 0)
        {
            print("Highlighted");
            allHighlight(); state = 1;
        }
        else if (state == 1)
        {
            clearHighlight();state = 0;
        }
    }

    void allHighlight()
    {
        for (int i = 0; i < components.Length; i++)
        {
            print("Highlight: " + components[i].name);
            originalMaterials[components[i].name] = components[i].GetComponent<MeshRenderer>().material;
            components[i].GetComponent<MeshRenderer>().material = highlightMaterial;
        }
    }
    void clearHighlight()
    {
        for (int i = 0; i < components.Length; i++)
        {
            print("Unhighlight: " + components[i].name);
            components[i].GetComponent<MeshRenderer>().material =
                originalMaterials[components[i].name];
        }
    }
}
