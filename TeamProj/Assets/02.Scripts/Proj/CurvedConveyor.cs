//using UnityEngine;

//public class CurvedConveyor : MonoBehaviour
//{
//	public Transform[] points; // 곡선 경로 포인트들
//	public float speed = 2f;

//	private void OnCollisionStay(Collision collision)
//	{
//		Rigidbody rb = collision.rigidbody;
//		if (rb == null) return;

//		// 현재 물체와 가장 가까운 점을 찾음
//		Transform closest = points[0];
//		float closestDist = Vector3.Distance(collision.transform.position, closest.position);

//		foreach (Transform t in points)
//		{
//			float dist = Vector3.Distance(collision.transform.position, t.position);
//			if (dist < closestDist)
//			{
//				closestDist = dist;
//				closest = t;
//			}
//		}

//		// 다음 방향을 결정 (다음 포인트를 향하도록)
//		int idx = System.Array.IndexOf(points, closest);
//		if (idx < points.Length - 1)
//		{
//			Vector3 dir = (points[idx + 1].position - closest.position).normalized;
//			rb.AddForce(dir * speed, ForceMode.VelocityChange);
//		}
//	}
//}
