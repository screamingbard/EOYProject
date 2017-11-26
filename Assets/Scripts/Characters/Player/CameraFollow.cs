using System.Collections;
using System.Collections.Generic;
using XboxCtrlrInput;
using UnityEngine;

/// ------------- ///
// Micheal Corben
/// ------------- ///
public class CameraFollow : MonoBehaviour {

    //What the camera is following
    public Transform m_tfTarget;

    //The size of the bounding box
    public Vector2 m_v2FocusAreaSize;

    //Stores the players input for detection to control the desired direction of the look ahead direction
    Vector2 playerInput;

    //The height of the camera relative to the centre of the bounding box
    public float m_fVerticalOffset;

    //The distance the camera moves along the players forward from the player
    public float m_fLookAheadDistanceX;

    //The time it takes to smooth the look ahead of the camera along the x axis
    public float m_fLookSmoothTimeX;

    //The time it takes to smooth the look ahead of the camera along the y axis
    public float m_fVerticalSmoothTime;

    //A speed control of the camera movement along the x axis
    public float m_fSmoothMultiplierX;

    //A speed control of the camera movement along the y axis
    public float m_fSmoothMultiplierY;

    //The getter and setter of the min camera height
    public float m_fMinCamHeight
    {
        get
        {
            return m_fMinimumCameraHeight;
        }
        set
        {
            m_fMinimumCameraHeight = m_fMinCamHeight;
        }
    }

    //The getter and setter of the max camera height
    public float m_fMaxCamHeight
    {
        get
        {
            return m_fMinimumCameraHeight;
        }
        set
        {
            m_fMinimumCameraHeight = m_fMaxCamHeight;
        }
    }

    //The minimum height the camera will follow the character
    public float m_fMinimumCameraHeight = -50;

    //The minimum height the camera will follow the character
    public float m_fMaximumCameraHeight = 30;

    //The current position of the position along the x axis
    float m_fCurrentLookAheadX;

    //The desired position of the camera along the x axis
    float m_fTargetLookAheadX;

    //The distance and direction from the player the middle of the camera along the x axis
    float m_fLookAheadDirectionX;

    //The speed of the smoothing of the camera along the x axis
    float m_fSmoothLookVelocityX;

    //The speed of the smoothing of the camera along the y axis
    float m_fSmoothVelocityY;

    //The bounds around the player which controls the movemnt of the camera
    FocusArea m_faFocusArea;

    //Is the camera moving towards the look ahead
    bool m_bLookAheadStopped;
    
    void Start()
    {
        //Generate the bounding box
        m_faFocusArea = new FocusArea(m_tfTarget.GetComponent<Collider2D>().bounds, m_v2FocusAreaSize, m_fSmoothMultiplierX, m_fSmoothMultiplierY);
    }
    void Update()
    {
        //Set input variables for checking other methods
        playerInput.x = XCI.GetAxis(XboxAxis.LeftStickX);
        playerInput.y = XCI.GetAxis(XboxAxis.LeftStickY);
    }
    
