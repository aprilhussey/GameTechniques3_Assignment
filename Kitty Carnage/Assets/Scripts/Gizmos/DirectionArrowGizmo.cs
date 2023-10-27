using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DirectionArrowGizmo : MonoBehaviour
{
	private float arrowLineLength = 2f;
	private float arrowHeadLength = 0.25f;
	private float arrowHeadAngle = 20f;

	void OnDrawGizmos()
	{
		Vector3 direction = transform.TransformDirection(Vector3.forward);
		Gizmos.color = Color.magenta;
		Gizmos.DrawRay(transform.position, direction * arrowLineLength);
		DrawArrowEnd(false, transform.position, direction * arrowLineLength, Gizmos.color, arrowHeadLength, arrowHeadAngle);
	}

	private static void DrawArrowEnd(bool is2D, Vector3 position, Vector3 direction, Color color, float arrowHeadLength, float arrowHeadAngle)
	{
		Gizmos.color = color;
		Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(is2D ? new Vector3(0, 0, -arrowHeadAngle) : new Vector3(-arrowHeadAngle, 0, 0)) * new Vector3(0, 0, 1);
		Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(is2D ? new Vector3(0, 0, arrowHeadAngle) : new Vector3(arrowHeadAngle, 0, 0)) * new Vector3(0, 0, 1);
		Gizmos.DrawRay(position + direction, right * arrowHeadLength);
		Gizmos.DrawRay(position + direction, left * arrowHeadLength);
	}
}
