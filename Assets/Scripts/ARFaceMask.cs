using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;

public class ARFaceMask : MonoBehaviour
{
    public GameObject arfacemask;
    public GameObject smilingMouth, go_leftEye, go_rightEye;
    public Text txt_mouthDist, txt_leftEye, txt_rightEye, txt_info;

    private float mouthCalMaxDist, leftEyeCalMaxDist = 1, rightEyeCalMaxDist = 1;
    private List<AugmentedFace> faces;
    private List<Vector3> vertices;
    private float mouthDist, leftEyeDist, rightEyeDist;
    private string s_mouthMaxDist, s_eyeLeftMaxDist, s_eyeRightMaxDist;
    private bool isCalibrating=false, mouthCalibrated = false, eyesCalibrated = false;

    // Start is called before the first frame update
    void Start()
    {
        faces = new List<AugmentedFace>();
        vertices = new List<Vector3>();
        txt_info.text = "Touch with 1 finger and smile to calibrate mouth, 2 fingers and close your eyes to calibrate it or 3 to reset";
    }

    // Update is called once per frame
    void Update()
    {
        Session.GetTrackables(faces, TrackableQueryFilter.All);
        foreach (AugmentedFace face in faces)
        {
            if (Input.touchCount == 1 && !mouthCalibrated)
            {
                calibrating("mouth");
                if (!isCalibrating)
                {
                    StartCoroutine(Calibrated("mouth"));
                    isCalibrating = true;
                }
            }
            if (Input.touchCount == 2 && !eyesCalibrated)
            {
                calibrating("eyes");
                if (!isCalibrating)
                {
                    StartCoroutine(Calibrated("eyes"));
                    isCalibrating = true;
                }
            }
            if(Input.touchCount == 3)
            {
                mouthCalMaxDist = 0;
                leftEyeCalMaxDist = 1;
                rightEyeCalMaxDist = 1;
                isCalibrating = false;
                mouthCalibrated = false;
                eyesCalibrated = false;
                txt_info.text = "Touch with 1 finger and smile to calibrate mouth, 2 fingers and close your eyes to calibrate it or 3 to reset";
            }
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
                if (leftEyeDist < leftEyeCalMaxDist * 1.1 && eyesCalibrated)
                {
                    go_leftEye.SetActive(true);
                }
                else
                {
                    go_leftEye.SetActive(false);
                }
                if (rightEyeDist < rightEyeCalMaxDist * 1.1 && eyesCalibrated)
                {
                    go_rightEye.SetActive(true);
                }
                else
                {
                    go_rightEye.SetActive(false);
                }
                if ((mouthDist > mouthCalMaxDist * 0.9) && mouthCalibrated)
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
    private void calibrating(string calType)
    {
        if (calType.Equals("mouth"))
        {
            if (!mouthCalibrated)
            {
                if (mouthDist > mouthCalMaxDist)
                {
                    mouthCalMaxDist = mouthDist;
                }
            }
        }else if(calType.Equals("eyes"))
        {
            if (!eyesCalibrated)
            {
                if (leftEyeDist < leftEyeCalMaxDist)
                {
                    leftEyeCalMaxDist = leftEyeDist;
                }
                if (rightEyeDist < rightEyeCalMaxDist)
                {
                    rightEyeCalMaxDist = rightEyeDist;
                }
            }
        }
    }
    IEnumerator Calibrated(string calType)
    {
        txt_info.text = "Calibrating...!";
        yield return new WaitForSeconds(3f);

        if (calType.Equals("mouth"))
        {
            mouthCalibrated = true;
            isCalibrating = false;
            s_mouthMaxDist = mouthCalMaxDist.ToString();
            txt_mouthDist.text = s_mouthMaxDist;
            txt_info.text = "Mouth calibrated!";
            yield return new WaitForSeconds(1f);
            if(eyesCalibrated)
                txt_info.text = "Touch with 3 fingers to reset";
            else
                txt_info.text = "Touch with 2 fingers and close your eyes to calibrate it or 3 to reset";
        }
        else if(calType.Equals("eyes"))
        {
            eyesCalibrated = true;
            isCalibrating = false;
            s_eyeLeftMaxDist = leftEyeCalMaxDist.ToString();
            s_eyeRightMaxDist = rightEyeCalMaxDist.ToString();
            txt_leftEye.text = s_eyeLeftMaxDist;
            txt_rightEye.text = s_eyeRightMaxDist;
            txt_info.text = "Eyes calibrated!";
            yield return new WaitForSeconds(1f);
            if (mouthCalibrated)
                txt_info.text = "Touch with 3 fingers to reset";
            else
                txt_info.text = "Touch with 1 fingers and smile to calibrate mouth or 3 to reset";
        }
    }
}
