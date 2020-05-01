using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;

public class ARFaceMask : MonoBehaviour
{
    public GameObject arfacemask;
    public GameObject go_mouthLenght;
    private List<AugmentedFace> faces;
    private List<Vector3> vertices;

    // Start is called before the first frame update
    void Start()
    {
        faces = new List<AugmentedFace>();
        vertices = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        Session.GetTrackables(faces, TrackableQueryFilter.All);
        foreach (AugmentedFace face in faces)
        {
            if (face.TrackingState == TrackingState.Tracking)
            {
                arfacemask.SetActive(false);
                go_mouthLenght.SetActive(true);
                face.GetVertices(vertices);
                //get vertice 62 and 292
                Vector3 mouthSide1 = vertices[62];
                Vector3 mouthSide2 = vertices[292];
                float dist = Vector3.Distance(mouthSide1, mouthSide2);
                go_mouthLenght.GetComponent<Text>().text = "Mouth length: " + dist;
            }
            else 
            {
                arfacemask.SetActive(false);
                go_mouthLenght.SetActive(false);
            }
        }
    }
}
