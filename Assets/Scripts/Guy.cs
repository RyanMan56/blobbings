using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guy : MonoBehaviour {

    public int guyNumber;
    public bool randomise = true;
    private GameObject bodyObj;
    private GameObject eyesObj;
    // How long the blink lasts
    private float blinkDuration;
    // Time between blinks
    private float blinkPause;
    private float blinkDurationCountdown = 0;
    private float blinkPauseCountdown = 0;
    private enum blinkStates { eyesOpen, eyesClosed }
    private blinkStates blinkState = blinkStates.eyesOpen;

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer eyesSpriteRenderer;
    private Sprite bodySprite;
    private Sprite eyesSprite;

    private Rigidbody2D rb;

    private int groundCollisions = 0;
    private bool canJump = false;

	// Use this for initialization
	void Start () {
        if (randomise)
        {
            guyNumber = Random.Range(1, 38);
        }

        bodyObj = gameObject;
        eyesObj = transform.Find("Eyes").gameObject;

        spriteRenderer = GetComponent<SpriteRenderer>();
        eyesSpriteRenderer = eyesObj.GetComponent<SpriteRenderer>();       

        bodySprite = Resources.Load<Sprite>("Images/" + guyNumber + "_body");     
        eyesSprite = Resources.Load<Sprite>("Images/" + guyNumber + "_eyes");
        spriteRenderer.sprite = bodySprite;
        eyesSpriteRenderer.sprite = eyesSprite;

        bodyObj.AddComponent<PolygonCollider2D>();

        blinkDuration = Random.Range(0.25f, 0.5f);
        blinkPause = Random.Range(3, 5);

        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        Blink();
        LookTowardsCursor();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            groundCollisions++;
            canJump = true;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            groundCollisions--;
            if (groundCollisions <= 0)
            {
                canJump = false;
            }
        }
    }

    void Blink()
    {
        if (blinkPauseCountdown <= 0 && blinkState == blinkStates.eyesOpen)
        {
            blinkState = blinkStates.eyesClosed;
            eyesSprite = Resources.Load<Sprite>("Images/" + guyNumber + "_eyes_closed");
            eyesSpriteRenderer.sprite = eyesSprite;
            blinkDurationCountdown = blinkDuration;
        }
        if (blinkDurationCountdown <= 0 && blinkState == blinkStates.eyesClosed)
        {
            blinkState = blinkStates.eyesOpen;
            eyesSprite = Resources.Load<Sprite>("Images/" + guyNumber + "_eyes");
            eyesSpriteRenderer.sprite = eyesSprite;
            blinkPauseCountdown = blinkPause;
        }
        blinkPauseCountdown -= Time.deltaTime;
        blinkDurationCountdown -= Time.deltaTime;
    }

    void LookTowardsCursor()
    {
        //var cameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //var eyesPos = eyesObj.transform.position;
    }

    void Move()
    {
        if (Input.GetAxis("Vertical") > 0 && canJump)
        {
            rb.AddForce(new Vector2(0, 2), ForceMode2D.Impulse);
        }
        if (Input.GetAxis("Horizontal") > 0 && canJump)
        {
            rb.AddForce(new Vector2(1, 1), ForceMode2D.Impulse);
        }
        if (Input.GetAxis("Horizontal") < 0 && canJump)
        {
            rb.AddForce(new Vector2(-1, 1), ForceMode2D.Impulse);
        }        
    }
}
