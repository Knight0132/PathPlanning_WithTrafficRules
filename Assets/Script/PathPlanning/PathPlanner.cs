using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

namespace PathPlanning
{
    public enum SearchAlgorithm
    {
        Astar_Basic,
        Astar_Traffic_Completed,
        Astar_Traffic_Developing
    }

    public class PathPlanner
    {
        public Graph graph;
        public RoutePoint startPoint;
        public RoutePoint endPoint;
        public float speed;
        
        public static List<ConnectionPoint> FindPath(SearchAlgorithm searchAlgorithm, Graph graph, RoutePoint startPoint, RoutePoint endPoint, float speed)
        {
            switch (searchAlgorithm)
            {
                case SearchAlgorithm.Astar_Traffic_Developing:
                    return Astar_Traffic_Developing.AstarAlgorithm_TD(graph, startPoint, endPoint, speed);
                case SearchAlgorithm.Astar_Traffic_Completed:
                    return Astar_Traffic_Completed.AstarAlgorithm_TC(graph, startPoint, endPoint, speed);
                case SearchAlgorithm.Astar_Basic:
                    return Astar_Basic.AstarAlgorithm_Basic(graph, startPoint, endPoint);
                default:
                    return null;
            }
        }
    }
}
