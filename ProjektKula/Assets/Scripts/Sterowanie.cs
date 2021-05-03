using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sterowanie : MonoBehaviour
{
    public bool active = true;
    public CharacterController characterControler;
    public Rigidbody characterRigid;
    public LayerMask groundMask;
    [SerializeField]
    private float predkoscPoruszania = 9.0f;
    [SerializeField]
    private float wysokoscSkoku = 7.0f;
    [SerializeField]
    private float predkoscOpadania = 2.0f;
    [SerializeField]
    private float aktualnaWysokoscSkoku = 0f;
    [SerializeField]
    private float predkoscBiegania = 7.0f;

    //Czulość myszki
    [SerializeField]
    private float czuloscMyszki = 3.0f;
    [SerializeField]
    private float myszGoraDol = 0.0f;
    //Zakres patrzenia w górę i dół.
    [SerializeField]
    private float zakresMyszyGoraDol = 90.0f;

    void Update()
    {
        klawiatura();
        myszka();
    }

    private void klawiatura()
    {
        float ruchPrzodTyl = 0;
        float ruchLewoPrawo = 0;
        if (active)
        {
            ruchPrzodTyl = Input.GetAxis("Vertical") * predkoscPoruszania;

            ruchLewoPrawo = Input.GetAxis("Horizontal") * predkoscPoruszania;

            if (isGrounded() && Input.GetButton("Jump"))
            {
                aktualnaWysokoscSkoku = wysokoscSkoku;
            }
            else if (isGrounded())
            {
                aktualnaWysokoscSkoku = 0;
            }
            else if (!isGrounded())
            {
                aktualnaWysokoscSkoku += Physics.gravity.y * Time.deltaTime * predkoscOpadania;
            }

            Debug.Log(isGrounded() + "  " + aktualnaWysokoscSkoku);

            if (Input.GetKeyDown("left shift"))
            {
                predkoscPoruszania += predkoscBiegania;
            }
            else if (Input.GetKeyUp("left shift"))
            {
                predkoscPoruszania -= predkoscBiegania;
            }
        }

        Vector3 ruch = new Vector3(ruchLewoPrawo, aktualnaWysokoscSkoku, ruchPrzodTyl);
        ruch = transform.rotation * ruch;

        RigidRepresentation();

        characterControler.Move(ruch * Time.deltaTime);
    }

    void RigidRepresentation()
    {
        float ruchPrzodTyl = 0;
        float ruchLewoPrawo = 0;

        float x = 0;
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            x = Mathf.Sqrt(1 / (Mathf.Pow(Input.GetAxis("Vertical"), 2) + Mathf.Pow(Input.GetAxis("Horizontal"), 2)));
        }

        ruchPrzodTyl = Input.GetAxis("Vertical") * predkoscPoruszania * x;

        ruchLewoPrawo = Input.GetAxis("Horizontal") * predkoscPoruszania * x;

        /*if (isGrounded() && Input.GetButton("Jump"))
        {
            aktualnaWysokoscSkoku = wysokoscSkoku;
        }
        else if (isGrounded())
        {
            aktualnaWysokoscSkoku = 0;
        }
        else if (!isGrounded())
        {
            aktualnaWysokoscSkoku += Physics.gravity.y * Time.deltaTime * predkoscOpadania;
        }*/

        if (Input.GetKeyDown("left shift"))
        {
            predkoscPoruszania += predkoscBiegania;
        }
        else if (Input.GetKeyUp("left shift"))
        {
            predkoscPoruszania -= predkoscBiegania;
        }

        Vector3 ruch = new Vector3(ruchLewoPrawo, aktualnaWysokoscSkoku, ruchPrzodTyl);
        ruch = transform.rotation * ruch;

        characterRigid.velocity = ruch;
    }

    private void myszka()
    {
        if (active)
        {
            float myszLewoPrawo = Input.GetAxis("Mouse X") * czuloscMyszki;
            transform.Rotate(0, myszLewoPrawo, 0);

            myszGoraDol -= Input.GetAxis("Mouse Y") * czuloscMyszki;
            //Funkcja nie pozwala aby wartość przekroczyła dane zakresy.
            myszGoraDol = Mathf.Clamp(myszGoraDol, -zakresMyszyGoraDol, zakresMyszyGoraDol);
            Camera.main.transform.localRotation = Quaternion.Euler(myszGoraDol, 0, 0);
        }
    }

    bool isGrounded()
    {
        Vector3 position = transform.TransformPoint(0, -1, 0);
        /*if (Physics.OverlapSphere(position, 1, groundMask).Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }*/
        return characterControler.isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.TransformPoint(0, -1, 0), 1);
    }

}