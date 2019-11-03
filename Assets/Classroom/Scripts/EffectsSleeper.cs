using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EffectsSleeper : MonoBehaviour
{
    private Camera m_MainCamera;
    public GameObject man;
    public GameObject manHallucination;
    public GameObject prompt;

    // Start is called before the first frame update
    void Start()
    {
        m_MainCamera = Camera.main; //This gets the Main Camera from the Scene
        m_MainCamera.enabled = true;
    }

    // Update is called once per frame
    private float timer = 0;
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            ControllerSleeper cam = m_MainCamera.GetComponent<ControllerSleeper>();
            if (timer < 5) { cam.blankOut();}
            else if (timer < 9) { wakeUp(); manReset(); }
            else if (timer < 11)
            {
                manMove();
                Instantiate(manHallucination, new Vector3(4.5f, 0.15f, 1f), Quaternion.identity);
                Instantiate(manHallucination, new Vector3(3f, 0.15f, -2.5f), Quaternion.identity);
                Instantiate(manHallucination, new Vector3(1f, 0.15f, 1f), Quaternion.identity);
                Instantiate(manHallucination, new Vector3(0f, 0.15f, -2.5f), Quaternion.identity);
                cam.hallucinateEffect();
            }
            else if (timer < 12)
            {
                Instantiate(manHallucination, new Vector3(4.4388f, 0.15f, 2.0934f), Quaternion.identity);
            }
            else if (timer < 15) { cam.blurEffect(); }
            else if (timer < 16) { cam.blankOut(); }
            else if (timer < 21) { cam.blurEffect(); }
            else if (timer < 22) { cam.blankOut(); }
            else if (timer < 30) {cam.blurEffect();}
            else {cam.resetEffect();}
            if ( m_MainCamera.GetComponent<CameraMovement>().mainSelect()) { timer = -1;print("Reset"); return; }


        }
        else if (timer < 0)
        {
            timer = 0;
            reset();
            print("End");
            //prompt.GetComponent<TextMeshPro>().color = new Color32(0, 0, 0, 255);
            prompt.GetComponent<TextMeshPro>().text = "Click to Start";

            ControllerSleeper cam = m_MainCamera.GetComponent<ControllerSleeper>();
            cam.resetEffect();
        }
        if(m_MainCamera.GetComponent<CameraMovement>().mainSelect()) { timer = 40; print("Start Timer");reset(); }

    }

    void reset()
    {
        manReset();
        //prompt.GetComponent<TextMeshPro>().color = new Color32(0, 0, 0, 255);
        prompt.GetComponent<TextMeshPro>().text = "";
    }
    void manReset() { man.transform.position = new Vector3(4.77f, 0.151f, -1.162f); }

    void manMove()
    {
        man.transform.position += new Vector3(0f, 0.1f, 0f);
    }

    void wakeUp()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("manHallucination");
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }

        /*enemies = GameObject.FindGameObjectsWithTag("ghostHallucination");
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }*/
    }
}
