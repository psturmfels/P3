using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeController : MonoBehaviour {
    public GameObject mellowMove;
    public EyeMovement leftEye;
    public EyeMovement rightEye;

    private FaceMoveWhenTransformed fmwt;
    private MellowCrushed mellowCrushed;
    private Rigidbody2D rb;
    private Vector3 originalPosition;

    // Eye Sprites
    public Sprite eyeSprite;
//    public Sprite largeIris;
//    public Sprite smallIris;
    public Sprite xEyeSprite;

    // Use this for initialization
    void Start() {
        fmwt = GetComponentInParent<FaceMoveWhenTransformed>();
        rb = GetComponentInParent<Rigidbody2D>();

        Invoke("Blink", 4.0f + Random.Range(0f, 4f));
        mellowCrushed = mellowMove.GetComponentInParent<MellowCrushed>();
        mellowCrushed.Remove += DisableEyes;
        mellowCrushed.Respawn += EnableEyes;
        mellowCrushed.HazardContacted += ChangeToXEyes;
        originalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update() {
        Vector3 lookingDirection = transform.position;
        if (mellowMove.activeSelf) {
            transform.localPosition = originalPosition;
            lookingDirection = rb.velocity;
        }
        else {
            if (fmwt.offsetAxis == Vector2.right) {
                if (transform.parent.localPosition.x > 0) {
                    lookingDirection = Vector3.right;
                }
                else if (transform.parent.localPosition.x < 0) {
                    lookingDirection = Vector3.left;
                }
                else {
                    lookingDirection = Vector3.zero;
                }
            }
            else if (fmwt.offsetAxis == Vector2.up) {
                if (transform.parent.localPosition.y > 0) {
                    lookingDirection = Vector3.up;
                }
                else if (transform.parent.localPosition.y < 0) {
                    lookingDirection = Vector3.down;
                }
                else {
                    lookingDirection = Vector3.zero;
                }
            }
        }
        leftEye.LookAt(lookingDirection);
        rightEye.LookAt(lookingDirection);
    }

    private void Blink() {
        GetComponent<Animator>().SetTrigger("blink");
        Invoke("Blink", 4f + Random.Range(0f, 4f));
    }

    private void DisableEyes() {
        gameObject.SetActive(false);
        leftEye.transform.localScale = 0.8f * Vector3.one;
        rightEye.transform.localScale = 0.8f * Vector3.one;
    }

    private void EnableEyes() {
        gameObject.SetActive(true);
        leftEye.transform.GetChild(1).gameObject.SetActive(true);
        leftEye.transform.GetChild(2).gameObject.SetActive(true);
        rightEye.transform.GetChild(1).gameObject.SetActive(true);
        rightEye.transform.GetChild(2).gameObject.SetActive(true);
        ChangeEyeSprites(eyeSprite);
        leftEye.transform.localScale = 0.8f * Vector3.one;
        rightEye.transform.localScale = 0.8f * Vector3.one;
    }

    private void ChangeToXEyes() {
        leftEye.transform.GetChild(1).gameObject.SetActive(false);
        leftEye.transform.GetChild(2).gameObject.SetActive(false);
        rightEye.transform.GetChild(1).gameObject.SetActive(false);
        rightEye.transform.GetChild(2).gameObject.SetActive(false);

        ChangeEyeSprites(xEyeSprite);
    }

    private void ChangeEyeSprites(Sprite s) {
        SpriteRenderer leftEyeSprite = leftEye.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        SpriteRenderer rightEyeSprite = rightEye.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        leftEyeSprite.sprite = s;
        rightEyeSprite.sprite = s;
    }
}
