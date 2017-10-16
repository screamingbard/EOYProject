using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMovement : MonoBehaviour
{
    //The location of the first look position target
    public Transform m_tfInitialRotation;

    //
    public Transform m_tfSecondRotation;

    //
    [Range (0.01f, 60)]
    public float m_fRotationSpeed;

    //
    public float m_fTimeBetweenRotations;
    
    //The rotation the head is currently turning to
    Transform m_tfDesiredRotation;
    
    //
    float m_fTimerBetweenRotations = 0;
    
    // Use this for initialization
    void Start()
    {
        m_fTimerBetweenRotations = m_fTimeBetweenRotations;
        m_tfDesiredRotation = m_tfSecondRotation;
    }

    void Update()
    {
        Vector3 vectorToTarget = m_tfDesiredRotation.position - transform.position;
        //If the cooldown of the rotation has ended
        if (m_fTimerBetweenRotations >= m_fTimeBetweenRotations)
        {
            //If the current rotation is the initial rotation
            if (vectorToTarget == m_tfInitialRotation.position - transform.position)
                {
                    m_tfDesiredRotation = m_tfSecondRotation;
                    m_fTimerBetweenRotations = 0;
                }
                //Otherwise if the current rotation is the second rotation
            else if (vectorToTarget == m_tfSecondRotation.position - transform.position)
                {
                    m_tfDesiredRotation = m_tfInitialRotation;
                    m_fTimerBetweenRotations = 0;
                }
        }
        //Otherwise
        else
        {
            //Count up the cooldown timer
            m_fTimerBetweenRotations += Time.deltaTime;
        }
        float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * m_fRotationSpeed);
    }

}
