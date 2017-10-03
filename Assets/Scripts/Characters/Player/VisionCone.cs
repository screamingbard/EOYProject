using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour {

    [Range(1, 50)]
    public float m_fRadius; //Length of the vision of an enemy
    [Range(10,170)]
    public float m_fAngle; // width of the enemy vision

    public LayerMask m_lmPlayerMask; //Layer mask specific to the player
    public LayerMask m_lmObstacleMask; //layer mask indicating obsticles to block the vision of an enemy

    public float meshResolution;

    public Transform m_tfPlayer;

    [HideInInspector]
    public bool m_bPlayerVisible;

    public MeshFilter viewMeshFilter;

    Mesh viewMesh;

    void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    void Update(){
        FindPlayer();
        DrawVisionCone();
    }

    //Function to find the player within the vision of an enemy
    void FindPlayer()
    {
        //collider to check if the player is within the radius of an enemies vision
        Collider2D m_cPlayerWithinView = Physics2D.OverlapCircle(transform.position, m_fRadius, m_lmPlayerMask);

        //get the players tranform
        if (m_cPlayerWithinView != null)
            m_tfPlayer = m_cPlayerWithinView.transform;
        else
            Debug.Log("Null");

         //get the direction of the vector between the player and enemy
         Vector3 m_v3DirectionToPlayer = (m_tfPlayer.position - transform.position).normalized;

         //check for the player being within the cone of vision rather than just the radius
         if (Vector3.Angle(transform.forward, m_v3DirectionToPlayer) < m_fAngle / 2) {

             //get the distance between the enemy and the player
             float m_fDistanceToPlayer = Vector3.Distance(transform.position, m_tfPlayer.position);

             //check for whether an obstacle is obstructing the enemies view of the player
             if (!Physics2D.Raycast(transform.position, m_v3DirectionToPlayer, m_fDistanceToPlayer, m_lmObstacleMask)) {
                 m_bPlayerVisible = true;
                Debug.Log("SEEN");
                 //kill the player, call the death and spot animation
             } else {
                 m_bPlayerVisible = false;
             }
         } else {
             m_bPlayerVisible = false;
         }
          
    }

    //draws the cone of vision in game
    void DrawVisionCone()
    {
        int m_iStepCount = Mathf.RoundToInt(m_fAngle * meshResolution);
        float m_fStepAngleSize = m_fAngle / m_iStepCount;
        List<Vector3> m_lv3ViewPoints = new List<Vector3>();
        for (int i = 0; i <= m_iStepCount; i++)
        {
            float m_fThisAngle = transform.eulerAngles.z + m_fAngle / 2 + m_fStepAngleSize * i;
            Debug.DrawLine(transform.position, transform.position + m_v3LookTarget(m_fThisAngle, true) * m_fRadius, Color.red);
            ViewCastInfo newViewCast = ViewCast(m_fThisAngle);
            m_lv3ViewPoints.Add(newViewCast.point);
        }

        int m_iVertexCount = m_lv3ViewPoints.Count + 1;
        Vector3[] m_v3Vertices = new Vector3[m_iVertexCount];
        int[] m_iTriangles = new int[(m_iVertexCount - 2) * 3];

        m_v3Vertices[0] = Vector3.zero;
        for (int i = 0; i < m_iVertexCount - 1; i++)
        {
            m_v3Vertices[i + 1] = m_lv3ViewPoints[i];

            if (i < m_iVertexCount - 2)
            {

                m_iTriangles[i * 3] = 0;
                m_iTriangles[i * 3 + 1] = i + 1;
                m_iTriangles[i * 3 + 2] = i + 2;
            }
        }
        viewMesh.Clear();
        viewMesh.vertices = m_v3Vertices;
        viewMesh.triangles = m_iTriangles;
        viewMesh.RecalculateNormals();
    }
    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = m_v3LookTarget(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast (transform.position, dir, out hit, m_fRadius)){
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        } else {
            return new ViewCastInfo(false, transform.position + dir * m_fRadius, m_fRadius, globalAngle);
        }
    }
    //Moves the cone around the circle when the object is rotated
    public Vector3 m_v3LookTarget(float a_fAngleInDegrees, bool a_bAngleIsGlobal){
        if (!a_bAngleIsGlobal)
        {
            a_fAngleInDegrees -= transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Sin(a_fAngleInDegrees * Mathf.Deg2Rad), Mathf.Cos(a_fAngleInDegrees * Mathf.Deg2Rad), 0);
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;
        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }
    
}
