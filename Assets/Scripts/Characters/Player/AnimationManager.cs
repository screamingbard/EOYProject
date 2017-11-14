using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class AnimationManager : MonoBehaviour {

    //The animator
    public Animator m_animatorAnimator;

    //The string for use to connect and control mechanim specifically for the walking animation
    public string m_stIsWalking;

    //The string for use to connect and control mechanim specifically for the jumping animation
    public string m_stIsJumping;

    //The string for use to connect and control mechanim specifically for the swinging animation
    public string m_stIsSwinging;

    //Is the player grounded
    public bool m_bIsGrounded;

    //Check for if the grapple is out and about
    public bool m_bGrappleIsOut;

    //Reference to the player
    public GameObject m_goPlayer;

    //Reference to the player controller
    public Controller2D m_pcPlayerContorller;

    //Reference to the player's player class
    public Player m_playerPlayer;
    
    void Update () {
        if (!m_pcPlayerContorller.collisions.below)
        {
            m_animatorAnimator.SetBool(m_stIsWalking, false);
            if (/*m_playerPlayer.bGrappling*/m_bGrappleIsOut)
            {
                m_animatorAnimator.SetBool(m_stIsSwinging, true);
                m_animatorAnimator.SetBool(m_stIsJumping, false);
            }
            else
            {
                m_animatorAnimator.SetBool(m_stIsJumping, true);
                m_animatorAnimator.SetBool(m_stIsSwinging, false);
            }
        }
        else
        {
            m_animatorAnimator.SetBool(m_stIsJumping, false);
            m_animatorAnimator.SetBool(m_stIsSwinging, false);
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
