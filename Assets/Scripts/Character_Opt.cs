using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Opt : MonoBehaviour
{
    [SerializeField] GameObject Head;

    Animator anim;

    public bool Finish;
    public bool FocusAnimEnd;
    

    void Start()
    {
        anim = GetComponent<Animator>();
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
                Level_Opt.Instance.CameraFowUpdateDown();

            }
        }
    }

    IEnumerator skullspawn(float time)
    {
        yield return new WaitForSeconds(time);

        Level_Opt.Instance.SkullSpawn();
        Head.transform.localScale = new Vector3(0, 0, 0);

        FocusAnimEnd = true;

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
        if (collision.transform.CompareTag("lav"))
        {
            GameManager.Instance.LevelState(false);
            Destroy(gameObject);

        }
        if (collision.transform.CompareTag("FinishPoint"))
        {
            Level_Opt.Instance.Invoke(nameof(Level_Opt.Instance.LevelTrue), 2);
            Finish = true;
            Physics.gravity = new Vector3(0, -10, 0);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //if (collision.transform.CompareTag("FinishPoint"))
        //{
        //    Level_Opt.Instance.Invoke(nameof(Level_Opt.Instance.LevelTrue), 2);
        //    Finish = true;
        //    Physics.gravity = new Vector3(0, -10, 0);
        //}
    }
}
