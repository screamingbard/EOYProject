using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour {

    //Length of the vision of an enemy
    [Range(0, 50)]     public float m_fRadius;

    //width of the enemy vision
    [Range(0, 360)]    public float m_fAngle = 27;

    //layer mask specific to the player
    public LayerMask m_lmPlayerMask;

    //layer mask indicating obsticles to block the vision of an enemy
    public LayerMask m_lmObstacleMask;

    //Number of rays cast in the vision cone per a thing
    [Range(0.5F, 200)]
    public float m_fMeshResolution;

    //Time the player has before they're killed within a vision cone
    [Tooltip("Time in seconds the player must be seen before they are killed")]
    public float m_fDeathTimer;

    //Time the player has before they're killed within a vision cone
    float m_fUseDeathTimer;

    //the transform of the player
    public Transform m_tfPlayer;
    
    //
    public MeshFilter m_mfViewMeshFilter;

    //
    [HideInInspector]
    public Mesh m_mViewMesh;                   

    void Start()
    {
        //Set the death timer of the vision cone
        m_fUseDeathTimer = m_fDeathTimer;
        //
        m_mViewMesh = new Mesh();

        //
        m_mViewMesh.name = "View Mesh";
        
        //
        m_mfViewMeshFilter.mesh = m_mViewMesh;
    }

    void Update()
    {
        //
        FindPlayer();  
       
        //
        DrawVisionCone();
    }

    void FindPlayer()
    //Function to find the player within the vision of an enemy
    {

        //collider to check if the player is within the radius of an enemies vision
        var m_cPlayerWithinView = Physics2D.OverlapCircle(transform.position, m_fRadius, m_lmObstacleMask);

        //get the players tranform if the player is within the view radius of an enemy
        if (m_cPlayerWithinView != null)
        {
            //get the direction of the vector between the player and enemy
            Vector3 m_v3DirectionToPlayer = (m_tfPlayer.position - transform.position);

            //
            float m_fAngleBetween = (Vector3.Angle(m_v3DirectionToPlayer, transform.up));

            //check for the player being within the cone of vision rather than just the radius
            if (m_fAngleBetween <= (m_fAngle / 2))
            {
                //get the distance between the enemy and the player
                float m_fDistanceToPlayer = Vector3.Distance(transform.position, m_tfPlayer.position);

                //if the player is within the radius of the vision cone
                if (m_fDistanceToPlayer < m_fRadius)
                {
                    //check for whether an obstacle is obstructing the enemies view of the player
                    if (Physics2D.Raycast(transform.position, m_v3DirectionToPlayer, m_fDistanceToPlayer, m_lmPlayerMask))
                    {
                        if (m_fUseDeathTimer <= 0)
                        {
                            //if the enemy has direct line of sight to the player kill the player
                            m_tfPlayer.GetComponent<PlayerRespawn>().Respawn();
                            m_fUseDeathTimer = m_fDeathTimer;
                        }
                        else
                        {
                            m_fUseDeathTimer -= Time.deltaTime;
                        }
                    }
                    else
                    {
                        m_fUseDeathTimer = m_fDeathTimer;
                    }
                }
                else
                {
                    m_fUseDeathTimer = m_fDeathTimer;
                }
            }
            else
            {
                m_fUseDeathTimer = m_fDeathTimer;
            }
        }
        else
        {
            m_fUseDeathTimer = m_fDeathTimer;
        }
    }

    void DrawVisionCone()
    //Draws the cone of vision in game
    {
        //number of Raycast2Ds based on the angle of the vision cone and the mesh resolution
        int m_iStepCount = Mathf.RoundToInt(m_fAngle * m_fMeshResolution); 
        //angle inbetween each Raycast2D
        float m_fStepAngleSize = m_fAngle / m_iStepCount;
        //list of vectors to direct each Raycast2D
        List<Vector3> m_lv3ViewPoints = new List<Vector3>();
        //looping through the number of steps or Raycast2Ds
        for (int i = 0; i <= m_iStepCount; i++){
            //angle used within this loop
            float m_fThisAngle = (transform.eulerAngles.z * -1) - m_fAngle/2 + m_fStepAngleSize * i;
            Debug.DrawLine(transform.position, transform.position + m_v3LookTarget(m_fThisAngle, true) * m_fRadius, Color.red);
            //use the ViewCast function to return a ViewCastInfo struct which contains information about an individual Raycast2D
            ViewCastInfo newViewCast = ViewCast(m_fThisAngle);
            m_lv3ViewPoints.Add(newViewCast.point);
        }
        int m_iVertexCount = m_lv3ViewPoints.Count + 1;
        Vector3[] m_v3Vertices = new Vector3[m_iVertexCount];
        int[] m_iTriangles = new int[(m_iVertexCount - 2) * 3];

        m_v3Vertices[0] = Vector3.zero;
        for (int i = 0; i < m_iVertexCount - 1; i++)
        {
            m_v3Vertices[i + 1] = transform.InverseTransformPoint(m_lv3ViewPoints[i]);
            
            if (i < m_iVertexCount - 2)
            {
                m_iTriangles[i * 3] = 0;
                m_iTriangles[i * 3 + 1] = i + 1;
                m_iTriangles[i * 3 + 2] = i + 2;
            }
        }

        m_mViewMesh.Clear();
        m_mViewMesh.vertices = m_v3Vertices;
        m_mViewMesh.triangles = m_iTriangles;
        m_mViewMesh.RecalculateNormals();
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = m_v3LookTarget(globalAngle, true);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, m_fRadius, m_lmObstacleMask );
        if (hit){
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        } else {
            return new ViewCastInfo(false, transform.position + dir * m_fRadius, m_fRadius, globalAngle);
        }
    }
    
    public Vector3 m_v3LookTarget(float a_fAngleInDegrees, bool a_bAngleIsGlobal){
        //Moves the cone around the circle when the object is rotated
        if (!a_bAngleIsGlobal)
        {
            //
            a_fAngleInDegrees -= transform.eulerAngles.z;
        }

        //
        return new Vector3(Mathf.Sin(a_fAngleInDegrees * Mathf.Deg2Rad), Mathf.Cos(a_fAngleInDegrees * Mathf.Deg2Rad), 0);
    }

    public struct ViewCastInfo
    {
        //
        public bool hit;

        //
        public Vector3 point;

        //
        public float dst;

        //
        public float angle;

        
        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
       //
        {
            //
            hit = _hit;

            //
            point = _point;

            //
            dst = _dst;

            //
            angle = _angle;
        }
    }
    
}
