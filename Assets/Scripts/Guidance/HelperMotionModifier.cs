using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperMotionModifier : MonoBehaviour {
	public bool useParabolicMotion = false;
	public bool clampParabolaHalf = false;
	public float parabolicMotionOffset = 1.0f;


	public Vector2 GetThirdPoint(float x1, float x2, float y1, float y2) {
//		float yDiff = y2 - y1;
//		float xDiff = x2 - x1;

		Vector2 midPoint = new Vector2 (x1, y1) * 0.5f + new Vector2 (x2, y2) * 0.5f;
		Vector2 thirdPoint = new Vector2 (midPoint.x, midPoint.y) + Vector2.up * parabolicMotionOffset;
//		Vector2 normalOffset = new Vector2 (-xDiff / yDiff, 1.0f).normalized;
//		Vector2 thirdPoint = midPoint + normalOffset * parabolicMotionOffset;
		return thirdPoint;
	}

	public float[] CoefficientsFromPoints(float x1, float x2, float y1, float y2) {
		Vector2 thirdPoint = GetThirdPoint (x1, x2, y1, y2); 
		float x3 = thirdPoint.x;
		float y3 = thirdPoint.y;

		float denom = (x1 - x2) * (x1 - x3) * (x2 - x3);
		float A = (x3 * (y2 - y1) + x2 * (y1 - y3) + x1 * (y3 - y2)) / denom;
		float B = (x3 * x3 * (y1 - y2) + x2 * x2 * (y3 - y1) + x1 * x1 * (y2 - y3)) / denom;
		float C = (x2 * x3 * (x2 - x3) * y1 + x3 * x1 * (x3 - x1) * y2 + x1 * x2 * (x1 - x2) * y3) / denom;
		return new float[] {A, B, C};
	}
}
