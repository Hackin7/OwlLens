using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gameplay : MonoBehaviour
{
    // Start is called before the first frame update
    //public Text information;
    //public Text myText;

    public GameObject info;
    //private TextMeshPro infoText;
    private float timer = 0;
    
    void Start()
    {
        info.GetComponent<TextMeshPro>().text = "Hello there\nClick here to start";
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0) {
            timer -= Time.deltaTime;
        }else if (timer < 0)
        {
            timer = 0;
        }
            
        trashGenerate();
        screenUpdate();
    }
   
    private string screenTime = "";
    private int score = 0;
    void screenUpdate()
    {   

        screenTime = "Throw your Trash! (cubes)\n\n\nScore: "+score+"\nTime: " + (timer);
        if (timer == 0)
        {
            screenTime = "Throw your Trash! (cubes)\nClick here to start\n\nScore: " + score ;
        }
        info.GetComponent<TextMeshPro>().text = screenTime;
    }
    

    public GameObject trash;
    void trashGenerate()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag( "Item" );

        if (items.Length == 0 && timer > 0)
        {
            score++;
            Instantiate(trash, new Vector3(-0.64f, 1.283f, 0.618865f), Quaternion.identity);
        }
        
    }
    public void reset() {
        if (timer <= 0)
        {
            score = 0; timer = 60;
        }
    }
}
