using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// ------------- ///
//  Micheal Corben //
/// ------------- ///
public class VisionCone : MonoBehaviour {

    //Length of the vision of an enemy
    [Range(0, 50)]
    public float m_fRadius;

    //Width of the enemy vision
    [Range(0, 360)]
    public float m_fAngle = 27;
    
    //Layer mask specific to the player
    public LayerMask m_lmPlayerMask;

    //Layer mask indicating obsticles to block the vision of an enemy
    public LayerMask m_lmObstacleMask;

    //Number of rays cast in the vision cone per a thing
    [Range(0.5F, 200)]
    public float m_fMeshResolution;

    //Time the player has before they're killed within a vision cone
    [Tooltip("Time in seconds the player must be seen before they are killed")]
    public float m_fDeathTimer;

    //Time modifyer for cooldown on enemy alert
    public float m_fAlertCooldown = 2;

    //Time modifyer for cooldown on enemy alert
    public float m_fPlayerDistanceCheck;

    //The radius of the overlay vision cone
    float m_fOverlayRadius;

    //The max radius of the overlay vision cone
    float m_fMaxOverlayRadius;

    //Time the player has before they're killed within a vision cone
    float m_fUseDeathTimer;

    //The transform of the player
    Transform m_tfPlayer;

    //A mesh filter variable used for the vision cone ingame visualisation
    public MeshFilter m_mfViewMeshFilter;

    //A mesh variable used for the vision cone ingame visualisation
    [HideInInspector]
    public Mesh m_mViewMesh;

    //A mesh filter variable used for the vision cone ingame visualisation
    public MeshFilter m_mfOverlayViewMeshFilter;

    //A mesh variable used for the vision cone ingame visualisation
    [HideInInspector]
    public Mesh m_mOverlayViewMesh;

    //The alert object that appears when the death timer starts counting
    public GameObject m_goAlertSignal;

    //The animation that will play once the player has been seen;
    public Animation m_aAlertAnimation;

    //The sound that will play when the player is seen
    public AudioClip m_acAlertSound;
    
    //Sound pleyererer
    AudioSource m_asAudioSource;

    //A boolean to restrict the animation to play only when the player is initially spotted
    bool m_bCanPlayAnimation;

    //A boolean to store whether or not the player is within a vision cone
    bool m_bPlayerIsBeingSeen;

    //A boolean to store if the alert sound has played to stop it being played more than once per time the player enters a vision cone
    bool m_bAlertHasPlayedSound;

    //A reference to the script required to kill the player
    public DiveKillPlayer dkPlayer;

    void Start()
    {
        //Get the players transforms
        m_tfPlayer = GameObject.FindWithTag("Player").transform;

        //Set the death timer of the vision cone
        m_fUseDeathTimer = 0;

        //Initialise the view mesh
        m_mViewMesh = new Mesh();

        //Name the view mesh, view mesh
        m_mViewMesh.name = "View Mesh";
        
        //Set the mesh of the mesh filter to the view mesh
        m_mfViewMeshFilter.mesh = m_mViewMesh;

        //Initialise the view mesh
        m_mOverlayViewMesh = new Mesh();

        //Name the view mesh, view mesh
        m_mOverlayViewMesh.name = "Overlay View Mesh";

        //Set the mesh of the mesh filter to the view mesh
        m_mfOverlayViewMeshFilter.mesh = m_mOverlayViewMesh;
    }

    void Update()
    {
        if (m_goAlertSignal != null)
        {
            //Show the alert icon if the vision cone overlay exists
            if (m_fUseDeathTimer > 0)
            {
                m_goAlertSignal.SetActive(true);
                if (m_aAlertAnimation != null)
                {
                    if (m_bCanPlayAnimation)
                    {
                        //Play the Alert animation
                        m_aAlertAnimation.Play();
                        m_bCanPlayAnimation = false;
                    }
                }
                if (m_acAlertSound != null)
                {
                    //If the player is within a vision cone
                    if (m_bPlayerIsBeingSeen)
                    {
                        //If the alert sounds has not yet played 
                        if (!m_bAlertHasPlayedSound)
                        {
                            //Play the alert sound when the player is finaly seen
                            if (PlayerPrefs.GetInt("SFX") == 1)
                            {
                                PlayClipAt(m_acAlertSound, Camera.main.transform.position);
                            }
                            else
                            {
                                //If the SFX setting is turned off set the volume to zero
                                PlayClipAt(m_acAlertSound, Camera.main.transform.position).volume = 0.0f;
                            }
                            //Remember that the alert sound has played
                            m_bAlertHasPlayedSound = true;
                        }
                    }
                    else
                    {
                        //Once the player leaves the vision cone say that the alert sound hasn't played so it will play when the player enters the vision cone again
                        m_bAlertHasPlayedSound = false;
                    }
                }
            }
            else
            {
                //If the corvus is no longer on alert, the death counter is zero, set the alert signal to not active and the can play animation bool to true
                m_goAlertSignal.SetActive(false);
                m_bCanPlayAnimation = true;
            }
        }
    }

