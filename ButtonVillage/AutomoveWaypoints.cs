using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomoveWaypoints : MonoBehaviour
{
    public bool Random;
    public float Speed = 1f;
    public float DistanceNeeded = 1f;

    public Vector3 GeneralOffset;
    public Vector3[] Waypoints;

    private Vector3 _currentObjective;
    private int _currentIndex;

    private System.Random _rnd;

	// Use this for initialization
	void Start ()
    {
        _rnd = new System.Random();
        GetObjective();
    }

    void GetObjective()
    {
        int next;
        if (Random)
        {
            next = _rnd.Next(Waypoints.Length);
            if (Waypoints[next] != Waypoints[_currentIndex])
            {
                _currentObjective = Waypoints[next];
                _currentIndex = next;
                return;
            }
        }

        next = _currentIndex;
        ++next;
        if (next >= Waypoints.Length)
            next = 0;

        _currentObjective = Waypoints[next];
        _currentIndex = next;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (Vector3.Distance(transform.position, _currentObjective + GeneralOffset) < DistanceNeeded)
        {
            GetObjective();
        }
        else
        {
            Vector3 direction = (_currentObjective + GeneralOffset) - transform.position;
            direction.Normalize();
            transform.Translate(direction * Time.deltaTime * Speed);
        }
	}

    private void OnDrawGizmosSelected()
    {
        if (Waypoints == null || Waypoints.Length == 0)
            return;

        Gizmos.color = Color.magenta;
        foreach(Vector3 waypoint in Waypoints)
        {
            Gizmos.DrawWireSphere(waypoint + GeneralOffset, DistanceNeeded);
        }
    }
}
