using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour {

    //Vision cone tag
    public string m_stVisionConeTag;
    //Death trap tag
    public string m_stDeathTrapTag;
    //The respawning point of the player
    public Transform m_tfRespawnPoint;
    //The player
    public GameObject m_goPlayer;

    void Respawn(){
        //Play death animation

        //Set the players position to the respawn points
        m_goPlayer.transform.SetPositionAndRotation(m_tfRespawnPoint.position, m_tfRespawnPoint.rotation);

        //Play the respawn animation
    }
    
    void OnCollisionEnter2D(Collision2D a_col2DCollider){
        //If the player enters the collider of an object DeathTrap
        if (a_col2DCollider.gameObject.tag == m_stDeathTrapTag)
        {
            //Call the respawn funtion
            Respawn();
        }
        //Or if the player enters the collider of an enemy vision cone
        else if (a_col2DCollider.gameObject.tag == m_stVisionConeTag)
        {
            //Play the killed by corvus animation

            //Call the respawn function
            Respawn();
        }
    }
}