    void LateUpdate()
    {
        //Check if the player is within a certain radius of the vision cone before drawing and calculating
        if (Vector2.Distance(m_tfPlayer.position, transform.position) > m_fPlayerDistanceCheck)
            return;
        //Call the find player method
        FindPlayer();  
       
        //Call the draw vision cone method
        DrawVisionCone();

        //Thing the thing
        m_fOverlayRadius = m_fMaxOverlayRadius * m_fUseDeathTimer / m_fDeathTimer;

        //Call the draw overlay method
        DrawOverlayVisionCone();
    }

    AudioSource PlayClipAt(AudioClip a_acClip, Vector2 a_v2Position)
        //Custon play clip at point method that will play a sound for as long as the file is to allow for alert sounds when the player moves through a vision cone
    {
        //Create a temperary game object which will hold the sound and play it
        GameObject m_goTempGameObject = new GameObject("TempAudio");
        //Set the position of the game object
        m_goTempGameObject.transform.position = a_v2Position;
        //Add an audio source to the temperary game object
        AudioSource m_audioSource = m_goTempGameObject.AddComponent<AudioSource>();
        //Set the audio clip to be played
        m_audioSource.clip = a_acClip;
        //Play the stored audio clip
        m_audioSource.Play();
        //Destroy the temp game object after the tim of the audio clip
        Destroy(m_goTempGameObject, a_acClip.length);
        //return an audio scource
        return m_audioSource;
    }
    void FindPlayer()
    //Method to find the player within the vision of an enemy
    {
        //Collider to check if the player is within the radius of an enemies vision
        bool m_bPlayerWithinView = Physics2D.OverlapCircle(transform.position, m_fRadius, m_lmPlayerMask);

        //Get the players tranform if the player is within the view radius of an enemy
        if (m_bPlayerWithinView == true)
        {
            //Get the direction of the vector between the player and enemy
            Vector3 m_v3DirectionToPlayer = (m_tfPlayer.position - transform.position).normalized;

            //Get the angle bewtween the forward of the vision cone and the vector to the player
            float m_fAngleBetween = (Vector3.Angle(m_v3DirectionToPlayer, transform.up));

            //Check for the player being within the cone of vision rather than just the radius
            if (m_fAngleBetween <= (m_fAngle / 2))
            {
                //Get the distance between the enemy and the player
                float m_fDistanceToPlayer = Vector3.Distance(transform.position, m_tfPlayer.position);
                
               //Check for whether an obstacle is obstructing the enemies view of the player
               if (!Physics2D.Raycast(transform.position, m_v3DirectionToPlayer, m_fDistanceToPlayer, m_lmObstacleMask))
               {
                   if (m_fUseDeathTimer >= m_fDeathTimer)
                   {
                        //If the enemy has direct line of sight to the player kill the player
                        dkPlayer.CreateBird();
                        //Reset the death timer
                        m_fUseDeathTimer = 0;
                   }
                   else
                    {
                        //Count up the death timer
                        m_fUseDeathTimer += Time.deltaTime;
                        m_bPlayerIsBeingSeen = true;
                   }
                }
                else
                {
                    //Reset the death timer
                    if (m_fUseDeathTimer <= 0)
                        m_fUseDeathTimer = 0;
                    else
                        m_fUseDeathTimer -= Time.deltaTime * m_fAlertCooldown;
                    m_bPlayerIsBeingSeen = false;
                }
            }
            else
            {
                //Reset the death timer
                if (m_fUseDeathTimer <= 0)
                    m_fUseDeathTimer = 0;
                else
                    m_fUseDeathTimer -= Time.deltaTime * m_fAlertCooldown;
                m_bPlayerIsBeingSeen = false;
            }
        }
        else
        {
            //Reset the death timer
            if (m_fUseDeathTimer <= 0)
                m_fUseDeathTimer = 0;
            else
                m_fUseDeathTimer -= Time.deltaTime * m_fAlertCooldown;
            m_bPlayerIsBeingSeen = false;
        }
    }

