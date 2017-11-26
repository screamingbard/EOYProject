using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour {

    //-----------------------------------------------------------------------
    //Checks if the bird will be flying in a linear or sine wave fashion.
    //-----------------------------------------------------------------------
    private bool LinearOrSin = true;

    //-------------------------------------------------------------------------------------------
    //Speed:
    //These variables allow the birds to fly at different speeds based on random variables.
    //-------------------------------------------------------------------------------------------
    public float minSpeed = 15.0f;
    public float maxSpeed = 16.0f;

    //---------------------------------------
    //This holds the speed of each bird.
    //---------------------------------------
    private float fSpeed = 0;

    //--------------------------------------------------------------------------------------
    //These allow for changes in ther envioroment for the sin graph motion of the corvus.
    //--------------------------------------------------------------------------------------
    public float fGraphAmplitude = 2.0f;
    public float fGraphPeriod = 1.0f;

    void Awake ()
    {
        //Randomly sets the speed based on the maximum and minimum stated
        fSpeed = RandomSpeedGen();
	}


    void Update()
    {

        //This statement makes the birds fly in different ways depending on the randomly decided variable.
        if (LinearOrSin)    //linear movement for the corvus.
                LinearMovement();
            else    //Sinusoidal movement for the corvus.
                SinMovement();
    }

    //------------------------------------------------------------
    //sin movement for the bird to add some sort of ambience to
    //the scene rather than just having straight line movement.
    //------------------------------------------------------------
    void SinMovement()
    {
        //temporarily holds the transform so that the players transform can be modified without too much breaking.
        Vector3 temp = transform.position;

        //does the sin graph calculations for the corvus movements, both the x and the y of the corvus.
        temp.x += fSpeed * Time.deltaTime;
        temp.y = fGraphAmplitude * Mathf.Sin(temp.x * fGraphPeriod);

        //Adds the movement back onto the corvus
        transform.position = temp;
    }

    //------------------------------------------------------------
    //The linear movement for the corvus,
    //the corvus moves in a long line.
    //------------------------------------------------------------
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
