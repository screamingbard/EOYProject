using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrappleSystem : MonoBehaviour {
    public LineRenderer grappleRenderer;
    public LayerMask grappleLayerMask;
    public float climbSpeed = 3f;
    public GameObject grappleHingeAnchor;
    public DistanceJoint2D grappleJoint;
    public Transform crosshair;
	public float timeBetweenGrapples;
	public float pullFromGroundTime;

	public float grappleMaxCastDistance = 50f;

	private Player player;

	private bool grappleAttached;
    private Vector2 playerPosition;
    private List<Vector2> grapplePositions = new List<Vector2>();
    private Rigidbody2D grappleHingeAnchorRb;
    private bool distanceSet;
    private bool isColliding;
    private Dictionary<Vector2, int> wrapPointsLookup = new Dictionary<Vector2, int>();
    private SpriteRenderer grappleHingeAnchorSprite;

	private float timeSinceLastGrapple;
	private bool canGrapple;
	private bool pullingFromGround;

	void Awake (){
		player = GetComponent<Player>();

		grappleJoint.enabled = false;
	    playerPosition = transform.position;
        grappleHingeAnchorRb = grappleHingeAnchor.GetComponent<Rigidbody2D>();
        grappleHingeAnchorSprite = grappleHingeAnchor.GetComponent<SpriteRenderer>();

		canGrapple = true;
	}

    /// <summary>
    /// Figures out the closest Polygon collider vertex to a specified Raycast2D hit point in order to assist in 'rope wrapping'
    /// </summary>
    /// <param name="hit">The raycast2d hit</param>
    /// <param name="polyCollider">the reference polygon collider 2D</param>
    /// <returns></returns>
    private Vector2 GetClosestColliderPointFromRaycastHit(RaycastHit2D hit, PolygonCollider2D polyCollider) {
        // Transform polygoncolliderpoints to world space (default is local)
        var distanceDictionary = polyCollider.points.ToDictionary<Vector2, float, Vector2>(
            position => Vector2.Distance(hit.point, polyCollider.transform.TransformPoint(position)), 
            position => polyCollider.transform.TransformPoint(position));

        var orderedDictionary = distanceDictionary.OrderBy(e => e.Key);
        return orderedDictionary.Any() ? orderedDictionary.First().Value : Vector2.zero;
    }

    void Update () {
        playerPosition = transform.position;

		// If we have grappled recently count up until timebetweenGrapples has occured
		// then make it so the player can grapple again
		if(canGrapple == false) {
			timeSinceLastGrapple += Time.deltaTime;

			if(timeSinceLastGrapple >= timeBetweenGrapples) {
				timeSinceLastGrapple = 0f;
				canGrapple = true;
			}
		}

        if (!grappleAttached) {
			if(player.IsGrappling)
				player.SetGrappling(false);
	    }
	    else {
            player.SetGrappling(true);
            player.SetRopeHook(grapplePositions.Last());

            // Wrap rope around points of colliders if there are raycast collisions between player position and their closest current wrap around collider / angle point.
	        if (grapplePositions.Count > 0) {
	            var lastRopePoint = grapplePositions.Last();
                var playerToCurrentNextHit = Physics2D.Raycast(playerPosition, (lastRopePoint - playerPosition).normalized, Vector2.Distance(playerPosition, lastRopePoint) - 0.1f, grappleLayerMask);
                if (playerToCurrentNextHit) {
                    var colliderWithVertices = playerToCurrentNextHit.collider as PolygonCollider2D;
                    if (colliderWithVertices != null) {
                        var closestPointToHit = GetClosestColliderPointFromRaycastHit(playerToCurrentNextHit, colliderWithVertices);
                        if (wrapPointsLookup.ContainsKey(closestPointToHit)) {
                            // Reset the rope if it wraps around an 'already wrapped' position.
                            ResetRope();
                            return;
                        }

                        grapplePositions.Add(closestPointToHit);
                        wrapPointsLookup.Add(closestPointToHit, 0);
                        distanceSet = false;
                    }
                }
            }
        }

	    UpdateRopePositions();
        HandleRopeUnwrap();
    }

    /// <summary>
    /// Handles input within the RopeSystem component
    /// </summary>
    /// <param name="aimDirection">The current direction for aiming based on mouse position</param>
    public void OnGrappleInput(Vector2 aimDirection) {
		if (grappleAttached || canGrapple == false)
			return;

		grappleRenderer.enabled = true;

		var hit = Physics2D.Raycast(playerPosition, aimDirection, grappleMaxCastDistance, grappleLayerMask);
		if (hit.collider != null) {
			grappleAttached = true;
			if (!grapplePositions.Contains(hit.point)) {
				// Jump slightly to distance the player a little from the ground after grappling to something.
				transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
				grapplePositions.Add(hit.point);
				wrapPointsLookup.Add(hit.point, 0);
				grappleJoint.distance = Vector2.Distance(playerPosition, hit.point);
				grappleJoint.enabled = true;
				grappleHingeAnchorSprite.enabled = true;
			}
		} else {
			grappleRenderer.enabled = false;
			grappleAttached = false;
			grappleJoint.enabled = false;
		}
	}

	public void OnGrappleInputUp() {
		ResetRope();
		canGrapple = false;
	}

	/// <summary>
	/// Resets the rope in terms of gameplay, visual, and supporting variable values.
	/// </summary>
	private void ResetRope() {
        grappleJoint.enabled = false;
        grappleAttached = false;
        player.SetGrappling(false);
        grappleRenderer.positionCount = 2;
        grappleRenderer.SetPosition(0, transform.position);
        grappleRenderer.SetPosition(1, transform.position);
        grapplePositions.Clear();
        wrapPointsLookup.Clear();
        grappleHingeAnchorSprite.enabled = false;
    }

    /// <summary>
    /// Retracts or extends the 'rope'
    /// </summary>
    public void AdjustRopeLength(float direction) {
		StopCoroutine(RemoveGrappleLength(0));
		if(direction > 0f && grappleAttached && isColliding == false) {
			grappleJoint.distance -= Time.deltaTime * climbSpeed;
		} 
		else if(direction < 0f && grappleAttached) {
			grappleJoint.distance += Time.deltaTime * climbSpeed;
		}
    }

	public void RemoveRopeLength(float amount) {
		if (pullingFromGround)
			return;

		StartCoroutine(RemoveGrappleLength(amount));
	}

    /// <summary>
    /// Handles updating of the rope hinge and anchor points based on objects the rope can wrap around. These must be PolygonCollider2D physics objects.
    /// </summary>
    private void UpdateRopePositions() {
        if (grappleAttached) {
            grappleRenderer.positionCount = grapplePositions.Count + 1;

            for (var i = grappleRenderer.positionCount - 1; i >= 0; i--) {
                if (i != grappleRenderer.positionCount - 1) { // if not the Last point of line renderer
                    grappleRenderer.SetPosition(i, grapplePositions[i]);
                    
                    // Set the rope anchor to the 2nd to last rope position (where the current hinge/anchor should be) or if only 1 rope position then set that one to anchor point
                    if (i == grapplePositions.Count - 1 || grapplePositions.Count == 1) {
                        if (grapplePositions.Count == 1) {
                            var ropePosition = grapplePositions[grapplePositions.Count - 1];
                            grappleHingeAnchorRb.transform.position = ropePosition;
                            if (!distanceSet) {
                                grappleJoint.distance = Vector2.Distance(transform.position, ropePosition);
                                distanceSet = true;
                            }
                        }
                        else {
                            var ropePosition = grapplePositions[grapplePositions.Count - 1];
                            grappleHingeAnchorRb.transform.position = ropePosition;
                            if (!distanceSet) {
                                grappleJoint.distance = Vector2.Distance(transform.position, ropePosition);
                                distanceSet = true;
                            }
                        }
                    }
                    else if (i - 1 == grapplePositions.IndexOf(grapplePositions.Last())) {
                        // if the line renderer position we're on is meant for the current anchor/hinge point...
                        var ropePosition = grapplePositions.Last();
                        grappleHingeAnchorRb.transform.position = ropePosition;
                        if (!distanceSet) {
                            grappleJoint.distance = Vector2.Distance(transform.position, ropePosition);
                            distanceSet = true;
                        }
                    }
                }
                else {
                    // Player position
                    grappleRenderer.SetPosition(i, transform.position);
                }
            }
        }
    }

    private void HandleRopeUnwrap() {
        if (grapplePositions.Count <= 1)
            return;

        // Hinge = next point up from the player position
        // Anchor = next point up from the Hinge
        // Hinge Angle = Angle between anchor and hinge
        // Player Angle = Angle between anchor and player

        // 1
        var anchorIndex = grapplePositions.Count - 2;
        // 2
        var hingeIndex = grapplePositions.Count - 1;
        // 3
        var anchorPosition = grapplePositions[anchorIndex];
        // 4
        var hingePosition = grapplePositions[hingeIndex];
        // 5
        var hingeDir = hingePosition - anchorPosition;
        // 6
        var hingeAngle = Vector2.Angle(anchorPosition, hingeDir);
        // 7
        var playerDir = playerPosition - anchorPosition;
        // 8
        var playerAngle = Vector2.Angle(anchorPosition, playerDir);

        if (!wrapPointsLookup.ContainsKey(hingePosition)) {
            Debug.LogError("We were not tracking hingePosition (" + hingePosition + ") in the look up dictionary.");
            return;
        }

        if (playerAngle < hingeAngle) {
            // 1
            if (wrapPointsLookup[hingePosition] == 1) {
                UnwrapRopePosition(anchorIndex, hingeIndex);
                return;
            }

            // 2
            wrapPointsLookup[hingePosition] = -1;
        }
        else {
            // 3
            if (wrapPointsLookup[hingePosition] == -1) {
                UnwrapRopePosition(anchorIndex, hingeIndex);
                return;
            }

            // 4
            wrapPointsLookup[hingePosition] = 1;
        }
    }

    private void UnwrapRopePosition(int anchorIndex, int hingeIndex) {
        // 1
        var newAnchorPosition = grapplePositions[anchorIndex];
        wrapPointsLookup.Remove(grapplePositions[hingeIndex]);
        grapplePositions.RemoveAt(hingeIndex);

        // 2
        grappleHingeAnchorRb.transform.position = newAnchorPosition;
        distanceSet = false;

        // Set new rope distance joint distance for anchor position if not yet set.
        if (distanceSet) {
            return;
        }
        grappleJoint.distance = Vector2.Distance(transform.position, newAnchorPosition);
        distanceSet = true;
    }

    void OnTriggerStay2D(Collider2D colliderStay) {
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D colliderOnExit) {
        isColliding = false;
    }

	private IEnumerator RemoveGrappleLength(float amount) {
		pullingFromGround = true;
		float currentLength = grappleJoint.distance;
		float desiredLength = currentLength - amount;

		float t = 0f;

		while(t < pullFromGroundTime) {
			grappleJoint.distance = Mathf.Lerp(currentLength, desiredLength, t / pullFromGroundTime);
			t += Time.deltaTime;
			yield return null;
		}

		grappleJoint.distance = desiredLength;
		pullingFromGround = false;
	}
}
