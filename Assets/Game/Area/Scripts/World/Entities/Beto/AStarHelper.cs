using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Linq;
using IA2Final;
using Debug = UnityEngine.Debug;

public class AStarHelper : MonoBehaviour
{
    public List<AStarNode> allNodes;
    int watchdog;
    int maxWatchdog = 200;

    [SerializeField] bool activeAStar = false;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (activeAStar)
        {
            AStarNode firstNode = GetRandomNode();
            ExecuteAStar(firstNode, GetRandomNode(firstNode), OnPathCompleted);
            activeAStar = false;
        }
    }

    public AStarNode GetRandomNode(AStarNode exceptWaypoint = null)
    {
        if (exceptWaypoint != null)
            allNodes.Remove(exceptWaypoint);

        var filtredList = allNodes.Where(x => !x.blocked).ToArray();

        var resultNode = filtredList[Random.Range(0, filtredList.Length)];


        if (exceptWaypoint != null)
            allNodes.Add(exceptWaypoint);
        return resultNode;
    }

    public AStarNode GetRandomNode(List<AStarNode> exceptWaypoint)
    {
        var filtredList = allNodes.Where(x => !x.blocked).Where(x=>!exceptWaypoint.Contains(x)).ToArray();

        var resultNode = filtredList[Random.Range(0, filtredList.Length)];

        return resultNode;
    }

    public AStarNode GetNearNode(Vector3 pos)
    {
        var filtredList = allNodes.Where(x => !x.blocked).ToArray();
        var result = filtredList[0];
        float currentDistance = Vector3.Distance(result.transform.position, pos);

        for (int i = 1; i < filtredList.Length; i++)
        {
            float newDistance = Vector3.Distance(filtredList[i].transform.position, pos);

            if (newDistance < currentDistance)
            {
                currentDistance = newDistance;
                result = filtredList[i];
            }
        }

        return result;
    }

    public List<AStarNode> GetNearNodes(Vector3 pos, float radious)
    {
        var result = allNodes.Where(x => !x.blocked).Where(x => Vector3.Distance(pos, x.transform.position) < radious).ToList();

        return result;
    }

    public void Initialize()
    {
        allNodes = GetComponentsInChildren<AStarNode>().ToList();

        StartCoroutine(InitializeNodes());
    }

    IEnumerator InitializeNodes()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        for (int i = 0; i < allNodes.Count; i++)
        {
            allNodes[i].InitializeNodes();

            if (stopwatch.ElapsedMilliseconds < 1f / 60f)
            {
                yield return null;
                stopwatch.Restart();
            }
        }
    }

    public void ExecuteAStar(AStarNode from, AStarNode to, System.Action<IEnumerable<AStarNode>> PlanComplete)
    {
        watchdog = maxWatchdog;

        var astar = new AStar<AStarNode>();
        astar.OnPathCompleted += PlanComplete;
        astar.OnCantCalculate += OnCantCalculate;

        var astarEnumerator = astar.Run(from,
                                        state => { return Satisfies(state, to); },
                                        node => Explode(node, ref watchdog),
                                        state => GetHeuristic(state, to));

        StartCoroutine(astarEnumerator);
    }

    private void OnPathCompleted(IEnumerable<AStarNode> sequence)
    {
        pathList = new List<AStarNode>();
        foreach (var act in sequence)
        {
            pathList.Add(act);
        }

        Debug.Log("WATCHDOG " + watchdog);
    }

    private void OnCantCalculate()
    {
        Debug.Log("No encontró camino");
    }

    List<AStarNode> pathList;

    private void OnDrawGizmos()
    {
        if (pathList == null || pathList.Count == 0) return;

        for (int i = 0; i < pathList.Count; i++)
        {
            Gizmos.DrawCube(pathList[i].transform.position, Vector3.one * 3);
        }
    }

    private static float GetHeuristic(AStarNode from, AStarNode goal) => from.heuristic + Vector3.Distance(from.transform.position, goal.transform.position);

    private static bool Satisfies(AStarNode state, AStarNode to) => state == to;

    private static IEnumerable<WeightedNode<AStarNode>> Explode(AStarNode node, ref int watchdog)
    {
        if (watchdog == 0) return Enumerable.Empty<WeightedNode<AStarNode>>();
        watchdog--;

        return node.adyacent
                      .Aggregate(new List<WeightedNode<AStarNode>>(), (possibleList, action) =>
                      {
                          possibleList.Add(new WeightedNode<AStarNode>(action, action.heuristic));
                          return possibleList;
                      });
    }
}