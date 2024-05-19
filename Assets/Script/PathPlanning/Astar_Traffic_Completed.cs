using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Map;
using System.Diagnostics;
using NetTopologySuite.Geometries;

namespace PathPlanning
{
    public class Astar_Traffic_Completed
    {
        public RoutePoint startPosition;
        public RoutePoint endPosition;
        public Graph graph;
        public List<ConnectionPoint> path = new List<ConnectionPoint>();

        public static List<ConnectionPoint> AstarAlgorithm_TC(Graph graph, RoutePoint startPosition, RoutePoint endPosition, float speed, float currentSpeedFactor = 1.0f)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Dictionary<RoutePoint, float> g_score = new Dictionary<RoutePoint, float>();
            Dictionary<RoutePoint, RoutePoint> previous = new Dictionary<RoutePoint, RoutePoint>();
            Dictionary<RoutePoint, float> h_score = new Dictionary<RoutePoint, float>();
            Dictionary<RoutePoint, float> f_score = new Dictionary<RoutePoint, float>();
            List<RoutePoint> openSet = new List<RoutePoint>();

            foreach (RoutePoint node in graph.RoutePoints)
            {
                g_score[node] = Mathf.Infinity;
                previous[node] = null;
                h_score[node] = hScore(node, endPosition, speed);
                f_score[node] = Mathf.Infinity;
                openSet.Add(node);
            }

            g_score[startPosition] = 0;
            f_score[startPosition] = hScore(startPosition, endPosition, speed);

            while (openSet.Count != 0)
            {
                RoutePoint current = openSet[0];
                foreach (RoutePoint node in openSet)
                {
                    if (f_score[node] < f_score[current])
                    {
                        current = node;
                    }
                }

                if (current == endPosition)
                {
                    stopwatch.Stop();
                    UnityEngine.Debug.Log("A* Algorithm Execution Time: " + stopwatch.ElapsedMilliseconds + "ms");
                    return GetPath(previous, startPosition, endPosition);
                }
                openSet.Remove(current);

                foreach (ConnectionPoint neighbor in current.Children)
                {
                    bool sequence = true;
                    RoutePoint neighborNode = graph.GetRoutePointFormConnectionPoint(neighbor);
                    Layer layerOfNeighbor = graph.GetLayerFromConnectionPoint(neighbor, sequence);
                    
                    float distance = (float)current.ConnectionPoint.Point.Distance(neighbor.Point);
                    float maxSpeed = speed;
                    if (maxSpeed > layerOfNeighbor.SpeedLimit())
                    {
                        maxSpeed = layerOfNeighbor.SpeedLimit();
                    }
                    
                    if (graph.NearIntersection(neighbor))
                    {
                        maxSpeed *= 0.5f;

                        if (graph.InIntersection(neighbor))
                        {
                            maxSpeed *= 2.0f;
                        }
                    }

                    if (graph.NearIntersection(current.ConnectionPoint, false))
                    {
                        maxSpeed = speed;
                        if (maxSpeed > layerOfNeighbor.SpeedLimit())
                        {
                            maxSpeed = layerOfNeighbor.SpeedLimit();
                        }
                    }

                    maxSpeed *= currentSpeedFactor;
                    float timeCost = distance / maxSpeed;
                    float tentative_gscore = g_score[current] + timeCost;

                    if (tentative_gscore < g_score[neighborNode])
                    {
                        previous[neighborNode] = current;
                        g_score[neighborNode] = tentative_gscore;
                        f_score[neighborNode] = g_score[neighborNode] + h_score[neighborNode];

                        if (!openSet.Contains(neighborNode))
                        {
                            openSet.Add(neighborNode);
                        }
                    }
                }
            }

            UnityEngine.Debug.Log("No path found");
            return new List<ConnectionPoint>();
        }

        private static List<ConnectionPoint> GetPath(Dictionary<RoutePoint, RoutePoint> previous, RoutePoint startPosition, RoutePoint endPosition)
        {
            List<ConnectionPoint> path = new List<ConnectionPoint>();

            RoutePoint current = endPosition;
            while (current != null && current != startPosition)
            {
                path.Add(current.ConnectionPoint);
                current = previous[current];
            }

            path.Add(startPosition.ConnectionPoint);
            path.Reverse();
            path.RemoveAt(path.Count - 1);
            
            List<string> pathIds = new List<string>();

            foreach (ConnectionPoint connectionPoint in path)
            {
                pathIds.Add(connectionPoint.Id);
            }
            UnityEngine.Debug.Log("Path: " + string.Join(" -> ", pathIds.ToArray()));

            return path;
        }

        private static float fScore(float gscore, float hscore)
        {
            return gscore + hscore;
        }

        private static float hScore(RoutePoint currentPosition, RoutePoint endPosition, float speed)
        {
            Point current = (Point)currentPosition.ConnectionPoint.Point;
            Point end = (Point)endPosition.ConnectionPoint.Point;

            float distance = (float)Math.Sqrt(Math.Pow(current.X - end.X, 2) + Math.Pow(current.Y - end.Y, 2));

            float timeCost = distance / speed;

            return timeCost;
        }
    }
}