    void LateUpdate()
    {
        //Call the FocusArea Update method
        m_faFocusArea.Update(m_tfTarget.GetComponent<Collider2D>().bounds);

        //The position of the bounding box around the player
        Vector2 focusPosition = m_faFocusArea.m_v2Centre + Vector2.up * m_fVerticalOffset;

        //If the focus are is moving along the x axis
        if (m_faFocusArea.m_v2Velocity.x != 0)
        {
            //Set the distance of the look ahead of the camera which is how far the camera is to the left or right depending on movement
            m_fLookAheadDirectionX = Mathf.Sign(m_faFocusArea.m_v2Velocity.x);
            //If the player is providing horizontal movement input and the velocity of the focus are along the x axis is relative to the player input along the x
            if (Mathf.Sign(playerInput.x) == Mathf.Sign(m_faFocusArea.m_v2Velocity.x) && playerInput.x != 0)
            {
                //Set the look ahead bool to false
                m_bLookAheadStopped = false;
                //Set the look ahead along the x axis to the directtion on the x and the distance also along the x of the look ahead
                m_fTargetLookAheadX = m_fLookAheadDirectionX * m_fLookAheadDistanceX;
            }
            else
            {
                //Otherwise if the look ahead is not stopped
                if (!m_bLookAheadStopped)
                {
                    //Set the look ahead to true
                    m_bLookAheadStopped = true;
                    //Set the look ahead target to the current look ahead plus the look ahead direction
                    //Multiplied by the Look ahead distance minus the current look ahead direction devided by 4
                    m_fTargetLookAheadX = m_fCurrentLookAheadX + (m_fLookAheadDirectionX * m_fLookAheadDistanceX - m_fCurrentLookAheadX) / 4;
                }
            }
        }
        //Smooth the transition of the look ahead based of the current speed of the camera relative to the focus area and the smooth time
        m_fCurrentLookAheadX = Mathf.SmoothDamp(m_fCurrentLookAheadX, m_fTargetLookAheadX, ref m_fSmoothLookVelocityX, m_fLookSmoothTimeX);

        //Smooth the focus position movement based of the movement of the focus area and the smooth time
        focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref m_fSmoothVelocityY, m_fVerticalSmoothTime);

        //Change the focus area based off the current look ahead
        focusPosition += Vector2.right * m_fCurrentLookAheadX;

        //The position of the camera
        transform.position = (Vector3) focusPosition + Vector3.forward * -10;
        

        //This if check keeps the camera from existing near what the player shouldn't be seeing
        if (transform.position.y < m_fMinimumCameraHeight)
        {
            transform.position = new Vector3(transform.position.x, m_fMinimumCameraHeight, transform.position.z);
        }

        if (transform.position.y > m_fMaximumCameraHeight)
        {
            transform.position = new Vector3(transform.position.x, m_fMaximumCameraHeight, transform.position.z);
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

        //The multipliers for the smooth times
        float m_fSmoothMultX;
        float m_fSmoothMultY;

        //The width of the bounding box
        float m_fLeft, m_fRight;

        //The height of the bounding box
        float m_fTop, m_fBottom;

        public FocusArea(Bounds a_bnTargetsBounds, Vector2 m_v2Size, float a_fSmoothMultX, float a_fSmoothMultY)
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

            //Set the multipliers
            m_fSmoothMultX = a_fSmoothMultX;
            m_fSmoothMultY = a_fSmoothMultY;
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
            //Move the bounds of the focus area along the x axis
            m_fLeft += m_fShiftX * Time.deltaTime * m_fSmoothMultX;
            m_fRight += m_fShiftX * Time.deltaTime * m_fSmoothMultX;
            
            //The amount the camera is shifted along the y axis
            float m_fShiftY = 0;

            //If the player touches an edge along the y axis
            if (a_bnTargetsBounds.min.y < m_fBottom)
            {
                //Shift the camera along the Y
                m_fShiftY = a_bnTargetsBounds.min.y - m_fBottom;
            }

            //If the player touches an edge along the y axis
            else if (a_bnTargetsBounds.max.y > m_fTop)
            {
                //Shift the camera along the Y
                m_fShiftY = a_bnTargetsBounds.max.y - m_fTop;
            }

            //Move the bounds of the focus area along the y axis
            m_fTop += m_fShiftY * Time.deltaTime * m_fSmoothMultY;
            m_fBottom += m_fShiftY * Time.deltaTime * m_fSmoothMultY;

            //Set the centre of the bounded camera
            m_v2Centre = new Vector2((m_fLeft + m_fRight) / 2, (m_fTop + m_fBottom) / 2);
            
            //Set the velocity of the camera
            m_v2Velocity = new Vector2(m_fShiftX, m_fShiftY);
        }
    }


    public void ResetCamera()
    //Resets the camera position to the centre of the player when called
    {
       m_faFocusArea = new FocusArea(m_tfTarget.GetComponent<Collider2D>().bounds, m_v2FocusAreaSize, m_fSmoothMultiplierX, m_fSmoothMultiplierY);
    }
}
