using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour {

    //Vision cone tag
    public string m_stVisionConeTag;
    //Death trap tag
    public string m_stDeathTrapTag;
    //New respawn point tag
    public string m_stNewRespawn;
    //The respawning point of the player
    public Transform m_tfRespawnPoint;
    //The player
    public GameObject m_goPlayer;
    //The grapple
    public GameObject m_goGrappleRefrence;
    //The time before you can move
    public float m_fSetMovementTimer = 0.1f;
    //The time before you can move
    [HideInInspector]
    public float m_fMovementTimer;

    public void Respawn(){
        //Play death animation

        //Set the players velocity to zero
        m_goPlayer.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        //start the timer to disallow movement for a split second
        m_fMovementTimer = m_fSetMovementTimer;

        //If it is connected to terain or in motion
        if (m_goGrappleRefrence != null){
            //Remove the grapple
            m_goGrappleRefrence.SetActive(false);
        }

        //Set the players position to the respawn points
        m_goPlayer.transform.SetPositionAndRotation(m_tfRespawnPoint.position, m_tfRespawnPoint.rotation);

        //Play the respawn animation
    }
    
    void OnCollisionEnter2D(Collision2D a_col2DCollider){
        //If the player enters the collider of an object DeathTrap
        if (a_col2DCollider.gameObject.tag == m_stDeathTrapTag){
            //Call the respawn funtion
            Respawn();
        }
        //Or if the player enters the collider of an enemy vision cone
        else if (a_col2DCollider.gameObject.tag == m_stVisionConeTag){
            //Play the killed by corvus animation

            //Call the respawn function
            Respawn();
        }
    }
    void OnTriggerEnter2D(Collider2D a_trTrigger2D){
		
        //If the player enters the trigger zone of a new respawn point
        if (a_trTrigger2D.gameObject.tag == m_stNewRespawn)
        {
			//set the new respawn point
			m_tfRespawnPoint = a_trTrigger2D.gameObject.transform;
        }
    }
}
