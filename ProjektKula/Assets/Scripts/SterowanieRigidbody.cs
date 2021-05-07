using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class SterowanieRigidbody : MonoBehaviour
{
    public bool active = true;
    public Rigidbody characterRigid;
    public Camera playerCam;
    public LayerMask groundLayer;
    public bool isGrounded;
    [SerializeField]
    private float maxSlopeAngle = 45.0f;
    private bool isSlopeGood;
    [Header("Ruch:")]
    [SerializeField]
    private float predkoscPoruszania = 9.0f;
    [SerializeField]
    private float predkoscBiegania = 7.0f;
    [Header("Skok:")]
    [SerializeField]
    private float wysokoscSkoku = 7.0f;
    [SerializeField]
    private float predkoscOpadania = 2.0f;
    private float aktualnaWysokoscSkoku = 0f;

    [Header("Myszka:")]
    //Czulość myszki
    [SerializeField]
    private float czuloscMyszki = 3.0f;
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

            if (isGrounded && Input.GetButton("Jump"))
            {
                aktualnaWysokoscSkoku = wysokoscSkoku;
            }
            else if (isGrounded && isSlopeGood)
            {
                aktualnaWysokoscSkoku = 0;
            }
            else if (!isGrounded || !isSlopeGood)
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

        isGrounded = IsGrounded();
        isSlopeGood = SlopeTest();
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

    bool IsGrounded()
    {
        float bodyHeight = GetComponent<CapsuleCollider>().height - 0.2f;
        float point = (-1f * ((GetComponent<CapsuleCollider>().height / 2f) - GetComponent<CapsuleCollider>().radius)) / (bodyHeight / 2f);
        Vector3 position = transform.TransformPoint(0, point, 0);
        if (Physics.OverlapSphere(position, GetComponent<CapsuleCollider>().radius * transform.localScale.y + 0.00001f, groundLayer).Length > 0){
            return true;
        }
        else
        {
            return false;
        }
    }

    bool SlopeTest()
    {
        float bodyHeight = GetComponent<CapsuleCollider>().height - 0.2f;
        if (Physics.Raycast(transform.TransformPoint(0, 0, 0), Vector3.down, out RaycastHit hit, ((GetComponent<CapsuleCollider>().height / 2f) + 0.1f) * transform.localScale.y, groundLayer))
        {
            float angle = Vector3.Angle(-Vector3.down, hit.normal);
            if(angle >= 0 && angle <= maxSlopeAngle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float bodyHeight = GetComponent<CapsuleCollider>().height - 0.2f;
        float point = (-1f * ((GetComponent<CapsuleCollider>().height / 2f) - GetComponent<CapsuleCollider>().radius)) / (bodyHeight/2f);
        Gizmos.DrawWireSphere(transform.TransformPoint(0, point, 0), GetComponent<CapsuleCollider>().radius * transform.localScale.y + 0.00001f);

        Gizmos.color = Color.blue;
        point = (-1f * ((GetComponent<CapsuleCollider>().height / 2f) + 0.1f)) / (bodyHeight / 2f);
        Gizmos.DrawLine(transform.TransformPoint(0, 0, 0), new Vector3(transform.position.x,transform.position.y - ((GetComponent<CapsuleCollider>().height / 2f) + 0.1f) * transform.localScale.y, transform.position.z));
    }
}
