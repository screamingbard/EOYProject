using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerCrosshair : MonoBehaviour {
	public Transform crosshair;
	public float distanceFromPlayer;

	private SpriteRenderer crosshairSprite;

	void Start () {
		crosshairSprite = GetComponent<SpriteRenderer>();
	}

	/// <summary>
	/// Move the aiming crosshair based on aim angle
	/// </summary>
	/// <param name="aimAngle">The mouse aiming angle</param>
	public void SetCrosshair(float aimAngle) {
		if (crosshairSprite == null)
			return;

		if (!crosshairSprite.enabled) {
			crosshairSprite.enabled = true;
		}

		var x = transform.position.x + distanceFromPlayer * Mathf.Cos(aimAngle);
		var y = transform.position.y + distanceFromPlayer * Mathf.Sin(aimAngle);

		Vector3 crossHairPosition = new Vector3(x, y, 0);
		crosshair.transform.position = crossHairPosition;
		RotateCrosshair();
	}

	private void RotateCrosshair() {
		Vector2 direction = crosshair.position - transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
		crosshair.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	private void OnValidate() {
		if(distanceFromPlayer < 1f) {
			distanceFromPlayer = 1f;
		}
	}
}
