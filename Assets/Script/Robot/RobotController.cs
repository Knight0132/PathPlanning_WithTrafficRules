using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetTopologySuite.Geometries;
using PathPlanning;
using Map;

namespace Robot {
    public class RobotController : MonoBehaviour
    {
        public MapLoader mapLoader;
        public Transform targetIndicator;
        public float currentSpeed = 5.0f;
        public SearchAlgorithm selectedAlgorithm = SearchAlgorithm.Astar_Traffic_Completed;

        private Vector3 currentTarget;
        private int pathIndex = 0;
        private bool isMoving = false;
        private List<ConnectionPoint> path = new List<ConnectionPoint>();
        private CellSpace cellSpace;
        private RoutePoint routePoint;
        private ConnectionPoint connectionPoint;
        private Layer layer;
        private Graph graph;
        private IndoorSpace indoorSpace;
        private float startTime;
        private float elapsedTime;



        void Start()
        {
            indoorSpace = mapLoader.LoadJson();
            graph = mapLoader.GenerateRouteGraph(indoorSpace); 
            SetStart(mapLoader.startPoint);
        }

        void Update()
        {
            if (!isMoving)
            {
                SetNewTarget(mapLoader.endPoint);
            }
            else if (pathIndex < path.Count)
            {
                MoveTowardsNext();
            }
            else
            {
                isMoving = false;
            }
        }

        void SetStart(Vector3 start)
        {
            transform.position = start;
            Debug.Log("Start position set to: " + start);
        }

        void SetNewTarget(Vector3 end)
        {
            currentTarget = end;
            targetIndicator.position = currentTarget;
            RoutePoint startPosition = graph.GetRoutePointFromCoordinate(transform.position, true);
            RoutePoint endPosition = graph.GetRoutePointFromCoordinate(currentTarget, true);
            if (startPosition != null && endPosition != null)
            {
                path = PathPlanner.FindPath(selectedAlgorithm, graph, startPosition, endPosition, currentSpeed);
                mapLoader.mapVisualizer.VisualizePath(indoorSpace, path);
                isMoving = true;
                pathIndex = 0;
                startTime = Time.time;
            }
            else
            {
                Debug.LogError("One of the points is invalid.");
            }
        }

        void MoveTowardsNext()
        {
            if (pathIndex < path.Count)
            {
                Vector3 targetPosition = graph.GetCoordinatesFromConnectionPoint(path[pathIndex]);
                CellSpace targetCellSpace = this.indoorSpace.GetCellSpaceFromConnectionPoint(path[pathIndex], false);
                Layer targetLayer = graph.GetLayerFromConnectionPoint(path[pathIndex], false);

                if (selectedAlgorithm == SearchAlgorithm.Astar_Traffic_Completed)
                {
                    currentSpeed = targetLayer.SpeedLimit();
                    if (graph.NearIntersection(path[pathIndex]))
                    {
                        currentSpeed *= 0.5f;
                        if (graph.InIntersection(path[pathIndex]))
                        {
                            currentSpeed *= 2.0f;
                        }
                    }
                }
                else if (selectedAlgorithm == SearchAlgorithm.Astar_Traffic_Developing)
                {
                    currentSpeed = targetLayer.SpeedLimit();
                }
                else if (selectedAlgorithm == SearchAlgorithm.Astar_Basic)
                {
                    currentSpeed = currentSpeed;
                }
                else
                {
                    Debug.LogError("Invalid algorithm selected.");
                }

                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * currentSpeed);
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    pathIndex++;

                    if (pathIndex == path.Count)
                    {
                        elapsedTime = Time.time - startTime;
                        Debug.Log("Path completed in: " + elapsedTime + " seconds.");
                        isMoving = false;
                    }
                }
            }
        }
    }
}
