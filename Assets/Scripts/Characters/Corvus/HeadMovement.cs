using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// ------------- ///
// Micheal Corben
/// ------------- ///
public class HeadMovement : MonoBehaviour
{
    //The location of the first look target
    public Transform m_tfInitialRotation;

    //The location of the second look target
    public Transform m_tfSecondRotation;

    //The speed of rotation
    [Range (0.01f, 60)]
    public float m_fRotationSpeed;

    //The length of time inbetween the start of rotations before the direction is switched
    public float m_fTimeBetweenRotations;
    
    //The rotation the head is currently turning to
    Transform m_tfDesiredRotation;
    
    //The counter for the time between look locations, based off the look time variable
    float m_fTimerBetweenRotations = 0;
    
    void Start()
    {
        //Initialise variables
        //Set the Timer to the time to start the loop
        m_fTimerBetweenRotations = m_fTimeBetweenRotations;

        //Set the desired rotation of the vision cone to the second set rotation
        m_tfDesiredRotation = m_tfSecondRotation;
    }

    void Update()
    {
        //Create a vector to a target
        Vector3 m_v3VectorToTarget = m_tfDesiredRotation.position - transform.position;
        //If the cooldown of the rotation has ended
        if (m_fTimerBetweenRotations >= m_fTimeBetweenRotations)
        {
            //If the current rotation is the initial rotation
            if (m_v3VectorToTarget == m_tfInitialRotation.position - transform.position)
                {
                //Set the desired rotation to the secondary rotation
                    m_tfDesiredRotation = m_tfSecondRotation;
                //Set the timer to the begining
                    m_fTimerBetweenRotations = 0;
                }
                //Otherwise if the current rotation is the second rotation
            else if (m_v3VectorToTarget == m_tfSecondRotation.position - transform.position)
            {
                //Set the desired rotation to the initial rotation
                m_tfDesiredRotation = m_tfInitialRotation;

                //Set the timer to the begining
                m_fTimerBetweenRotations = 0;
                }
        }
        //Otherwise
        else
        {
            //Count up the cooldown timer
            m_fTimerBetweenRotations += Time.deltaTime;
        }
        //The angle used in determining the quatonion used for the rotation
        float m_fAngle = (Mathf.Atan2(m_v3VectorToTarget.y, m_v3VectorToTarget.x) * Mathf.Rad2Deg) - 90;

        //The quatonion used for the rotation
        Quaternion m_qAngle = Quaternion.AngleAxis(m_fAngle, Vector3.forward);

        //The rotation of the vision cone is modified by the slerp call based on the qutonion previously created
        transform.rotation = Quaternion.Slerp(transform.rotation, m_qAngle, Time.deltaTime * m_fRotationSpeed);
    }

}