    void DrawVisionCone()
    //Draws the cone of vision in game
    {
        //Number of Raycast2Ds based on the angle of the vision cone and the mesh resolution
        int m_iStepCount = Mathf.RoundToInt(m_fAngle * m_fMeshResolution); 
        //Angle inbetween each Raycast2D
        float m_fStepAngleSize = m_fAngle / m_iStepCount;
        //List of vectors to direct each Raycast2D
        List<Vector3> m_lv3ViewPoints = new List<Vector3>();
        //Looping through the number of steps or Raycast2Ds
        for (int i = 0; i <= m_iStepCount; i++){
            //Angle used within this loop
            float m_fThisAngle = (transform.eulerAngles.z * -1) - m_fAngle/2 + m_fStepAngleSize * i;
            //Use the ViewCast method to return a ViewCastInfo struct which contains information about an individual Raycast2D
            ViewCastInfo newViewCast = ViewCast(m_fThisAngle);
            //Add a raycast to the list
            m_lv3ViewPoints.Add(newViewCast.m_v3Point);
        }
        //The number of vertecies
        int m_iVertexCount = m_lv3ViewPoints.Count + 1;

        //An array of Verticies
        Vector3[] m_v3Vertices = new Vector3[m_iVertexCount];

        //An array of triangles
        int[] m_iTriangles = new int[(m_iVertexCount - 2) * 3];

        //Set the zeroeth vertex in the array to (0, 0, 0)
        m_v3Vertices[0] = Vector3.zero;
        for (int i = 0; i < m_iVertexCount - 1; i++)
        //Go through the verticies array and set them
        {
            m_v3Vertices[i + 1] = transform.InverseTransformPoint(m_lv3ViewPoints[i]);
            
            //If the iteration count is less than the vertex count minus two
            if (i < m_iVertexCount - 2)
            {
                //Set the triangles to their relavent numbers
                m_iTriangles[i * 3] = 0;
                m_iTriangles[i * 3 + 1] = i + 1;
                m_iTriangles[i * 3 + 2] = i + 2;
            }
        }

        //Clear the view mesh
        m_mViewMesh.Clear();

        //Set the vertecies of the view mesh
        m_mViewMesh.vertices = m_v3Vertices;

        //Set the triangles of the view mesh
        m_mViewMesh.triangles = m_iTriangles;

        //Recalculate the normals of the view mesh
        m_mViewMesh.RecalculateNormals();

        //Set the max overlay radius to zero so the for loop can find the longest
        m_fMaxOverlayRadius = 0;

        //Set the max overlay radius to conform within the mesh
        for (int i = 0; i < m_v3Vertices.Length; i++)
        {
            if (m_fMaxOverlayRadius < m_v3Vertices[i].sqrMagnitude)
            {
                m_fMaxOverlayRadius = m_v3Vertices[i].sqrMagnitude;
            }
        }
        m_fMaxOverlayRadius = Mathf.Sqrt(m_fMaxOverlayRadius);
    }

    ViewCastInfo ViewCast(float a_fGlobalAngle)
    {
        //The look direction of the vision cone relative to global
        Vector3 m_v3Direction = m_v3LookTarget(a_fGlobalAngle, true);
        //A test for if the rays hit something
        RaycastHit2D m_v3Hit = Physics2D.Raycast(transform.position, m_v3Direction, m_fRadius, m_lmObstacleMask );
        //If the rays did hit
        if (m_v3Hit)
        {
            //Returns a new view cast info struct with relavent information
            return new ViewCastInfo(true, m_v3Hit.point, m_v3Hit.distance, a_fGlobalAngle);
        }
        //Otherwise
        else
        {
            //Returns a new view cast info struct with different relavent information
            return new ViewCastInfo(false, transform.position + m_v3Direction * m_fRadius, m_fRadius, a_fGlobalAngle);
        }
    }

    public Vector3 m_v3LookTarget(float a_fAngleInDegrees, bool a_bAngleIsGlobal){
        //Moves the cone around the circle when the object is rotated
        if (!a_bAngleIsGlobal)
        {
            //Invert the angle so that it follows the correctorientation
            a_fAngleInDegrees -= transform.eulerAngles.z;
        }

        //Return the angle in degrees the diference is between the look target and the forward
        return new Vector3(Mathf.Sin(a_fAngleInDegrees * Mathf.Deg2Rad), Mathf.Cos(a_fAngleInDegrees * Mathf.Deg2Rad), 0);
    }

