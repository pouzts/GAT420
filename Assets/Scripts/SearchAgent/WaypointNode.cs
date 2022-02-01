using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNode : Node
{
	public WaypointNode[] nextWaypoints;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.TryGetComponent<WaypointAgent>(out WaypointAgent waypointAgent))
		{
			if (waypointAgent.targetNode == this)
			{
				waypointAgent.targetNode = nextWaypoints[Random.Range(0, nextWaypoints.Length)];
			}
		}
	}
}
