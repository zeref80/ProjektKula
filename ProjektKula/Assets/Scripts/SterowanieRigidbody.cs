using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SterowanieRigidbody : MonoBehaviour
{
    public Rigidbody characterRigid;
    public Camera playerCam;
    public bool active = true;
    [SerializeField]
    private float predkoscPoruszania = 9.0f;
    [SerializeField]
    private float predkoscBiegania = 7.0f;
    [SerializeField]
    private float wysokoscSkoku = 7.0f;
    [SerializeField]
    private float predkoscOpadania = 2.0f;
    private float aktualnaWysokoscSkoku = 0f;

    //Czulość myszki
    [SerializeField]
    private float czuloscMyszki = 3.0f;
    [SerializeField]
    private float myszGoraDol = 0.0f;
    //Zakres patrzenia w górę i dół.
    [SerializeField]
    private float zakresMyszyGoraDol = 90.0f;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        klawiatura();
        myszka();
    }

    void klawiatura()
    {
        if (active)
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
        else
        {
            characterRigid.velocity = Vector3.zero;
        }
    }

    void myszka()
    {
        if (active)
        {
            float myszLewoPrawo = Input.GetAxis("Mouse X") * czuloscMyszki;
            transform.Rotate(0, myszLewoPrawo, 0);

            myszGoraDol -= Input.GetAxis("Mouse Y") * czuloscMyszki;
            //Funkcja nie pozwala aby wartość przekroczyła dane zakresy.
            myszGoraDol = Mathf.Clamp(myszGoraDol, -zakresMyszyGoraDol, zakresMyszyGoraDol);
            playerCam.transform.localRotation = Quaternion.Euler(myszGoraDol, 0, 0);
        }
    }

    bool isGrounded()
    {
        Vector3 position = transform.TransformPoint(0, -1, 0);
        string[] maskParms = { "Default", "TransparentFX", "Ignore Raycast", "Water", "PostProcesing" };
        if (Physics.OverlapSphere(position, 1, LayerMask.GetMask(maskParms)).Length > 0){
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.TransformPoint(0, -1, 0), 1);
    }
}
