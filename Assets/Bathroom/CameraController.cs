using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]

public class CameraController : MonoBehaviour
{
    //Shading
    private int effect = 0;
    private bool blurLatch = false;
    public Shader gaussianShader;
    public Shader snesShader;

    private Material mainMaterial;
    private Material snesMaterial;

    // Start is called before the first frame update
    void Start()
    {
        //Shader///////////////////////////////////////
        mainMaterial = new Material(gaussianShader);
        snesMaterial = new Material(snesShader);
        ////////////////////////////////////////////
    }

    public void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (effect > 1) { effect = 0; }
        else if (effect == 1)
        {
            Graphics.Blit(src, dst); // Needed for VR 
            //Set Blurriness
            // Create a temporary RenderTexture to hold the first pass.
            RenderTexture tmp =
                RenderTexture.GetTemporary(src.width, src.height, 0, src.format);
            // Perform both passes in order.
            Graphics.Blit(src, tmp, mainMaterial, 0);   // First pass.
            Graphics.Blit(tmp, dst, mainMaterial, 1);   // Second pass.
            RenderTexture.ReleaseTemporary(tmp);
        }
        else if (effect == -1)
        {
            Graphics.Blit(src, dst); // Needed for VR 
            Graphics.Blit(src, dst, snesMaterial);   // Second pass.
        }
        else
        {//Graphics Passthrough
            Graphics.Blit(src, dst);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Change blur Mode
        if (gameObject.GetComponent<CameraMovement>().secondSelect() && !blurLatch)
        {
            switchBlur(); blurLatch = true;
        }
        else
        {
            blurLatch = false;
        }
    }

    public void switchBlur()
    {
        effect = effect + 1;
    }
    public void snesEffect()
    {
        effect = -1;
    }
    public void resetEffect()
    {
        effect = 0;
    }
}
