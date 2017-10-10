using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public Vector2 focusAreaSize;

    public float verticalOffset;

    FocusArea focusArea;

    void Start()
    {
        focusArea = new FocusArea(target.GetComponent<Collider2D>().bounds, focusAreaSize);
    }
    void LateUpdate()
    {
        focusArea.Update(target.GetComponent<Collider2D>().bounds);

        Vector2 focusPosition = focusArea.centre + Vector2.up * verticalOffset;

        transform.position = (Vector3) focusPosition + Vector3.forward * -10;

        if (transform.position.y < -5)
        {
            transform.position = new Vector3(transform.position.x, -5, transform.position.z);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(focusArea.centre, focusAreaSize);
    }
    struct FocusArea
    {
        public Vector2 centre;
        public Vector2 velocity;
        float left, right;
        float top, bottom;

        public FocusArea(Bounds targetsBounds, Vector2 size)
        {
            left = targetsBounds.center.x - size.x / 2;
            right = targetsBounds.center.x + size.x / 2;
            bottom = targetsBounds.min.y;
            top = targetsBounds.min.y + size.y;

            velocity = Vector2.zero;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;
            
            float shiftY = 0;
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;

            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}
