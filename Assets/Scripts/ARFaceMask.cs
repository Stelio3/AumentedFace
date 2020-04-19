using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class ARFaceMask : MonoBehaviour
{
    public GameObject arfacemask;
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
                arfacemask.SetActive(true);
                face.GetVertices(vertices);
                //get vertice 62 and 292
                Vector3 mouthSide1 = vertices[62];
                Vector3 mouthSide2 = vertices[292];
                float dist = Vector3.Distance(mouthSide1, mouthSide2);
                print("Distance to other: " + dist);
            }
            else 
            {
                arfacemask.SetActive(false);
            }
        }
    }
}
