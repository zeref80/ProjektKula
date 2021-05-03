using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjektNaPlytce : MonoBehaviour
{
    public PlytkaNaciskowa plytka;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == null)
            return;

        AddObjekt(collision.rigidbody);
        if (collision.gameObject.GetComponent<ObjektNaPlytce>() == null)
        {
            ObjektNaPlytce ob = collision.gameObject.AddComponent<ObjektNaPlytce>();
            ob.plytka = plytka;
        }
        else
        {
            collision.gameObject.GetComponent<ObjektNaPlytce>().plytka = plytka;
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

        if (!plytka.objects.Contains(collision.rigidbody))
        {
            AddObjekt(collision.rigidbody);
            if (collision.gameObject.GetComponent<ObjektNaPlytce>() == null)
            {
                ObjektNaPlytce ob = collision.gameObject.AddComponent<ObjektNaPlytce>();
                ob.plytka = plytka;
            }
            else
            {
                collision.gameObject.GetComponent<ObjektNaPlytce>().plytka = plytka;
            }
        }
        else
        {
            if (collision.gameObject.GetComponent<ObjektNaPlytce>() == null)
            {
                ObjektNaPlytce ob = collision.gameObject.AddComponent<ObjektNaPlytce>();
                ob.plytka = plytka;
            }
        }
    }

    public void AddObjekt(Rigidbody ob)
    {
        plytka.AddObjekt(ob);
    }

    public void DelObjekt(Rigidbody ob)
    {
        plytka.DelObjekt(ob);
    }
}
