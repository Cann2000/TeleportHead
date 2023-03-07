using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Opt : MonoBehaviour
{
    [SerializeField] GameObject Head;

    [SerializeField] GameObject Skull;
    [SerializeField] Transform SkullPos;
    [SerializeField] GameObject SkullPosParent;

    Animator anim;

    public bool Finish;
    public bool FocusAnimEnd;
    

    void Start()
    {
        anim = GetComponent<Animator>();

        CameraOpt.instance.CamStartPos();
        CameraOpt.instance.CameraFowUpdateDown();
    }

    void Update()
    {
        if (!Finish)
        {
            if (Input.GetMouseButtonDown(0) && !FocusAnimEnd)
            {
                anim.SetTrigger("focus");

                StartCoroutine(skullspawn(0.2f));
            }

            if (!FocusAnimEnd)
            {
                CameraOpt.instance.CameraFowUpdateDown();

            }
        }
    }

    IEnumerator skullspawn(float time)
    {
        yield return new WaitForSeconds(time);

        SkullSpawn();
        Head.transform.localScale = Vector3.zero;

        FocusAnimEnd = true;

    }
    public void SkullSpawn()
    {
        GameObject SkullClone = Instantiate(Skull, SkullPos.position, Quaternion.identity);
        SkullClone.transform.SetParent(SkullPosParent.transform);
    }
    public void CharacterPos(Transform SkullPos,Quaternion CharacterRot)
    {
        transform.position = SkullPos.position;
        transform.localRotation = CharacterRot;

        Head.transform.localScale = new Vector3(1, 1, 1);

        FocusAnimEnd = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("FailArea"))
        {
            GameManager.Instance.LevelState(false);
            Destroy(gameObject);

        }
        if (collision.transform.CompareTag("FinishPoint"))
        {
            CameraOpt.instance.Invoke(nameof(CameraOpt.instance.LevelTrue), 1);
            Finish = true;
            Physics.gravity = new Vector3(0, -10, 0);
        }
    }

}