    public struct ViewCastInfo
    {
        //Boolean variable to check if the raycast has hit anything
        public bool m_bHit;

        //Vector 3 variable to get the
        public Vector3 m_v3Point;

        //Float variable to get the distance the ray cast goes before it hits someting, will be max range if it doesn't hit
        public float m_fDistance;

        //Float variable to get the angle between the forward and the target
        public float m_fVCIAngle;

        
        public ViewCastInfo(bool a_bHit, Vector3 a_v3Point, float a_fDistance, float a_fAngle)
       //Sets all the variables within the view cast info struct
        {
            //Sets the hit boolean
            m_bHit = a_bHit;

            //Sets the point variable
            m_v3Point = a_v3Point;

            //Sets the distance variable
            m_fDistance = a_fDistance;

            //Sets the angle variable
            m_fVCIAngle = a_fAngle;
        }
    }

    void DrawOverlayVisionCone()
    //Draws the cone of vision detection Overlay in game
    {
        //Number of Raycast2Ds based on the angle of the vision cone and the mesh resolution
        int m_iStepCount = Mathf.RoundToInt(m_fAngle * m_fMeshResolution);
        //Angle inbetween each Raycast2D
        float m_fStepAngleSize = m_fAngle / m_iStepCount;
        //List of vectors to direct each Raycast2D
        List<Vector3> m_lv3ViewPoints = new List<Vector3>();
        //Looping through the number of steps or Raycast2Ds
        for (int i = 0; i <= m_iStepCount; i++)
        {
            //Angle used within this loop
            float m_fThisAngle = (transform.eulerAngles.z * -1) - m_fAngle / 2 + m_fStepAngleSize * i;
            //Use the ViewCast method to return a ViewCastInfo struct which contains information about an individual Raycast2D
            ViewCastInfo newViewCast = OverlayViewCast(m_fThisAngle);
            //Add a raycast to the list
            m_lv3ViewPoints.Add(newViewCast.m_v3Point);
        }
        //The number of vertecies
        int m_iVertexCount = m_lv3ViewPoints.Count + 1;

        //An array of Verticies
        Vector3[] m_v3Vertices = new Vector3[m_iVertexCount];

        //An array of triangles
        int[] m_iTriangles = new int[(m_iVertexCount - 2) * 3];

        //Set the zeroeth vertex in the array to (0, 0, 0)
        m_v3Vertices[0] = Vector3.zero;
        for (int i = 0; i < m_iVertexCount - 1; i++)
        //Go through the verticies array and set them
        {
            m_v3Vertices[i + 1] = transform.InverseTransformPoint(m_lv3ViewPoints[i]);

            //If the iteration count is less than the vertex count minus two
            if (i < m_iVertexCount - 2)
            {
                //Set the triangles to their relavent numbers
                m_iTriangles[i * 3] = 0;
                m_iTriangles[i * 3 + 1] = i + 1;
                m_iTriangles[i * 3 + 2] = i + 2;
            }
        }

        //Clear the Overlay view mesh
        m_mOverlayViewMesh.Clear();

        //Set the vertecies of the Overlay view mesh
        m_mOverlayViewMesh.vertices = m_v3Vertices;

        //Set the triangles of the Overlay view mesh
        m_mOverlayViewMesh.triangles = m_iTriangles;

        //Recalculate the normals of the Overlay view mesh
        m_mOverlayViewMesh.RecalculateNormals();
    }

    ViewCastInfo OverlayViewCast(float a_fGlobalAngle)
    {
        //The look direction of the vision cone relative to global
        Vector3 m_v3Direction = m_v3LookTarget(a_fGlobalAngle, true);
        //A test for if the rays hit something
        RaycastHit2D m_v3Hit = Physics2D.Raycast(transform.position, m_v3Direction, m_fOverlayRadius, m_lmObstacleMask);
        //If the rays did hit
        if (m_v3Hit)
        {
            //Returns a new view cast info struct with relavent information
            return new ViewCastInfo(true, m_v3Hit.point, m_v3Hit.distance, a_fGlobalAngle);
        }
        //Otherwise
        else
        {
            //Returns a new view cast info struct with different relavent information
            return new ViewCastInfo(false, transform.position + m_v3Direction * m_fOverlayRadius, m_fOverlayRadius, a_fGlobalAngle);
        }
    }
}
