using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAway : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private float timer = 0.8f;
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0){Destroy(gameObject);}
        transform.position += new Vector3(0f, 0.1f, 0f);
    }
}
