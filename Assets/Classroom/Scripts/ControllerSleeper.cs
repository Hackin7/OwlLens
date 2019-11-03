using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]

public class ControllerSleeper : MonoBehaviour
{ 
    //Shading
    private int effect = 1;
    private bool blurLatch = false;
    public Shader gaussianShader;
    public Shader blackShader;
    public Shader hallucinateShader;
    public GameObject blackCamera; //https://answers.unity.com/questions/299703/how-to-make-screen-turn-black.html
    private Material blurMaterial;
    private Material blackMaterial;
    private Material hallucinateMaterial;

    // Start is called before the first frame update
    void Start()
    {
        //Shader///////////////////////////////////////
        blurMaterial = new Material(gaussianShader);
        blackMaterial = new Material(blackShader);
        hallucinateMaterial = new Material(hallucinateShader);
        ////////////////////////////////////////////
        gameObject.GetComponent<Camera>().enabled = true;
        blackCamera.GetComponent<Camera>().enabled = false;
    }

    public void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (effect > 3) { effect = 0; }
        if (effect == 3)
        {
            Graphics.Blit(src, dst); // Needed for VR 
            //Set Blurriness
            // Create a temporary RenderTexture to hold the first pass.
            RenderTexture tmp =
                RenderTexture.GetTemporary(src.width, src.height, 0, src.format);
            RenderTexture tmp1 =
                RenderTexture.GetTemporary(src.width, src.height, 0, src.format);
            // Perform both passes in order.
            Graphics.Blit(src, tmp, blurMaterial, 0);   // First pass.
            Graphics.Blit(tmp, tmp1, blurMaterial, 1);   // Second pass.
            Graphics.Blit(tmp1, dst, hallucinateMaterial);   // Second pass.
            RenderTexture.ReleaseTemporary(tmp);
        }
        else if (effect == 2)
        {
            //Blankout
             //Graphics.Blit(src, dst); // Needed for VR 
            Graphics.Blit(src, dst, blackMaterial);
        }
        else if (effect == 1)
        {
            Graphics.Blit(src, dst); // Needed for VR 
            //Set Blurriness
            // Create a temporary RenderTexture to hold the first pass.
            RenderTexture tmp =
                RenderTexture.GetTemporary(src.width, src.height, 0, src.format);
            // Perform both passes in order.
            Graphics.Blit(src, tmp, blurMaterial, 0);   // First pass.
            Graphics.Blit(tmp, dst, blurMaterial, 1);   // Second pass.
            RenderTexture.ReleaseTemporary(tmp);
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
        if (effect == 0) { blurEffect(); }
        else if (effect == 1) { blankOut(); }
        else if (effect == 1) { hallucinateEffect(); }
        else { effect = effect + 1; }
    }
    public void blurEffect()
    {
        effect = 1;
        gameObject.GetComponent<Camera>().enabled = true;
        blackCamera.GetComponent<Camera>().enabled = false;
    }
    public void blankOut()
    {
        effect = 2;
        gameObject.GetComponent<Camera>().enabled = false;
        blackCamera.GetComponent<Camera>().enabled = true;

    }
    public void hallucinateEffect()
    {
        effect = 3;
        gameObject.GetComponent<Camera>().enabled = true;
        blackCamera.GetComponent<Camera>().enabled = false;

    }
    public void resetEffect()
    {
        effect = 0;
        gameObject.GetComponent<Camera>().enabled = true;
        blackCamera.GetComponent<Camera>().enabled = false;
    }
}
