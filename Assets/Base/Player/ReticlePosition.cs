using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticlePosition : MonoBehaviour
{
    Camera m_MainCamera;
    
    //https://answers.unity.com/questions/1452254/how-to-make-an-object-follow-the-camera-orientatio.html
    private Transform cameraTransform;
    public float distanceFromCamera = 0.5f; //Set it to whatever value you think is best

    // Start is called before the first frame update
    void Start()
    {
        m_MainCamera = Camera.main; //This gets the Main Camera from the Scene
        m_MainCamera.enabled = true; //This enables Main Camera 
        cameraTransform = m_MainCamera.transform;
    }

    private Vector3 resultingPosition;
    // Update is called once per frame
    void Update()
    {
        resultingPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;
        transform.position = resultingPosition;
        transform.localRotation = cameraTransform.localRotation;
    }

}
