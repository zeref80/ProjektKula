using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnim : MonoBehaviour
{
    public Vector3 moveLocal;
    public float time;
    public bool isPingPong = false;
    int times = 0;
    bool isPlaying = false;
    Vector3 prevPos;
    Vector3 moveTo;
    float timeToFinish;

    public void PlayMoveAnim()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            moveTo = Vector3.zero;
            prevPos = transform.localPosition;
            if (isPingPong)
            {
                if (times % 2 == 0)
                {
                    moveTo = new Vector3(transform.localPosition.x + moveLocal.x, transform.localPosition.y + moveLocal.y, transform.localPosition.z + moveLocal.z);
                }
                else
                {
                    moveTo = new Vector3(transform.localPosition.x - moveLocal.x, transform.localPosition.y - moveLocal.y, transform.localPosition.z - moveLocal.z);
                }
            }
            else
            {
                moveTo = new Vector3(transform.localPosition.x + moveLocal.x, transform.localPosition.y + moveLocal.y, transform.localPosition.z + moveLocal.z);
            }
            times++;
            LeanTween.moveLocal(this.gameObject, moveTo, time).setOnComplete(SetNotIsPlaying);
        }
    }

    public void ForceMoveAnim()
    {
        if (isPlaying)
        {
            LeanTween.cancel(this.gameObject);
            timeToFinish = time * Mathf.Abs(Vector3.Distance(transform.localPosition, prevPos))/ Mathf.Abs(Vector3.Distance(Vector3.zero, moveLocal));
            LeanTween.moveLocal(this.gameObject, prevPos, timeToFinish).setOnComplete(SetNotIsPlaying);
            Vector3 backup = prevPos;
            prevPos = moveTo;
            moveTo = backup;
            times++;
        }
        else
        {
            PlayMoveAnim();
        }
    }

    void SetNotIsPlaying()
    {
        isPlaying = false;
    }
}
