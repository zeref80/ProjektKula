using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mass : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log(rb.mass);
    }
}

    // Update is called once per frame


