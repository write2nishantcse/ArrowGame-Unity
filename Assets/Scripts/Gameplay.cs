using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Main Repo master change 1


public class Gameplay : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefab;

    [SerializeField]
    float pointSize;

    [SerializeField]
    float spacing;

    [SerializeField, Range(10, 100)]
    int resolution = 10;

    public Transform startingPoint;

    public float initialSpeed;

    public float gravity = 9.8f;

    public List<Transform> points;

    [Header("Test Variables...")]
    public Transform testCube;
    [SerializeField] Vector3 initialVelocityDirection;
    [SerializeField] Vector3 initialVelocity;
    [SerializeField] float angle;
    [SerializeField] int lastResolution;

    private void Awake()
    {
        InstantiatePointsBasedOnResolution();
        SetpositionInX();
    }

    void InstantiatePointsBasedOnResolution()
    {
        if (points == null)
        {
            points = new List<Transform>();
        }
        int i = 0;
        for (i = 0; i < resolution; i++)
        {
            // create new point
            if (i >= points.Count)
            {
                Transform point = Instantiate(pointPrefab);
                point.parent = this.transform;
                points.Add(point);

            }
            else
            {
                points[i].gameObject.SetActive(true);
            }
        }

        // If extra points are there in list
        if (i < points.Count)
        {
            for (; i < points.Count; i++)
            {
                points[i].gameObject.SetActive(false);
            }
        }

        lastResolution = resolution;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startingPoint.position;
        GetDirectionOfVelocity();
        FindAngle();
        SetpositionInX();
        for (int i = 0; i < points.Count; i++) 
        {
            Transform point = points[i];
            Vector3 position = point.localPosition;
            position.y = FindPositionInY(position.x);
            point.localPosition = position;
        }
    }

    void SetpositionInX()
    {
        if (lastResolution != resolution)
        {
            InstantiatePointsBasedOnResolution();
        }

        Vector3 position = Vector3.zero;
        var scale = Vector3.one * pointSize;

        for (int i = 0; i < points.Count; i++)
        {
            Transform point = points[i];
            position.x = i * spacing;
            point.localPosition = position;
            point.localScale = scale;
        }
    }

    float FindPositionInY(float DistX)
    {
        // Calculate time
        float time = DistX / initialVelocity.x;

        float DistY = initialVelocity.y * time - (0.5f) * gravity * time * time;

        return DistY;
    }

    void FindAngle()
    {
        angle = Mathf.Atan(initialVelocityDirection.y / initialVelocityDirection.x);
    }

    void GetDirectionOfVelocity()
    {
        Vector3 camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        camPos.z = 0;
        testCube.transform.position = camPos;
        Debug.DrawLine(startingPoint.position, camPos);
        initialVelocityDirection = (camPos - startingPoint.position).normalized;
        initialVelocity = initialSpeed * initialVelocityDirection;
    }
}
