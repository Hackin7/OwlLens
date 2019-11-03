using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReticleSceneSelector : MonoBehaviour
{
    Camera m_MainCamera;
    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        m_MainCamera = Camera.main; //This gets the Main Camera from the Scene
        cameraTransform = m_MainCamera.transform;
    }

    private Vector3 resultingPosition;
    // Update is called once per frame
    void Update()
    {
        resultingPosition = gameObject.transform.position;
        rayCasting();
    }

    private RaycastHit vision;
    private float rayLength = 100;
    void rayCasting()
    {
        Debug.DrawRay(resultingPosition, cameraTransform.forward * rayLength, Color.red, 0.5f);
        if (Physics.Raycast(resultingPosition, cameraTransform.forward, out vision, rayLength))
        {
            if (m_MainCamera.gameObject.GetComponent<CameraMovement>().mainSelect())//Primary mouse Button
            {
                if (vision.collider.name == "CrappyBathroomSelector") { SceneManager.LoadScene(sceneName: "Bathroom"); }
                else if (vision.collider.name == "ClassroomSelector") {SceneManager.LoadScene(sceneName: "Classroom");}
            }
        }
    }
    
}
