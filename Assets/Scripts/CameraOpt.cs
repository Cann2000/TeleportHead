using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOpt : MonoBehaviour
{
    public static CameraOpt instance;

    public bool FocusAnimEnd;

    Camera cam;
    float TimeLine = 0.0f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    private void Start()
    {
        cam = Camera.main;
    }

    public void CamStartPos()
    {
        cam.transform.localPosition = new Vector3(-17, -25, 0);
    }
    public void CameraFowUpdateUp()
    {
        if (Time.time >= TimeLine && cam.fieldOfView < 115)
        {
            cam.fieldOfView++;

            TimeLine = Time.time + 0.01f;
        }
    }
    public void CameraFowUpdateDown()
    {
        if (Time.time >= TimeLine && cam.fieldOfView > 90)
        {
            cam.fieldOfView--;

            TimeLine = Time.time + 0.01f;
        }
    }

    public void CameraMove()
    {
        float Horizontal;

        cam.GetComponent<CameraFollower>().target = null;

        Horizontal = 0.8f * Input.GetAxis("Mouse X");

        Horizontal = Mathf.Clamp(Horizontal, -2, 2);

        float cameraPosX = cam.transform.position.x;

        cam.transform.localPosition = new Vector3(cameraPosX + (-Horizontal), cam.transform.position.y, cam.transform.position.z);

    }
    public void LevelTrue()
    {
        GameManager.Instance.LevelState(true);
    }


}
