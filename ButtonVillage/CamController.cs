using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [Header("Camera")]
    public Camera Camera;

    [Header("Targeting")]
    public bool AutoFindPlayer = true;
    public List<Transform> Targets;
    
    [Space][Header("Moving")]
    public Vector3 Offset;
    public float SmoothTime = 2f;


    [Space][Header("Zooming")]
    public float minSize = 20f;
    public float maxSize = 30f;
    public float ZoomMultiplier = 50f;

    [Space][Header("Camera limits")]
    public Vector2 TopLeftLimit;
    public Vector2 BottomRightLimit;

    private Vector3 _velocity;
    private Bounds _bounds;
    

    private void Start()
    {
        if (Camera == null)
            Camera = GetComponent<Camera>();

        if (AutoFindPlayer)
            FindPlayers();

        if (Targets.Count == 0)
            return;

        UpdateBounds();
        //transform.position = _bounds.center;
    }

    private void LateUpdate()
    {
        if (AutoFindPlayer)
            FindPlayers();

        if (Targets.Count == 0)
            return;

        UpdateBounds();
        Move();
        Zoom();
    }

    private void Zoom()
    {
        float dist = (float) Math.Sqrt(_bounds.size.x * _bounds.size.x + _bounds.size.z * _bounds.size.z);
        dist = Mathf.Clamp(dist, minSize, maxSize);
        float newZoom = Mathf.Lerp(minSize, maxSize, dist / ZoomMultiplier);
        Camera.orthographicSize = newZoom;
    }

    private void Move()
    {
        Vector3 centerPoint = _bounds.center;
        Vector3 newPosition = centerPoint + Offset;

        if (TopLeftLimit != BottomRightLimit)
        {
            float x = Mathf.Clamp(newPosition.x, BottomRightLimit.x, TopLeftLimit.x);
            float z = Mathf.Clamp(newPosition.z, BottomRightLimit.y, TopLeftLimit.y);
            newPosition = new Vector3(x, newPosition.y, z);
        }
        
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, SmoothTime);
    }

    void UpdateBounds()
    {
        if (Targets.Count == 0)
            return;

        _bounds = new Bounds(Targets[0].position, Vector3.zero);
        foreach(Transform target in Targets)
        {
            _bounds.Encapsulate(target.position);
        }
    }

    void FindPlayers()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
        Targets = new List<Transform>();

        foreach(GameObject go in gos)
        {
            Targets.Add(go.transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Vector3 TopLeft = new Vector3(TopLeftLimit.x, transform.position.y, TopLeftLimit.y);
        Vector3 TopRight= new Vector3(BottomRightLimit.x, transform.position.y, TopLeftLimit.y);
        Vector3 BottomLeft = new Vector3(TopLeftLimit.x, transform.position.y, BottomRightLimit.y);
        Vector3 BottomRight = new Vector3(BottomRightLimit.x, transform.position.y, BottomRightLimit.y);

        Gizmos.DrawLine(TopLeft, TopRight);
        Gizmos.DrawLine(TopRight, BottomRight);
        Gizmos.DrawLine(BottomRight, BottomLeft);
        Gizmos.DrawLine(BottomLeft, TopLeft);
    }
}
