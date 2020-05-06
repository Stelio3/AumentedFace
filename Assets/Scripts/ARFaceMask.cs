using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;

public class ARFaceMask : MonoBehaviour
{
    public GameObject arfacemask;
    public GameObject smilingMouth, go_leftEye, go_rightEye;
    public Text txt_mouthDist, txt_leftEye, txt_rightEye;
    public float mouthMaxDist, leftEyeMaxDist, rightEyeMaxDist;
    private List<AugmentedFace> faces;
    private List<Vector3> vertices;
    private float mouthDist, leftEyeDist, rightEyeDist;

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
                
                Vector3 mouthSide1 = vertices[62];
                Vector3 mouthSide2 = vertices[292];
                Vector3 leftEye1 = vertices[159];
                Vector3 leftEye2 = vertices[145];
                Vector3 rightEye1 = vertices[386];
                Vector3 rightEye2 = vertices[374];
                mouthDist = Vector3.Distance(mouthSide1, mouthSide2);
                leftEyeDist = Vector3.Distance(leftEye1, leftEye2);
                rightEyeDist = Vector3.Distance(rightEye1, rightEye2);
                if(leftEyeDist < leftEyeMaxDist)
                {
                    go_leftEye.SetActive(true);
                }
                else
                {
                    go_leftEye.SetActive(false);
                }
                if (rightEyeDist < rightEyeMaxDist)
                {
                    go_rightEye.SetActive(true);
                }
                else
                {
                    go_rightEye.SetActive(false);
                }
                if (mouthDist > mouthMaxDist)
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
            txt_rightEye.text = "EyeRight length: " + rightEyeDist.ToString();
            txt_leftEye.text = "EyeLeft length: " + leftEyeDist.ToString();
            yield return new WaitForSeconds(1.5f);
        }
    }
}
