using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAction : MonoBehaviour {
	public bool disableObjectColliderWhileHeld = true;
	public bool lerpObjectToIdentity = true;
	public Vector2 holdOffset = new Vector2 (0.4f, 0.2f);
	public KeyCode pickUpKey;

	private GameObject currentPickedUpItem;
	private bool hasPickedUpObject = false;
	private Vector2 pickedUpOffset = new Vector2(0.4f, 0.2f);
	private float dropForceScalingFactor = 8.0f;
	private float pickUpRadius = 1.3f;
	private float pickUpLerpSpeed = 0.1f;
	private float minLerpSpeed = 0.1f;
	private float maxLerpSpeed = 1.0f;
	private float lerpIncrement = 0.1f;
	private float objectGravityScale;

	private InputMove im;

	void Start() {	
		im = GetComponent<InputMove> ();
	}

	void Update () {
		if (Input.GetKeyDown (pickUpKey)) {
			if (hasPickedUpObject) {
				DropItem ();
			} else {
				PickUpItem ();
			}
		}

		if (hasPickedUpObject && currentPickedUpItem != null) {
			if (Mathf.Sign (pickedUpOffset.x) != im.GetCurrentFaceDirection ()) {
				pickUpLerpSpeed = minLerpSpeed;
				if (im.GetCurrentFaceDirection () > 0.0f) {
					if (currentPickedUpItem.GetComponent<SpriteRenderer> () != null) {
						currentPickedUpItem.GetComponent<SpriteRenderer> ().flipX = false;
					}

					pickedUpOffset = new Vector2 (holdOffset.x, holdOffset.y);
				} else {
					if (currentPickedUpItem.GetComponent<SpriteRenderer> () != null) {
						currentPickedUpItem.GetComponent<SpriteRenderer> ().flipX = true;
					}
					pickedUpOffset = new Vector2 (-holdOffset.x, holdOffset.y);
				}
			}

			pickUpLerpSpeed = Mathf.Min(pickUpLerpSpeed + lerpIncrement, maxLerpSpeed);

			Vector3 targetPosition = new Vector3(transform.position.x + pickedUpOffset.x, transform.position.y + pickedUpOffset.y, currentPickedUpItem.transform.position.z);
			Vector3 lerpedPosition = Vector3.Lerp (currentPickedUpItem.transform.position, targetPosition, pickUpLerpSpeed);
			currentPickedUpItem.transform.position = new Vector3 (lerpedPosition.x, lerpedPosition.y, currentPickedUpItem.transform.position.z);

			Quaternion lerpedAngle = Quaternion.Lerp (currentPickedUpItem.transform.rotation, Quaternion.identity, pickUpLerpSpeed);
			currentPickedUpItem.transform.rotation = lerpedAngle;
		}
	}

	void DropItem() {
		pickUpLerpSpeed = minLerpSpeed;
		hasPickedUpObject = false;
		if (currentPickedUpItem.GetComponent<SpriteRenderer> () != null) {
			currentPickedUpItem.GetComponent<SpriteRenderer> ().flipX = false;
		}		
		currentPickedUpItem.transform.position += new Vector3 (pickedUpOffset.x, pickedUpOffset.y, 0.0f);

		if (currentPickedUpItem.GetComponent<Rigidbody2D> () != null) {
			currentPickedUpItem.GetComponent<Rigidbody2D> ().gravityScale = objectGravityScale;
			Vector2 forceApplied = new Vector2 (pickedUpOffset.x * dropForceScalingFactor, pickedUpOffset.y * dropForceScalingFactor * 2.0f);
			currentPickedUpItem.GetComponent<Rigidbody2D> ().AddForce (forceApplied, ForceMode2D.Impulse);
		}
		if (currentPickedUpItem.GetComponent<PolygonCollider2D> () != null) {
			currentPickedUpItem.GetComponent<PolygonCollider2D> ().enabled = true;
		}
		if (currentPickedUpItem.GetComponent<BoxCollider2D> () != null) {
			currentPickedUpItem.GetComponent<BoxCollider2D> ().enabled = true;
		}
		currentPickedUpItem = null;
	}

	void PickUpItem() {
		Vector2 currentPosition = new Vector2 (transform.position.x, transform.position.y);
		Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll (currentPosition, pickUpRadius);

		foreach (Collider2D nearbyObject in nearbyObjects) {
			if (nearbyObject.gameObject.GetComponent<PickUpItem> () != null) {
				hasPickedUpObject = true;
				currentPickedUpItem = nearbyObject.gameObject;

				if (currentPickedUpItem.GetComponent<Rigidbody2D> () != null) {
					objectGravityScale = currentPickedUpItem.GetComponent<Rigidbody2D> ().gravityScale;
					currentPickedUpItem.GetComponent<Rigidbody2D> ().gravityScale = 0.0f;
				}
					
				if (disableObjectColliderWhileHeld) {
					if (currentPickedUpItem.GetComponent<PolygonCollider2D> () != null) {
						currentPickedUpItem.GetComponent<PolygonCollider2D> ().enabled = false;
					}
					if (currentPickedUpItem.GetComponent<BoxCollider2D> () != null) {
						currentPickedUpItem.GetComponent<BoxCollider2D> ().enabled = false;
					}
				}
				return;
			}
		}
	}
}
