﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sterowanie : MonoBehaviour
{
    public CharacterController characterControler;
    public Rigidbody characterRigid;
    public bool active = true;
    [SerializeField]
    private float predkoscPoruszania = 9.0f;
    [SerializeField]
    private float wysokoscSkoku = 7.0f;
    [SerializeField]
    private float predkoscOpadania = 2.0f;
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

            if (characterControler.isGrounded && Input.GetButton("Jump"))
            {
                aktualnaWysokoscSkoku = wysokoscSkoku;
            }
            else if (!characterControler.isGrounded)
            {
                aktualnaWysokoscSkoku += Physics.gravity.y * Time.deltaTime * predkoscOpadania;
            }

            //Debug.Log(Physics.gravity.y);

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

        characterControler.Move(ruch * Time.deltaTime);
        characterRigid.velocity = ruch * Time.deltaTime;
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

}