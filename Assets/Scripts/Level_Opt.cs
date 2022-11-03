using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Opt : Singleton<Level_Opt>
{
    [SerializeField] GameObject Skull;
    [SerializeField] Transform SkullPos;
    [SerializeField] GameObject SkullPosParent;
    [SerializeField] GameObject Character;

    public bool FocusAnimEnd;

    Camera cam;
    float TimeLine = 0.0f;

    private void Start()
    {
        cam = Camera.main;
    }

    public void SkullSpawn()
    {
        GameObject SkullClone = Instantiate(Skull, SkullPos.position, Quaternion.identity);
        SkullClone.transform.SetParent(SkullPosParent.transform);
        //SkullClone.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
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

    public void CameraOpt()
    {
        float Horizontal;

        cam.GetComponent<CameraFollower>().target = null;

        Horizontal = 0.8f * Input.GetAxis("Mouse X");

        Horizontal = Mathf.Clamp(Horizontal, -2, 2);

        Debug.Log(Horizontal);

        float cameraPosX = cam.transform.position.x;

        cam.transform.localPosition = new Vector3(cameraPosX + (-Horizontal), cam.transform.position.y, cam.transform.position.z);

    }
    public void LevelTrue()
    {
        GameManager.Instance.LevelState(true);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("FinishPoint"))
        {

        }
    }

}
