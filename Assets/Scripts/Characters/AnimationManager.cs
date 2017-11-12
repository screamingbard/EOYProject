using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class AnimationManager : MonoBehaviour {

    //
    public Animator m_animatorAnimator;

    //
    public string m_stIsWalking;

    //
    public string m_stIsJumping;

    //
    public string m_stIsSwinging;

    //
    public bool m_bIsGrounded;

    //
    public bool m_bGrappleIsOut;

    //
    public GameObject m_goPlayer;

    //
    public Controller2D m_cont2dPlayerContorller;

    void Start()
    {
    }
    // Update is called once per frame
    void Update () {
        if (!m_cont2dPlayerContorller.collisions.IsDying)
        {
        }
        else
        {
            if (XCI.GetAxisRaw(XboxAxis.LeftStickX) != 0)
            {
                m_animatorAnimator.SetBool(m_stIsWalking, true);
            }
            else
            {
                m_animatorAnimator.SetBool(m_stIsWalking, false);
            }
        }
	}
}
