using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AfterTime : MonoBehaviour
{
    public float time = 0;
    public UnityEvent thingsToHappen;
    float countdown = 0;

    private void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            if(thingsToHappen != null)
                thingsToHappen.Invoke();
        }
    }

    private void OnEnable()
    {
        countdown = time;
    }
}
