using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.VR;

public class CameraMovement : MonoBehaviour
{
    //private GameObject player;
    public float sensitivity=5;
    private Vector3 originalPosition;
    //Touch/////////////////////////////////////////////////////
    public float rotateSpeed = 15.0f;
    public int invertPitch = 1;
    private float pitch = 0.0f, yaw = 0.0f;
    private Vector3 oRotation;//cache initial rotation of player so pitch and yaw don't reset to 0 before rotating

    private int viewMode = 0; //0 for 2D, 1 is VR mode

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //cache original position of camera
        originalPosition = transform.position;
        //cache original rotation of player so pitch and yaw don't reset to 0 before rotating
        oRotation = transform.eulerAngles;
        pitch = oRotation.x;
        yaw = oRotation.y;

        if (XRSettings.loadedDeviceName == "") { viewMode = 0; }
        else { viewMode = 1; }
    }
    private Vector3 resultingPosition;

    // Update is called once per frame
    void Update()
    {
        

        if (Application.platform == RuntimePlatform.Android)
        {
            touchCameraPan();//if (viewMode == 0) {  };
            if (Input.GetKeyDown("escape")){
                StartCoroutine(SwitchTo2D()); viewMode = 0;
                //transform.position = originalPosition;
            }
        }
        else
        {
            mousePan();
            keyboardInput();
        }
        
        //Change between 2D and VR Modes
        //https://docs.unity3d.com/Manual/Coroutines.html
        if (Input.touchCount >= 3)
        {
            if (viewMode == 0) { StartCoroutine(SwitchToVR()); viewMode = 1; }
            
        }
    }

    void mousePan()
    {
        float rotateHorizontal = Input.GetAxis("Mouse X");
        float rotateVertical = Input.GetAxis("Mouse Y");
        transform.RotateAround(gameObject.transform.position, Vector3.up, rotateHorizontal * sensitivity);
        transform.RotateAround(gameObject.transform.position, -transform.right, rotateVertical * sensitivity);
    }
    void touchCameraPan()
    {
        if (Input.touches.Length == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                pitch -= Input.GetTouch(0).deltaPosition.y * rotateSpeed * invertPitch * Time.deltaTime;
                yaw += Input.GetTouch(0).deltaPosition.x * rotateSpeed * invertPitch * Time.deltaTime;
                //limit so we dont do backflips
                pitch = Mathf.Clamp(pitch, -80, 80);
                //do the rotations of our camera
                gameObject.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
            }
        }
    }
    void keyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            print("Toggle Fullscreen");
            Screen.fullScreen = !Screen.fullScreen;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            print("Toggle Mouse");
            if (Cursor.lockState == CursorLockMode.Locked) { Cursor.lockState = CursorLockMode.None; }
            else { Cursor.lockState = CursorLockMode.Locked; }
            Cursor.visible = !Cursor.visible;
        }
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }

    private float holdTimer = 0;
    private float holdWait = 0.2f;
    //Only for quick taps
    bool touchQuick(bool start, bool hold, bool end)
    {
        if (start)
        {
            holdTimer = holdWait;
        }
        else if (hold){ holdTimer -= Time.deltaTime; }
        else if (end && holdTimer > 0) { return true; }
        //else if (end && holdTimer < 0) { return true; } //long hold
        else { holdTimer = 0; }
        return false;
    }
    public bool mainSelect()
    {
        if (Application.platform == RuntimePlatform.Android) {
            if (viewMode == 0) {
                return touchQuick(Input.GetMouseButtonDown(0),
                     Input.GetMouseButton(0),
                     Input.GetMouseButtonUp(0));
            }
            else if (viewMode == 1) {return Input.GetMouseButtonDown(0);}
            return false;
        }
        else{
            /*return touchQuick(Input.GetMouseButtonDown(0),
                    Input.GetMouseButton(0),
                    Input.GetMouseButtonUp(0));*/
            return Input.GetMouseButtonDown(0);
        }
    }
    public bool secondSelect()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            /*if (Input.touchCount == 2){
                return touchQuick(Input.GetMouseButtonDown(0),
                     Input.GetMouseButton(0),
                     Input.GetMouseButtonUp(0));
            }*/
            return false;
        }
        else
        {
            return Input.GetMouseButtonDown(1);
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
    }
    /////VR Modes///////////////////////////////////////////////////////////////////////
    //https://developers.google.com/vr/develop/unity/guides/hybrid-apps
    IEnumerator SwitchToVR()
    {
        // Device names are lowercase, as returned by `XRSettings.supportedDevices`.
        string desiredDevice = "cardboard";// XRSettings.supportedDevices;

        // Some VR Devices do not support reloading when already active, see
        // https://docs.unity3d.com/ScriptReference/XR.XRSettings.LoadDeviceByName.html
        if (string.Compare(XRSettings.loadedDeviceName, desiredDevice, true) != 0)
        {
            XRSettings.LoadDeviceByName(desiredDevice);

            // Must wait one frame after calling `XRSettings.LoadDeviceByName()`.
            yield return null;
        }

        // Now it's ok to enable VR mode.
        XRSettings.enabled = true;
    }
    IEnumerator SwitchTo2D()
    {
        // Empty string loads the "None" device.
        XRSettings.LoadDeviceByName("");

        // Must wait one frame after calling `XRSettings.LoadDeviceByName()`.
        yield return null;

        // Not needed, since loading the None (`""`) device takes care of this.
        // XRSettings.enabled = false;

        // Restore 2D camera settings.
        ResetCameras();
    }
    void ResetCameras()
    {
        // Camera looping logic copied from GvrEditorEmulator.cs
        for (int i = 0; i < Camera.allCameras.Length; i++)
        {
            Camera cam = Camera.allCameras[i];
            if (cam.enabled && cam.stereoTargetEye != StereoTargetEyeMask.None)
            {

                // Reset local position.
                // Only required if you change the camera's local position while in 2D mode.
                cam.transform.localPosition = Vector3.zero;

                // Reset local rotation.
                // Only required if you change the camera's local rotation while in 2D mode.
                cam.transform.localRotation = Quaternion.identity;

                // No longer needed, see issue github.com/googlevr/gvr-unity-sdk/issues/628.
                // cam.ResetAspect();

                // No need to reset `fieldOfView`, since it's reset automatically.
            }
        }
    }
}
