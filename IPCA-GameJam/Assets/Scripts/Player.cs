﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float maxSpeed = 3;
    public float speed = 50f;
    public float jumpPower = 250f;
	public bool grounded, jumpr, jumpl, mover, movel;
    public int weather = 1; //1 - Quente, 2 - Frio
    public bool weatherDelay = false;
    public bool death = false;
    public Vector2 spawn;
    public Vector2 checkp;
    public int weatherCounter = 0;
    public int weatherWait;
    public bool showE = false;
    public bool showASDF = false;
    public bool showClosed = false;
    public bool showIPush = false;
    public bool showItSays = false;
    public bool showPull = false;
    public bool hasGlasses = false;
    public bool firstLine = true;
    public bool finishLevel = false;
    public AudioClip deathRewind;
    public AudioClip weatherToHot;
    public AudioClip weatherToCold;
    public GameObject player;

    private Rigidbody2D rb;
    private Animator anim;

    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        spawn = new Vector2(-20f, 0f);

    }
	

	void Update () {
        //anim.SetBool("Grounded", grounded);
        //anim.SetFloat("Speed", Math.Abs(Input.GetAxis("Horizontal")));

        /*(Input.GetAxis("Horizontal") < -0.1f)
        {
            transform.localScale = new Vector3(, 1, 1);
        }

        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }*/

        checkp = player.GetComponent<CheckpointsCheck>().checkpoint;

        if (Input.GetKeyDown(KeyCode.H))
        {
            death = true;
        }


        if ((Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Z)) && !weatherDelay)
        {
            if (weather != 1)
            {
                weather = 1;
            }

        }

        if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.X) && !weatherDelay)
        {
            if (weather != 2)
            {
                weather = 2;
            }

        }

		anim.SetBool("jumpr", jumpr);
		anim.SetBool("jumpl", jumpl);
		anim.SetBool("mover", mover);
		anim.SetBool("movel", movel);
		anim.SetBool("glasses", hasGlasses);

		if (Input.GetButtonDown ("Jump") && grounded) {
			jumpr = true;
			rb.AddForce (Vector2.up * jumpPower);
		} else jumpr = false;

        if (death)
        {
            SoundManager.instance.PlaySingle(deathRewind);
            player.gameObject.transform.position = checkp;
            death = false;
        }
    }


    void DelayChange()
    {

        if (weatherCounter == 0)
        {
            weatherDelay = false;
        }
        else
        {
            weatherDelay = true;
        }
    }


    void FixedUpdate()
    {
		float moveHorizontal = Input.GetAxis("Horizontal");

		rb.AddForce(Vector2.right * speed * moveHorizontal);

		if (rb.velocity.x < 0) {
			movel = true;
			if (rb.velocity.x < -maxSpeed)
				rb.velocity = new Vector2 (-maxSpeed, rb.velocity.y);
			if (Input.GetButtonDown ("Jump") && grounded) {
				jumpl = true;
				rb.AddForce (Vector2.up * jumpPower);
			} else jumpl = false;
		} else movel = false;

		if (rb.velocity.x > 0) {
			mover = true;
			if (rb.velocity.x > maxSpeed)
				rb.velocity = new Vector2 (maxSpeed, rb.velocity.y);
			if (Input.GetButtonDown ("Jump") && grounded) {
				jumpr = true;
				rb.AddForce (Vector2.up * jumpPower);
			} else jumpr = false;
		} else mover = false;

        //Change Wheather
        if (weatherCounter > 0)
            weatherCounter--;

        DelayChange();

        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Z))
        {
            if (weather != 1)
            {
                SoundManager.instance.PlaySingle(weatherToHot);
                weather = 1;
                weatherCounter = weatherWait;
            }

        }

        if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.X))
        {
            if (weather != 2)
            {
                SoundManager.instance.PlaySingle(weatherToCold);
                weather = 2;
                weatherCounter = weatherWait;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Interagir
        if (other.gameObject.tag.Equals("E")) {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (showE == false) {
                    showE = true;
                    showASDF = false;
                    showClosed = false;
                    showIPush = false;
                    showItSays = false;
                    showPull = false;
                }else
                {
                    showE = false;
                }
            }
        }
        else if (other.gameObject.tag.Equals("Sign") && !hasGlasses)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (showASDF == false)
                {
                    showE = false;
                    showASDF = true;
                    showClosed = false;
                    showIPush = false;
                    showItSays = false;
                    showPull = false;
                }
                else
                {
                    showASDF = false;
                }
            }
        }
        else if (other.gameObject.tag.Equals("Door") && !hasGlasses && !firstLine)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (showClosed == false)
                {
                    showE = false;
                    showASDF = true;
                    showClosed = false;
                    showIPush = false;
                    showItSays = false;
                    showPull = false;
                    firstLine = true;
                }
                else
                {
                    showClosed = false;
                }
            }
        }
        else if (other.gameObject.tag.Equals("Door") && !hasGlasses && firstLine)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (showIPush == false)
                {
                    showE = false;
                    showASDF = true;
                    showClosed = false;
                    showIPush = false;
                    showItSays = false;
                    showPull = false;
                    firstLine = false;
                }
                else
                {
                    showIPush = false;
                }
            }
        }
        else if (other.gameObject.tag.Equals("Sign") && hasGlasses && firstLine)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (showItSays == false)
                {
                    showE = false;
                    showASDF = true;
                    showClosed = false;
                    showIPush = false;
                    showItSays = false;
                    showPull = false;
                    firstLine = false;
                }
                else
                {
                    showItSays = false;
                }
            }
        }
        else if (other.gameObject.tag.Equals("Sign") && hasGlasses && !firstLine)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (showPull == false)
                {
                    showE = false;
                    showASDF = true;
                    showClosed = false;
                    showIPush = false;
                    showItSays = false;
                    showPull = false;
                }
                else
                {
                    finishLevel = true;
                }
            }
        }
    }
}
