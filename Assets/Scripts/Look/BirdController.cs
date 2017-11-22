using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour {

    public bool LinearOrSin = true;

    //star end locations
    public GameObject Start;
    public GameObject End;

    //Speed
    public float minSpeed = 15.0f;
    public float maxSpeed = 16.0f;

    private float fSpeed = 0;

    //graph sin
    public float fGraphAmplitude = 2.0f;
    public float fGraphPeriod = 1.0f;

    BirdbackgroundSummon bbgSummon;

    void Awake ()
    {
        fSpeed = RandomSpeedGen();
        bbgSummon = GetComponent<BirdbackgroundSummon>();
	}

    void Update() {
            if (LinearOrSin)
            {
                LinearMovement();
            }
            else
            {
                SinMovement();
            }
        }

    void SinMovement()
    {
        Vector3 temp = transform.position;

        temp.x += fSpeed * Time.deltaTime;
        temp.y = fGraphAmplitude * Mathf.Sin(temp.x * fGraphPeriod);

        transform.position = temp;
    }

    void LinearMovement()
    {
        //makes the bird move based on a speed and deltaTime
        transform.position += new Vector3(fSpeed * Time.deltaTime, 0, 0);
    }

    //--------------------------------------------------------------------------------------------------------------------
    //This method controls the speed of the roaming birds by randomly generating a number between the maximum and minimum.
    //Safety measures are put in.
    //--------------------------------------------------------------------------------------------------------------------
    float RandomSpeedGen()
    {
        //To make sure that the speed variable will not be overly broken
        //This changes the minimum speed to be the maximum if the minimum variable is larger than the maximum
        if (minSpeed > maxSpeed)
        {
            //temporarily stores the minimum speed for switching
            float temp = minSpeed;

            //Switches the min and max to the desired values
            minSpeed = maxSpeed;
            maxSpeed = temp;
        }

        //Randomly generates a number based on a range
        float speed = Random.Range(minSpeed, maxSpeed);

        //returns the speed
        return speed;
    }
}
