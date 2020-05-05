using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;

public class ARFaceMask : MonoBehaviour
{
    public GameObject arfacemask;
    public GameObject smilingMouth;
    public Text txt_mouthDist;
    public float mouthMaxDist;
    private List<AugmentedFace> faces;
    private List<Vector3> vertices;
    private float mouthDist;

    // Start is called before the first frame update
    void Start()
    {
        faces = new List<AugmentedFace>();
        vertices = new List<Vector3>();
        StartCoroutine(castDistance());
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
                mouthDist = Vector3.Distance(mouthSide1, mouthSide2);
                if(mouthDist > mouthMaxDist)
                {
                    smilingMouth.SetActive(true);
                }
                else
                {
                    smilingMouth.SetActive(false);
                }
            }
            else 
            {
                arfacemask.SetActive(false);
            }
        }
    }
    private IEnumerator castDistance()
    {
        while (true)
        {
            txt_mouthDist.text = "Mouth length: " + mouthDist.ToString();
            yield return new WaitForSeconds(1.5f);
        }
    }
}
