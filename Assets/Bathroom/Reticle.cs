using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    Camera m_MainCamera;
    
    //https://answers.unity.com/questions/1452254/how-to-make-an-object-follow-the-camera-orientatio.html
    private Transform cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        m_MainCamera = Camera.main; cameraTransform = m_MainCamera.transform;
    }

    private Vector3 resultingPosition;
    // Update is called once per frame
    void Update()
    {
        resultingPosition = gameObject.transform.position;
        if (carrying != null) { carrying.transform.position = resultingPosition;}
        rayCasting();
    }

    

    private GameObject carrying = null;
    public GameObject trash;
    //private bool selected = false;

    private RaycastHit vision;
    private float rayLength = 10;
    private bool isGrabbed;
    private Rigidbody grabbedObject;
    void rayCasting()
    {
        Debug.DrawRay(resultingPosition, cameraTransform.forward * rayLength, Color.red, 0.5f);
        if (Physics.Raycast(resultingPosition, cameraTransform.forward, out vision, rayLength))
        {
            //Debug.Log(vision.collider.name);
            if (m_MainCamera.gameObject.GetComponent<CameraMovement>().mainSelect())//Primary mouse Button
            {
                if (carrying == null)
                {
                    if (vision.collider.name == "Sleep Deprivation Juice")
                    {
                        CameraController cam = m_MainCamera.GetComponent<CameraController>();
                        cam.switchBlur();
                    }
                    else if (vision.collider.name == "SNES")
                    {
                        CameraController cam = m_MainCamera.GetComponent<CameraController>();
                        cam.snesEffect();
                    }
                    else if (vision.collider.name == "Hole")
                    {
                        CameraController cam = m_MainCamera.GetComponent<CameraController>();
                        cam.resetEffect();
                    }
                    else if (vision.collider.gameObject.CompareTag("Item"))
                    {
                        carrying = vision.collider.gameObject;
                    }
                    else if (vision.collider.gameObject.name == "Mirror")
                    {
                        Gameplay g = GameObject.Find("Scoring").GetComponent< Gameplay >();
                        g.reset();
                    }
                }
                else if (carrying != null)
                {
                    //Place it slightly out so can collect
                    carrying.transform.position = resultingPosition + cameraTransform.forward * 0.2f;
                    if (vision.collider.name == "Trash Can")
                    {
                        Destroy(carrying);
                        
                    }

                    carrying = null;
                }
                //selected = true;
            }
            else
            {
                //selected = false;
            }
                
        }
    }
    //Collision selection
    /*
    //Debugging
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Item"))
            {
            Debug.Log("Tooth Brush");
        }
        else if (other.gameObject.CompareTag("Trash Can"))
        {
            Debug.Log("Trash Can");
        }
        else
        {
            Debug.Log("I don't know what this is");
        }
    
    }
    private bool selected = false;
    
    private void OnTriggerStay(Collider other)
    {
        //Select Object
        if ( (Input.GetMouseButtonDown(0) || Input.touchCount > 0) && 
            !selected)//Primary mouse Button
        {

            if (carrying == null)
            {
                if (other.gameObject.CompareTag("Item")) {
                    carrying = other.gameObject;
                }
                else if (other.gameObject.CompareTag("Sleep Deprivation Juice"))
                {
                    CameraController cam = m_MainCamera.GetComponent<CameraController>();
                    cam.switchBlur();
                }
                else if (other.gameObject.CompareTag("SNES"))
                {
                    CameraController cam = m_MainCamera.GetComponent<CameraController>();
                    cam.snesEffect();
                }
                else if (other.gameObject.CompareTag("Toilet"))
                {
                    CameraController cam = m_MainCamera.GetComponent<CameraController>();
                    cam.resetEffect();
                }
                else if (!other.gameObject.CompareTag("Trash Can"))
                {
                    carrying = other.gameObject;
                }
            }
            else
            {
                if (other.gameObject.CompareTag("Trash Can") )
                {
                    Destroy(carrying);
                    Instantiate(trash, new Vector3(-0.64f, 1.283f, 0.618865f), Quaternion.identity);
                }
                carrying = null;
            }
            selected = true;
        }
        else if (Input.touchCount == 0) {
            selected = false;
        }
    }
    */
}
