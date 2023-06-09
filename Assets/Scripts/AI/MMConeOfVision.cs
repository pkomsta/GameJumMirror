﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

   
    public class MMConeOfVision : MonoBehaviour
    {

        public struct RaycastData
        {
            public bool Hit;
            public Vector3 Point;
            public float Distance;
            public float Angle;

            public RaycastData(bool hit, Vector3 point, float distance, float angle)
            {
                Hit = hit;
                Point = point;
                Distance = distance;
                Angle = angle;
            }
        }

        public struct MeshEdgePosition
        {
            public Vector3 PointA;
            public Vector3 PointB;

            public MeshEdgePosition(Vector3 pointA, Vector3 pointB)
            {
                PointA = pointA;
                PointB = pointB;
            }
        }

        [Header("Vision")]
        public LayerMask ObstacleMask;
        public float VisionRadius = 5f;
        [Range(0f, 360f)]
        public float VisionAngle = 20f;
        public Vector3 Direction;
        public Vector3 EulerAngles;
        public Vector3 Offset;
        [HideInInspector]
        public float VisionRadiusSmall;
        [HideInInspector]
        public float VisionRadiusBig;

    [Header("Target scanning")]
        public bool ShouldScanForTargets = true;
        public LayerMask TargetMask;
        public float ScanFrequencyInSeconds = 1f;
  
        public List<Transform> VisibleTargets = new List<Transform>();
        
        [Header("Mesh")]
        public bool ShouldDrawMesh = true;
        public float MeshDensity = 0.2f;
        public int EdgePrecision = 3;
        public float EdgeThreshold = 0.5f;

        public MeshFilter VisionMeshFilter;
        public MeshRenderer MeshRenderer;
        public List<Material> Materials;
        protected Mesh _visionMesh;
        protected Collider[] _targetsWithinDistance; 
        protected Transform _target;
        protected Vector3 _directionToTarget;
        protected float _distanceToTarget;
        protected float _lastScanTimestamp;

        protected List<Vector3> _viewPoints = new List<Vector3>();
        protected RaycastData _oldViewCast = new RaycastData();
        protected RaycastData _viewCast = new RaycastData();
        protected Vector3[] _vertices;
        protected int[] _triangles;
        protected Vector3 _minPoint, _maxPoint, _direction;
        protected RaycastData _returnRaycastData;
        protected RaycastHit _raycastAtAngleHit;
        protected int _numberOfVerticesLastTime = 0;


        
        public Vector3 Center { get { return this.transform.position + Offset;  } }

        private Enemy enemy;
        protected virtual void Awake()
        {
           
            enemy = GetComponent<Enemy>();
            _visionMesh = new Mesh();
            if (ShouldDrawMesh)
            {
                VisionMeshFilter.mesh = _visionMesh;    
            }
            VisionRadiusSmall = VisionRadius;
            VisionRadiusBig = VisionRadius*2;

    }

    protected virtual void LateUpdate()
        {
         

            if ((Time.time - _lastScanTimestamp > ScanFrequencyInSeconds) && ShouldScanForTargets)
            {
                ScanForTargets();
            }
            DrawMesh();

    }

        public virtual void SetDirectionAndAngles(Vector3 direction, Vector3 eulerAngles)
        {
        Direction = direction;
        EulerAngles = eulerAngles;
    }

    protected virtual void ScanForTargets()
    {
        _lastScanTimestamp = Time.time;
        VisibleTargets.Clear();
        _targetsWithinDistance = Physics.OverlapSphere(Center, VisionRadius, TargetMask);
        if (_targetsWithinDistance.Length != 0)
        {
            Collider collider = _targetsWithinDistance[0];
            _target = collider.transform;
            _directionToTarget = (_target.position - Center).normalized;
            if (Vector3.Angle(Direction, _directionToTarget) < VisionAngle / 2f)
            {
                _distanceToTarget = Vector3.Distance(Center, _target.position);

                bool duplicate = false;
                foreach (Transform visibleTarget in VisibleTargets)
                {
                    if (visibleTarget == _target)
                    {
                        duplicate = true;
                    }
                }

                if ((!Physics.Raycast(Center, _directionToTarget, _distanceToTarget, ObstacleMask)) && !duplicate)
                {
                    VisibleTargets.Add(_target);
                    enemy.canSeePlayer = true;
                    Debug.Log("Target added");

                }
                else
                {
                    enemy.canSeePlayer = false;
                }
            }
            else
            {
                enemy.canSeePlayer = false;
            }
        }
        else if (enemy.canSeePlayer)
        {
            enemy.canSeePlayer = false;
        }
    }

    protected virtual void DrawMesh()
    {
        if (!ShouldDrawMesh)
        {
            return;
        }

        int steps = Mathf.RoundToInt(MeshDensity * VisionAngle);
        float stepsAngle = VisionAngle / steps;

        _viewPoints.Clear();

        for (int i = 0; i <= steps; i++)
        {
            float angle = stepsAngle * i + EulerAngles.y - VisionAngle / 2f;
            _viewCast = RaycastAtAngle(angle);

            if (i > 0)
            {
                bool thresholdExceeded = Mathf.Abs(_oldViewCast.Distance - _viewCast.Distance) > EdgeThreshold;

                if ((_oldViewCast.Hit != _viewCast.Hit) || (_oldViewCast.Hit && _viewCast.Hit && thresholdExceeded))
                {
                    MeshEdgePosition edge = FindMeshEdgePosition(_oldViewCast, _viewCast);
                    if (edge.PointA != Vector3.zero)
                    {
                        _viewPoints.Add(edge.PointA);
                    }
                    if (edge.PointB != Vector3.zero)
                    {
                        _viewPoints.Add(edge.PointB);
                    }
                }
            }

            _viewPoints.Add(_viewCast.Point);
            _oldViewCast = _viewCast;
        }

        int numberOfVertices = _viewPoints.Count + 1;
        if (numberOfVertices != _numberOfVerticesLastTime)
        {
            Array.Resize(ref _vertices, numberOfVertices);
            Array.Resize(ref _triangles, (numberOfVertices - 2) * 3);
        }

        _vertices[0] = Offset;
        for (int i = 0; i < numberOfVertices - 1; i++)
        {
            _vertices[i + 1] = this.transform.InverseTransformPoint(_viewPoints[i]);

            if (i < numberOfVertices - 2)
            {
                _triangles[i * 3] = 0;
                _triangles[i * 3 + 1] = i + 1;
                _triangles[i * 3 + 2] = i + 2;
            }
        }

        _visionMesh.Clear();
        _visionMesh.vertices = _vertices;
        _visionMesh.triangles = _triangles;
        _visionMesh.RecalculateNormals();

        _numberOfVerticesLastTime = numberOfVertices;
    }

    MeshEdgePosition FindMeshEdgePosition(RaycastData minimumViewCast, RaycastData maximumViewCast)
    {
        float minAngle = minimumViewCast.Angle;
        float maxAngle = maximumViewCast.Angle;
        _minPoint = minimumViewCast.Point;
        _maxPoint = maximumViewCast.Point;

        for (int i = 0; i < EdgePrecision; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            RaycastData newViewCast = RaycastAtAngle(angle);

            bool thresholdExceeded = Mathf.Abs(minimumViewCast.Distance - newViewCast.Distance) > EdgeThreshold;
            if (newViewCast.Hit == minimumViewCast.Hit && !thresholdExceeded)
            {
                minAngle = angle;
                _minPoint = newViewCast.Point;
            }
            else
            {
                maxAngle = angle;
                _maxPoint = newViewCast.Point;
            }
        }

        return new MeshEdgePosition(_minPoint, _maxPoint);
    }

    RaycastData RaycastAtAngle(float angle)
    {
        _direction = DirectionFromAngle(angle, 0f);


        if (Physics.Raycast(Center, _direction, out _raycastAtAngleHit, VisionRadius, ObstacleMask))
        {
            //Debug.Log("ObstacleHit");
            _returnRaycastData.Hit = true;
            _returnRaycastData.Point = _raycastAtAngleHit.point;
            _returnRaycastData.Distance = _raycastAtAngleHit.distance;
            _returnRaycastData.Angle = angle;
        }
        else
        {
            _returnRaycastData.Hit = false;
            _returnRaycastData.Point = Center + _direction * VisionRadius;
            _returnRaycastData.Distance = VisionRadius;
            _returnRaycastData.Angle = angle;
        }

        return _returnRaycastData;
    }
    public static Vector3 DirectionFromAngle(float angle, float additionalAngle)
    {
        angle += additionalAngle;

        Vector3 direction = Vector3.zero;
        direction.x = Mathf.Sin(angle * Mathf.Deg2Rad);
        direction.y = 0f;
        direction.z = Mathf.Cos(angle * Mathf.Deg2Rad);
        return direction;
    }
    protected virtual void Update()
    {
        UpdateDirection();
    }

    protected virtual void UpdateDirection()
    {
        
     SetDirectionAndAngles(this.transform.forward, this.transform.eulerAngles);
        
        
    }

}
