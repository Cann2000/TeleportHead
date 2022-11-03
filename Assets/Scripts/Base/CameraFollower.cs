using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] private float lerpSpeed = 0.125f;
    public Vector3 offset;
    Vector3 lerpPos;
    public Transform target;

    private void Start()
    {
    }

    void LateUpdate() => CameraMove();

    void CameraMove()
    {
        if (target == null) return;

        lerpPos = Vector3.Lerp(transform.localPosition, target.localPosition, lerpSpeed) + offset;
        transform.localPosition = lerpPos;


    }
}
