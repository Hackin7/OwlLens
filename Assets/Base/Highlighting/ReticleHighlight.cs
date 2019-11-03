using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleHighlight : MonoBehaviour
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
        rayCasting(); clearHighlight();
    }
    
    public Material highlightMaterial;
    private RaycastHit vision;
    private float rayLength = 100;
    private List<GameObject> highlighted = new List<GameObject> { };
    private Dictionary<string, Material> originalMaterials = new Dictionary<string, Material> { };
    public List<string> namesToHighlight = new List<string> { }; public List<string> tagsToHighlight = new List<string> { };
    void rayCasting()
    {
        if (Physics.Raycast(resultingPosition, cameraTransform.forward, out vision, rayLength))
        {
            

            //Highlight////////////////////////////////////////////
            if ((namesToHighlight.Contains(vision.collider.name) || tagsToHighlight.Contains(vision.collider.tag))
                && !highlighted.Contains(vision.collider.gameObject))
            {
                Debug.Log(vision.collider.name);
                highlighted.Add(vision.collider.gameObject);
                originalMaterials[vision.collider.name] = vision.collider.gameObject.GetComponent<MeshRenderer>().material;
                vision.collider.gameObject.GetComponent<MeshRenderer>().material = highlightMaterial;
            }
            
        }
    }

    void clearHighlight()
    {
        for (int i = 0; i < highlighted.Count; i++)
        {
            if (highlighted[i] != null)
            {
                if (vision.collider.name != highlighted[i].name)
                {
                    print("Unhighlight: "+highlighted[i].name);
                    highlighted[i].GetComponent<MeshRenderer>().material =
                        originalMaterials[highlighted[i].name];
                    highlighted[i] = null;
                }
            }

        }
    }
}
