using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private GameObject blobs;
    private GameObject selectedBlob;
    private GameObject arrow;
    private GameObject arrowSpriteMask;    
    private bool arrowCharging = false;
    private float currentCharge = 4.54f;
    private float chargingSpeed = 5f;
    private float arrowMinX = 4.54f;
    private float arrowMaxX = 7.46f;
    private float chargeAngle;
    private float maxForce = 5.0f;

	// Use this for initialization
	void Start () {
        blobs = GameObject.Find("Blobs");        
        arrow = GameObject.Find("Arrow");
        arrowSpriteMask = arrow.transform.Find("SpriteMask").gameObject;

        arrow.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {        
        if (Input.GetMouseButtonDown(0))
        {
            GetRandomBlob();
            //arrow.transform.parent = selectedBlob.transform;
            currentCharge = arrowMinX;            
            arrowCharging = true;
        }
        if (selectedBlob != null)
        {
            PositionArrow();
        }
        if (Input.GetMouseButton(0))
        {
            arrow.SetActive(true);
            AngleArrow();
        }
        if (Input.GetMouseButtonUp(0))
        {
            arrow.SetActive(false);
            arrowCharging = false;
            CalculateImpulse();
        }
        if (arrowCharging)
        {
            if (currentCharge < arrowMinX)
            {
                chargingSpeed = Mathf.Abs(chargingSpeed);
            }
            if (currentCharge > arrowMaxX)
            {
                chargingSpeed = -Mathf.Abs(chargingSpeed);
            }
            currentCharge += chargingSpeed * Time.deltaTime;
            arrowSpriteMask.transform.localPosition = new Vector2(currentCharge, arrowSpriteMask.transform.localPosition.y);
        }
	}

    private void GetRandomBlob()
    {
        selectedBlob = blobs.transform.GetChild(Random.Range(0, blobs.transform.childCount)).gameObject;
    }

    private void PositionArrow()
    {
        arrow.transform.position = selectedBlob.transform.position;
    }

    private void AngleArrow()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var arrowPos = arrow.transform.position;

        chargeAngle = Mathf.Atan2((mousePos.y - arrowPos.y), (mousePos.x - arrowPos.x)) * Mathf.Rad2Deg;
        arrow.transform.localRotation = Quaternion.Euler(0, 0, chargeAngle);
    }

    private void CalculateImpulse()
    {
        var strength = (currentCharge - arrowMinX) / (arrowMaxX - arrowMinX);
        var actualStrength = maxForce * strength;

        var xStrength = actualStrength * Mathf.Cos(chargeAngle * Mathf.Deg2Rad);// * Mathf.Rad2Deg;
        var yStrength = actualStrength * Mathf.Sin(chargeAngle * Mathf.Deg2Rad);// * Mathf.Rad2Deg;

        Debug.Log("s: " + actualStrength + " angle: " + chargeAngle + " x: " + xStrength + " y: " + yStrength);

        selectedBlob.GetComponent<Rigidbody2D>().AddForce(new Vector2(xStrength, yStrength), ForceMode2D.Impulse);
    }
}
