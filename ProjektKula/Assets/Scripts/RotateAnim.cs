using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnim : MonoBehaviour
{
    public Vector3 rotateLocal;
    public float time;
    public bool isPingPong = false;
    int times = 0;
    bool isPlaying = false;
    Vector3 prevRot;
    Vector3 rotateTo;
    float timeToFinish;

    public void PlayAnim()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            rotateTo = Vector3.zero;
            prevRot = transform.localPosition;
            if (isPingPong)
            {
                if (times % 2 == 0)
                {
                    rotateTo = new Vector3(transform.localEulerAngles.x + rotateLocal.x, transform.localEulerAngles.y + rotateLocal.y, transform.localEulerAngles.z + rotateLocal.z);
                }
                else
                {
                    rotateTo = new Vector3(transform.localEulerAngles.x - rotateLocal.x, transform.localEulerAngles.y - rotateLocal.y, transform.localEulerAngles.z - rotateLocal.z);
                }
            }
            else
            {
                rotateTo = new Vector3(transform.localEulerAngles.x + rotateLocal.x, transform.localEulerAngles.y + rotateLocal.y, transform.localEulerAngles.z + rotateLocal.z);
            }
            times++;
            LeanTween.rotateLocal(this.gameObject, rotateTo, time).setOnComplete(SetNotIsPlaying);
        }
    }

    public void ForceAnim()
    {
        if (isPlaying)
        {
            LeanTween.cancel(this.gameObject);
            timeToFinish = time * Mathf.Abs(Vector3.Distance(transform.localEulerAngles, prevRot)) / Mathf.Abs(Vector3.Distance(Vector3.zero, rotateLocal));
            LeanTween.rotateLocal(this.gameObject, prevRot, timeToFinish).setOnComplete(SetNotIsPlaying);
            Vector3 backup = prevRot;
            prevRot = rotateTo;
            rotateTo = backup;
            times++;
        }
        else
        {
            PlayAnim();
        }
    }

    void SetNotIsPlaying()
    {
        isPlaying = false;
    }
}
