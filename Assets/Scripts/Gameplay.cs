using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    [SerializeField]
    public Transform pointPrefab;

    [SerializeField, Range(10, 100)]
    int resolution = 10;

    [SerializeField, Range(2, 100)]
    float range = 4;

    [SerializeField]
    float maxPointScale = 0.4f;

    public Transform startingPoint;

    public float initialSpeed;

    public float gravity = 9.8f;

    public Transform[] points;

    [Header("Test Variables...")]
    public Transform testCube;
    [SerializeField] Vector3 initialVelocityDirection;
    [SerializeField] Vector3 initialVelocity;
    [SerializeField] float angle;

    private void Awake()
    {
        float rangeEffectedPosition = range / resolution;
        float rangeEffectedScale = Mathf.Clamp(rangeEffectedPosition, 0, maxPointScale);

        Vector3 position = Vector3.zero;
        var scale = Vector3.one * rangeEffectedScale;

        points = new Transform[resolution];
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab);
            position.x = i * rangeEffectedPosition;
            point.localPosition = position;
            point.localScale = scale;
            point.SetParent(transform, false);

            points[i] = point;
        }
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
        CalculateVariables();
        SetpositionInX();
        for (int i = 0; i < points.Length; i++) 
        {
            Transform point = points[i];
            Vector3 position = point.localPosition;
            position.y = FindPointionInY(position.x);
            point.localPosition = position;
        }
    }

    void SetpositionInX()
    {
        float rangeEffectedPosition = range / resolution;
        float rangeEffectedScale = Mathf.Clamp(rangeEffectedPosition, 0, maxPointScale);

        Vector3 position = Vector3.zero;
        var scale = Vector3.one * rangeEffectedScale;

        for (int i = 0; i < points.Length; i++)
        {
            Transform point = points[i];
            position.x = i * rangeEffectedPosition;
            point.localPosition = position;
            point.localScale = scale;
        }
    }

    void CalculateVariables()
    {
        range = initialSpeed * initialSpeed * Mathf.Sin(2f * angle) / gravity;
    }

    float FindPointionInY(float DistX)
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
