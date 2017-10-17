using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    //What the camera is following
    public Transform m_tfTarget;

    //The size of the bounding box
    public Vector2 m_v2FocusAreaSize;

    //The height of the camera relative to the centre of the bounding box
    public float m_fVerticalOffset;

    //The minimum height the camera will follow the character
    public float m_fMinimumCameraHeight = -5;

    //
    FocusArea m_faFocusArea;

    void Start()
    {
        //Generate the bounding box
        m_faFocusArea = new FocusArea(m_tfTarget.GetComponent<Collider2D>().bounds, m_v2FocusAreaSize);
    }
    void LateUpdate()
    {
        //Call the FocusArea Update method
        m_faFocusArea.Update(m_tfTarget.GetComponent<Collider2D>().bounds);

        //The position of the bounding box around the player
        Vector2 focusPosition = m_faFocusArea.m_v2Centre + Vector2.up * m_fVerticalOffset;

        //The position of the camera
        transform.position = (Vector3) focusPosition + Vector3.forward * -10;

        //This if check keeps the camera from falling below what the player should be seeing
        if (transform.position.y < m_fMinimumCameraHeight)
        {
            transform.position = new Vector3(transform.position.x, m_fMinimumCameraHeight, transform.position.z);
        }
    }   

    void OnDrawGizmos()
    //An in editor visual aid for building the bounding box
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(m_faFocusArea.m_v2Centre, m_v2FocusAreaSize);
    }
    struct FocusArea
    {
        //The centre position of the bounding box
        public Vector2 m_v2Centre;

        //The velocity of the bounding box
        public Vector2 m_v2Velocity;

        //The width of the bounding box
        float m_fLeft, m_fRight;

        //The height of the bounding box
        float m_fTop, m_fBottom;

        public FocusArea(Bounds a_bnTargetsBounds, Vector2 m_v2Size)
        {
            //Set the bounds of the box around the player
            m_fLeft = a_bnTargetsBounds.center.x - m_v2Size.x / 2;
            m_fRight = a_bnTargetsBounds.center.x + m_v2Size.x / 2;
            m_fBottom = a_bnTargetsBounds.min.y;
            m_fTop = a_bnTargetsBounds.min.y + m_v2Size.y;
            
            //Stores the velocity
            m_v2Velocity = Vector2.zero;

            //Stores the centre for tracking
            m_v2Centre = new Vector2((m_fLeft + m_fRight) / 2, (m_fTop + m_fBottom) / 2);
        }
        public void Update(Bounds a_bnTargetsBounds)
        {
            //The amount the camera is shifted along the x axis
            float m_fShiftX = 0;

            //If the player touches an edge along the x axis
            if (a_bnTargetsBounds.min.x < m_fLeft)
            {
                //Shift the camera along the x
                m_fShiftX = a_bnTargetsBounds.min.x - m_fLeft;
            }
            //If the player touches an edge along the x axis
            else if (a_bnTargetsBounds.max.x > m_fRight)
            {
                //Shift the camera along the x
                m_fShiftX = a_bnTargetsBounds.max.x - m_fRight;
            }
            m_fLeft += m_fShiftX;
            m_fRight += m_fShiftX;

            //The amount the camera is shifted along the y axis
            float m_fShiftY = 0;

            //If the player touches an edge along the y axis
            if (a_bnTargetsBounds.min.y < m_fBottom)
            {
                //Shift the camera along the x
                m_fShiftY = a_bnTargetsBounds.min.y - m_fBottom;
            }

            //If the player touches an edge along the y axis
            else if (a_bnTargetsBounds.max.y > m_fTop)
            {
                //Shift the camera along the x
                m_fShiftY = a_bnTargetsBounds.max.y - m_fTop;
            }

            m_fTop += m_fShiftY;
            m_fBottom += m_fShiftY;

            //Set the centre of the bounded camera
            m_v2Centre = new Vector2((m_fLeft + m_fRight) / 2, (m_fTop + m_fBottom) / 2);
            
            //Set the velocity of the camera
            m_v2Velocity = new Vector2(m_fShiftX, m_fShiftY);
        }
    }

    
    public void ResetCamera()
    //Resets the camera position to the centre of the player when called
    {
        m_faFocusArea = new FocusArea(m_tfTarget.GetComponent<Collider2D>().bounds, m_v2FocusAreaSize);
    }
}
