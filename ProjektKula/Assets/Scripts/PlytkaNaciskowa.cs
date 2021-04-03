using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Ma pewne wady i błędy jak coś. Don't juge me Adam pls xddd
/// </summary>
public class PlytkaNaciskowa : MonoBehaviour
{
    public float massToInteract;
    public UnityEvent thingsToHappenOnEnter;
    public UnityEvent thingsToHappenOnExit;

    List<Rigidbody> objects;
    float scaleY;
    float positionY;

    float totalMass = 0;
    bool isBelow = true;

    private void Start()
    {
        objects = new List<Rigidbody>();
        scaleY = transform.localScale.y;
        positionY = transform.localPosition.y;
    }

    private void Update()
    {
        CheckMass();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == null)
            return;

        if (!objects.Contains(collision.rigidbody))
        {
            objects.Add(collision.rigidbody);
            totalMass += collision.rigidbody.mass;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.rigidbody == null)
            return;

        if (objects.Contains(collision.rigidbody))
        {
            objects.Remove(collision.rigidbody);
            totalMass -= collision.rigidbody.mass;
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() == null)
            return;

        if (!objects.Contains(other.GetComponent<Rigidbody>()))
        {
            objects.Add(other.GetComponent<Rigidbody>());
            totalMass += other.GetComponent<Rigidbody>().mass;
        }
    }*/

    /*private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>() == null)
            return;

        if (objects.Contains(other.GetComponent<Rigidbody>()))
        {
            objects.Remove(other.GetComponent<Rigidbody>());
            totalMass -= other.GetComponent<Rigidbody>().mass;
        }
    }*/

    void CheckMass()
    {
        if(totalMass >= massToInteract && isBelow)
        {
            LeanTween.cancel(this.gameObject);
            thingsToHappenOnEnter.Invoke();
            float timeToEnd = 1f * (transform.localScale.y - 0.5f * scaleY) / scaleY;
            LeanTween.scaleY(this.gameObject, 0.5f * scaleY, timeToEnd);
            LeanTween.moveLocalY(this.gameObject, positionY - 0.25f * scaleY, timeToEnd);
            isBelow = false;
        }
        else if (totalMass < massToInteract && !isBelow)
        {
            LeanTween.cancel(this.gameObject);
            thingsToHappenOnExit.Invoke();
            float timeToEnd = 1f * (scaleY - transform.localScale.y) / scaleY;
            LeanTween.scaleY(this.gameObject, scaleY, timeToEnd);
            LeanTween.moveLocalY(this.gameObject, positionY, timeToEnd);
            isBelow = true;
        }
    }
}
