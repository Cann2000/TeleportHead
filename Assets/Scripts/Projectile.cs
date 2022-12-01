using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Camera cam;

    Vector3 CamPos;

    Animator anim;

    GameObject character;
    Character_Opt characterOpt;
    Quaternion CharacterRot;

    bool TouchedTheGround;
    bool ShootActive;
    bool HitOnplayer;


    // Drag Mechanic

    public int LineSegment = 10;
    int numberOfShots;

    Rigidbody rb;
    //LineRenderer LineVisual;

    Vector3 Hitpos;
    Vector3 trajectoryDistance;

    [SerializeField] Transform ShootPoint;
    RaycastHit hit;
    public LayerMask layer;

    public GameObject pointPrefab;
    public GameObject[] points;
    public int numberOfPoints;


    void Start()
    {
        characterOpt = GameObject.FindGameObjectWithTag("Player").GetComponent<Character_Opt>();
        character = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;

        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

        CamPos = cam.transform.localPosition;

        //Drag Mechanic
        rb = GetComponent<Rigidbody>();
        //LineVisual = GetComponent<LineRenderer>();
        //LineVisual.enabled = false;
        //LineVisual.positionCount = LineSegment;

        PointsSpawn();


    }

    void Update()
    {
        LaunchProjectile();

        if (TouchedTheGround && !characterOpt.Finish)
        {
            if (rb.velocity.magnitude < 25 && rb.velocity.magnitude > 0.1f)
            {

                rb.angularVelocity = Vector3.zero;

            }
            if (rb.velocity.magnitude <= 1f && TouchedTheGround)
            {
                rb.velocity = Vector3.zero;

                characterOpt.CharacterPos(transform, CharacterRot);

                TouchedTheGround = false;

                Destroy(gameObject);

            }
        }

        if (!ShootActive)
        {
            if (HitOnplayer)
            {
                //Level_Opt.Instance.CameraFowUpdateDown();
                cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, CamPos, 5  * Time.deltaTime);
                CameraOpt.Instance.Invoke("CameraFowUpdateDown", 0.2f);

            }
        }

    }

    void LaunchProjectile()
    {
        if (numberOfShots == 0)
        {

            if (Input.GetMouseButton(0))
            {
                Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);


                //LineVisual.enabled = true;
                HitOnplayer = false;
                ShootActive = true;

                if (Physics.Raycast(camRay, out hit, Mathf.Infinity, layer))
                {
                    CameraOpt.Instance.Invoke("CameraFowUpdateUp", 0.3f);

                    Vector2 vo = CalculateVelocty(hit.point, transform.position, 1);

                    CameraOpt.Instance.CameraMove();

                    Hitpos = -vo;

                    Visualize(vo);

                    if (trajectoryDistance.x > -12 && trajectoryDistance.x < 12)
                    {
                        ShootActive = false;
                    }

                    else
                    {
                        ShootActive = true;
                    }

                }

            }
            if (Input.GetMouseButtonUp(0) && characterOpt.FocusAnimEnd)
            {

                if (ShootActive)
                {
                    anim.SetTrigger("throw");
                    StartCoroutine(ThrowSkull(0.8f));
                }
                else
                {
                    HitOnplayer = true;

                    DestroyPoints();
                    PointsSpawn();


                }

            }

        }
    }
    void Visualize(Vector3 vo)
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            Vector3 pos = CalculatePosInTime(vo, i / -(float)LineSegment);

            //LineVisual.SetPosition(i, pos);

            points[i].transform.position = pos;
        }
    }

    Vector3 CalculateVelocty(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXz = distance;
        distanceXz.y = 0f;

        trajectoryDistance = distance;


        if (trajectoryDistance.x < -3)
        {
            character.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            character.transform.rotation = Quaternion.Euler(0, -90, 0);

        }

        float sY = distance.y;
        float sXz = distanceXz.magnitude;

        float Vxz = sXz * time;
        float Vy = (sY / time) + (-0.5f * Mathf.Abs(Physics.gravity.y) * time);

        Vector3 result = distanceXz.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

    Vector3 CalculatePosInTime(Vector3 vo, float time)
    {
        Vector3 Vxz = vo;
        Vxz.y = 0f;

        Vector3 result = ShootPoint.position + vo * time;
        float sY = (-0.45f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (vo.y * time) + ShootPoint.position.y;

        result.y = sY;

        return result;
    }
    void PointsSpawn()
    {
        points = new GameObject[numberOfPoints];

        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Instantiate(pointPrefab, transform.position, Quaternion.identity);
        }
    }
    void DestroyPoints()
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            
            Destroy(points[i].gameObject);
        }
    }
    IEnumerator ThrowSkull(float time)
    {
        yield return new WaitForSeconds(time);

        cam.GetComponent<CameraFollower>().target = transform;

        transform.SetParent(null);

        rb.isKinematic = false;

        rb.velocity = Hitpos;

        DestroyPoints();
        //LineVisual.enabled = false;

        numberOfShots++;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            TouchedTheGround = true;
            CharacterRot = collision.transform.localRotation;
        }
        if (collision.transform.CompareTag("FinishPoint"))
        {
            TouchedTheGround = true;
            CharacterRot = collision.transform.localRotation;
        }
        if (collision.transform.CompareTag("lav"))
        {
            GameManager.Instance.LevelState(false);
            Destroy(gameObject);

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            TouchedTheGround = false;
        }
    }

}
