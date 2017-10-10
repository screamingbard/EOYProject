using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VisionCone))]
public class EditorVisualisations : Editor{


    // in editor GUI for level design assistance
    void OnSceneGUI(){
        VisionCone m_vcVisualiser = (VisionCone)target;

        //the colour of the in editor circle around the enemy with a vision cone
        Handles.color = Color.yellow;
        //draw a circle around the enemy indicating the radius of detection
        Handles.DrawWireArc(m_vcVisualiser.transform.position, Vector3.forward, Vector3.up, 360, m_vcVisualiser.m_fRadius);

        //Draws a cone withing the circle indication the area within which the enemy can detect the player
        Vector3 viewAngleA = m_vcVisualiser.m_v3LookTarget(-m_vcVisualiser.m_fAngle / 2, false);
        Vector3 viewAngleB = m_vcVisualiser.m_v3LookTarget(m_vcVisualiser.m_fAngle / 2, false);
        Handles.DrawLine(m_vcVisualiser.transform.position, m_vcVisualiser.transform.position + viewAngleA * m_vcVisualiser.m_fRadius);
        Handles.DrawLine(m_vcVisualiser.transform.position, m_vcVisualiser.transform.position + viewAngleB * m_vcVisualiser.m_fRadius);
    }

}