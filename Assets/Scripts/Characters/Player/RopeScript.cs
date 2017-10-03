using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour {

    public Vector2 v2Point;
    public const float cfshootSpeed = 5.0f;
    public float fHookSpeed = 5.0f;

    public float fNodeDist = 3.0f;
    public GameObject goNodePrefab = null;
    public GameObject goPlayer = null;
    GameObject LastNode = null;

    public bool bConnected = false;

    public LineRenderer lr;

    int iVertCount = 2;
    public List<GameObject> lgoRopeNodes = new List<GameObject>();

    // Use this for initialization
    void Awake ()
    {
        lr = GetComponent<LineRenderer>();

        goPlayer = GameObject.FindGameObjectWithTag("Player");

        LastNode = transform.gameObject;

        lgoRopeNodes.Add(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = Vector2.MoveTowards(transform.position, v2Point, fHookSpeed);

        if( (Vector2)transform.position != v2Point)
        {
            if(Vector2.Distance(goPlayer.transform.position, LastNode.transform.position) > fNodeDist)
            {       
                CreateNode();
            }
        }
        else if (bConnected == false)
        {
            bConnected = true;

            while (Vector2.Distance(goPlayer.transform.position, LastNode.transform.position) > fNodeDist)
            {
                CreateNode();
            }

            LastNode.GetComponent<HingeJoint2D>().connectedBody = goPlayer.GetComponent<Rigidbody2D>();
        }

        RenderLine();
    }
    
    void RenderLine()
    {
        lr.positionCount = iVertCount;
        
        int i;
        for (i = 0; i < lgoRopeNodes.Count; i++)
        {
            lr.SetPosition(i, lgoRopeNodes[i].transform.position);
        }
        lr.SetPosition(i, goPlayer.transform.position);
    }

    //use a list later in order to allow backtracking
    void CreateNode()
    {
        Vector2 posCreate = goPlayer.transform.position - LastNode.transform.position;
        posCreate.Normalize();
        posCreate *= fNodeDist;
        posCreate += (Vector2)LastNode.transform.position;

        GameObject temp = Instantiate(goNodePrefab, posCreate, Quaternion.identity);

        temp.transform.SetParent(transform);

        LastNode.GetComponent<HingeJoint2D>().connectedBody = temp.GetComponent<Rigidbody2D>();

        LastNode = temp;

        lgoRopeNodes.Add(LastNode);

        iVertCount++;
    }
}
