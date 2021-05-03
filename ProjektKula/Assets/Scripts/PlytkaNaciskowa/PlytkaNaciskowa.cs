using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
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

    public List<Rigidbody> objects = new List<Rigidbody>();
    /*float scaleY;
    float positionY;*/

    float totalMass = 0;
    bool isBelow = true;

    private void Start()
    {
        objects = new List<Rigidbody>();
        /*scaleY = transform.localScale.y;
        positionY = transform.localPosition.y;*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == null)
            return;

        AddObjekt(collision.rigidbody);
        if (collision.gameObject.GetComponent<ObjektNaPlytce>() == null)
        {
            ObjektNaPlytce ob = collision.gameObject.AddComponent<ObjektNaPlytce>();
            ob.plytka = this;
        }
        else
        {
            collision.gameObject.GetComponent<ObjektNaPlytce>().plytka = this;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.rigidbody == null)
            return;

        DelObjekt(collision.rigidbody);
        Destroy(collision.gameObject.GetComponent<ObjektNaPlytce>());
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.rigidbody == null)
            return;

        if (!objects.Contains(collision.rigidbody))
        {
            AddObjekt(collision.rigidbody);
            if (collision.gameObject.GetComponent<ObjektNaPlytce>() == null)
            {
                ObjektNaPlytce ob = collision.gameObject.AddComponent<ObjektNaPlytce>();
                ob.plytka = this;
            }
            else
            {
                collision.gameObject.GetComponent<ObjektNaPlytce>().plytka = this;
            }
        }
        else
        {
            if(collision.gameObject.GetComponent<ObjektNaPlytce>() == null)
            {
                ObjektNaPlytce ob = collision.gameObject.AddComponent<ObjektNaPlytce>();
                ob.plytka = this;
            }
        }
    }

    public void AddObjekt(Rigidbody ob)
    {
        objects.Add(ob);
        CheckMass();
    }

    public void DelObjekt(Rigidbody ob)
    {
        if (objects.Contains(ob))
        {
            objects.RemoveAll(obj => obj == ob);
            CheckMass();
        }
    }

    public void CheckMass()
    {
        // Towrzenie listy unikalnych
        List<Rigidbody> unikalne = new List<Rigidbody>();
        foreach(var ob in objects)
        {
            if (!unikalne.Contains(ob))
            {
                unikalne.Add(ob);
            }
        }

        // Calculate Mass
        totalMass = 0;
        foreach(var objekt in unikalne)
        {
            totalMass += objekt.mass;   
        }


        // Do if have enought mass
        if(totalMass >= massToInteract && isBelow)
        {
            LeanTween.cancel(this.gameObject);
            thingsToHappenOnEnter.Invoke();
            //float timeToEnd = 1f * (transform.localScale.y - 0.5f * scaleY) / scaleY;
            //LeanTween.scaleY(this.gameObject, 0.5f * scaleY, timeToEnd);
            //LeanTween.moveLocalY(this.gameObject, positionY - 0.5f * scaleY, timeToEnd);
            isBelow = false;
        }
        else if (totalMass < massToInteract && !isBelow)
        {
            LeanTween.cancel(this.gameObject);
            thingsToHappenOnExit.Invoke();
            //float timeToEnd = 1f * (scaleY - transform.localScale.y) / scaleY;
            //LeanTween.scaleY(this.gameObject, scaleY, timeToEnd);
            //LeanTween.moveLocalY(this.gameObject, positionY, timeToEnd);
            isBelow = true;
        }
    }
}
