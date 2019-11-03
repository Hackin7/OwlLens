using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ghostHallucination : MonoBehaviour
{
    Camera m_MainCamera;
    private Transform cameraTransform;
    public float distanceFromCamera; //Set it to whatever value you think is best
    private RawImage image;

    // Start is called before the first frame update
    void Start()
    {
        m_MainCamera = Camera.main; //This gets the Main Camera from the Scene
        m_MainCamera.enabled = true; //This enables Main Camera 
        cameraTransform = m_MainCamera.transform;
        image = GetComponent<RawImage>();
        setOpacity(1f);
    }

    private float timer = 0.8f;
    // Update is called once per frame
    void Update()
    {
        Vector3 resultingPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;
        transform.position = resultingPosition;
        transform.localRotation = cameraTransform.localRotation;

        timer -= Time.deltaTime;
        if (timer < 0) { Destroy(gameObject); }

        //https://stackoverflow.com/questions/32415545/how-can-i-decrease-opacity-in-unity

        setOpacity(0.5f);
    }

    void setOpacity(float t)
    {
        var tempColor = image.color;
        tempColor.a = t;
        image.color = tempColor;
    }
}
