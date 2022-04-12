using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPath : MonoBehaviour
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

    public List<Transform> points;

    [Header("Test Variables...")]
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

    // Update is called once per frame
    void Update()
    {
        if (!DataStore.drawPoints)
        {
            return;
        }
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

        if (time == float.NaN)
        {
            time = 0;
        }

        float DistY = initialVelocity.y * time - (0.5f) * DataStore.gravity * time * time;

        return DistY;
    }

    void FindAngle()
    {
        angle = Mathf.Atan(DataStore.initialVelocityDirn.y / DataStore.initialVelocityDirn.x);
    }

    void GetDirectionOfVelocity()
    {
        initialVelocity = DataStore.speed * DataStore.initialVelocityDirn;
    }
}
