using System.Collections;
using System.Collections.Generic;
using XboxCtrlrInput;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    //What the camera is following
    public Transform m_tfTarget;

    //The size of the bounding box
    public Vector2 m_v2FocusAreaSize;

    Vector2 playerInput;

    //The height of the camera relative to the centre of the bounding box
    public float m_fVerticalOffset;

    //
    public float m_fLookAheadDistanceX;

    //
    public float m_fLookSmoothTimeX;

    //
    public float m_fVerticalSmoothTime;

    //
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

    //
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
    float m_fMaximumCameraHeight = 30;

    //
    float m_fCurrentLookAheadX;

    //
    float m_fTargetLookAheadX;

    //
    float m_fLookAheadDirectionX;

    //
    float m_fSmoothLookVelocityX;

    //
    float m_fSmoothVelocityY;

    //
    FocusArea m_faFocusArea;

    //
    bool m_bLookAheadStopped;
    
    void Start()
    {
        //Generate the bounding box
        m_faFocusArea = new FocusArea(m_tfTarget.GetComponent<Collider2D>().bounds, m_v2FocusAreaSize);
    }
    void Update()
    {
        playerInput.x = XCI.GetAxis(XboxAxis.LeftStickX);
        playerInput.y = XCI.GetAxis(XboxAxis.LeftStickY);
    }
    
    void LateUpdate()
    {
        //Call the FocusArea Update method
        m_faFocusArea.Update(m_tfTarget.GetComponent<Collider2D>().bounds);

        //The position of the bounding box around the player
        Vector2 focusPosition = m_faFocusArea.m_v2Centre + Vector2.up * m_fVerticalOffset;

        if (m_faFocusArea.m_v2Velocity.x != 0)
        {
            m_fLookAheadDirectionX = Mathf.Sign(m_faFocusArea.m_v2Velocity.x);
            if (Mathf.Sign(playerInput.x) == Mathf.Sign(m_faFocusArea.m_v2Velocity.x) && playerInput.x != 0)
            {
                m_bLookAheadStopped = false;
                m_fTargetLookAheadX = m_fLookAheadDirectionX * m_fLookAheadDistanceX;
            }
            else
            {
                if (!m_bLookAheadStopped)
                {
                    m_bLookAheadStopped = true;
                    m_fTargetLookAheadX = m_fCurrentLookAheadX + (m_fLookAheadDirectionX * m_fLookAheadDistanceX - m_fCurrentLookAheadX) / 4;
                }
            }
        }
        
        m_fCurrentLookAheadX = Mathf.SmoothDamp(m_fCurrentLookAheadX, m_fTargetLookAheadX, ref m_fSmoothLookVelocityX, m_fLookSmoothTimeX);

        focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref m_fSmoothVelocityY, m_fVerticalSmoothTime);

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

    struct RaycastOrigins
    {
        public Vector2 m_v2TopLeft, m_v2TopRight;
        public Vector2 m_v2BottomLeft, m_v2BottomRight;
    }
}
