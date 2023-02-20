using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewtonsCradle : Interactable
{

    public int mode;
    public bool dontStop;
    public bool startNow;

    Transform ball1;
    Transform ball2;

    float amptitude;

    float sinIndexBall;

    int ballMode;

    //public GameObject prompt;

    public AudioSource cradle;

    void Start()
    {
        ball1 = transform.Find("ball1");
        ball2 = transform.Find("ball5");

        dontStop = true;
        startNow = false;

        ballMode = 0;
        mode = 0;
    }
    void doBallMotion()
    {
        //-- ball left is moving
        if (ballMode == 0)
        {
            float angle = 0.0f;
            angle = Mathf.Sin(sinIndexBall) * 60.0f * amptitude;
            sinIndexBall += Time.deltaTime * 3.0f;
            if (sinIndexBall >= Mathf.PI)
            {
                ballMode = 1;
                sinIndexBall = 0.0f;
            }
            ball1.localEulerAngles = new Vector3(0.0f, 0.0f, angle);
            ball2.localEulerAngles = Vector3.zero;
        }
        //-- ball right is moving
        else
        {
            float angle = 0.0f;
            angle = Mathf.Sin(sinIndexBall) * 60.0f * amptitude;
            sinIndexBall += Time.deltaTime * 3.0f;
            if (sinIndexBall >= Mathf.PI)
            {
                ballMode = 0;
                sinIndexBall = 0.0f;
            }
            ball2.localEulerAngles = new Vector3(0.0f, 0.0f, -angle);
            ball1.localEulerAngles = Vector3.zero;
        }
        if (!dontStop)
            amptitude -= Time.deltaTime * 0.05f;
    }
    void Update()
    {
        if (mode == 1)
        {

            doBallMotion();
            if (amptitude <= 0.0f)
            {
                ball1.localEulerAngles = Vector3.zero;
                ball2.localEulerAngles = Vector3.zero;
                mode = 0;
                cradle.Stop();
            }
        }

    }
    public override void OnFocus()
    {
        throw new System.NotImplementedException();
    }

    public override void OnInteract()
    {
        cradle.Play();

        if (mode == 0)
        {
            if (Input.GetMouseButton(0))
            {
                ballMode = 0;
                mode = 1;
                amptitude = 1.0f;
                sinIndexBall = Mathf.PI / 4.0f;
            }
            if (Input.GetMouseButton(1))
            {
                ballMode = 1;
                mode = 1;
                amptitude = 1.0f;
                sinIndexBall = Mathf.PI / 4.0f;
            }


        }
    }

    public override void OnLoseFocus()
    {
        throw new System.NotImplementedException();
    }
 
    
}
