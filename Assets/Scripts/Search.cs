using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Priority_Queue;

public static class Search
{
    public delegate bool SearchAlgorithm(GraphNode source, GraphNode destination, ref List<GraphNode> path, int maxSteps);

    static public bool BuildPath(SearchAlgorithm searchAlgorithm, GraphNode source, GraphNode destination, ref List<GraphNode> path, int steps = int.MaxValue)
    {
        if (source == null || destination == null) return false;

        // reset graph nodes
        GraphNode.ResetNodes();

        // search for path from source to destination nodes        
        bool found = searchAlgorithm(source, destination, ref path, steps);
        return found;
    }

    public static bool DFS(GraphNode source, GraphNode destination, ref List<GraphNode> path, int maxSteps)
    {
        bool found = false;

        // create stack
        var nodes = new Stack<GraphNode>();
        // push source onto stack
        nodes.Push(source);

        int steps = 0;
        while (!found && nodes.Count > 0 && steps++ < maxSteps)
        {
            // get top node of stack node (peek)
            var node = nodes.Peek();
            node.visited = true;

            bool forward = false;
            // search node neighbors/edges for unvisited node
            foreach (var neighbor in node.neighbors) 
            {
                // if neighbor is not visited, then push on stack
                if (!neighbor.visited)
                {
                    nodes.Push(neighbor);
                    forward = true;

                    // check if neighbor is the destination node
                    if (neighbor == destination)
                    {
                        // set found to true
                        found = true;
                    }

                    break;
                }
            }

            // if not moving forward, pop current node off stack
            if (!forward)
            {
                nodes.Pop();
            }
        }

        // convert stack path node to list
        path = new List<GraphNode>(nodes);
        // reverse path
        path.Reverse();

        return found;
    }

    public static bool BFS(GraphNode source, GraphNode destination, ref List<GraphNode> path, int maxSteps)
    {
        bool found = false;

        // create queue of graph nodes
        var nodes = new Queue<GraphNode>();

        // set source node visited to true
        source.visited = true;
        // enqueue source node
        nodes.Enqueue(source);

        // set the current number of steps
        int steps = 0;
        while (!found && nodes.Count > 0 && steps++ < maxSteps)
        {
            // dequeue node
            var node = nodes.Dequeue();
            // loop through neighbors/edges
            foreach (var neighbor in node.neighbors)
            {
                // if neighbor is not visited
                if (!neighbor.visited)
                {
                    // set visited to true
                    neighbor.visited = true;
                    // set neighbor's parent to node
                    neighbor.parent = node;
                    // enqueue neighbor
                    nodes.Enqueue(neighbor);
                }
                // if neighbor is the destination node
                if (neighbor == destination)
                {
                    // set found to true
                    found = true;
                    break;
                }
            }
        }

        // create a list of graph nodes (path)
        path = new List<GraphNode>();
        // if found is true
        if (found)
        {
            // create path from parents
            CreatePathFromParents(destination, ref path);
        }
        else 
        {
            path = nodes.ToList();
        }

        return found;
    }

    public static bool Dijkstra(GraphNode source, GraphNode destination, ref List<GraphNode> path, int maxSteps)
    {
        bool found = false;

        // create priority queue
        var nodes = new SimplePriorityQueue<GraphNode>();
        
        // set source node cost to 0
        source.cost = 0;
        // enqueue source node with source cost as the priority
        nodes.Enqueue(source, source.cost);

        // set current number of steps
        int steps = 0;
        while (!found && nodes.Count > 0 && steps++ < maxSteps)
        {
            // dequeue node
            var node = nodes.Dequeue();

            // check if node is the destination node
            if (node == destination)
            {
                // set found to true
                found = true;
                break;
            }

            foreach (var neighbor in node.neighbors)
            {
                neighbor.visited = true;

                // calculate costy to neighbor (cost + distance)
                float cost = node.cost + node.DistanceTo(neighbor);

                // if cost < neighbor cost, add to priority queue
                if (cost < neighbor.cost)
                {
                    // set neighbor cost to cost
                    cost = neighbor.cost;
                    // set neighbor partent to node
                    neighbor.parent = node;
                    // enqueue without duplicates, neighbor with cost as priority
                    nodes.EnqueueWithoutDuplicates(node, cost);
                }
            }
        }

        if (found)
        {
            // create parth from destination to source using node parents
            path = new List<GraphNode>();
            CreatePathFromParents(destination, ref path);
        }
        else 
        {
            path = nodes.ToList();
        }

        return found;
    }

    public static bool AStar(GraphNode source, GraphNode destination, ref List<GraphNode> path, int maxSteps)
    {
        bool found = false;

        // create priority queue
        var nodes = new SimplePriorityQueue<GraphNode>();

        // set source node cost to 0
        source.cost = 0;
        // set heuristic to the distance of the cource to the diestination
        float heuristic = Vector3.Distance(source.transform.position, destination.transform.position);
        // enqueue source node with source cost as the priority
        nodes.Enqueue(source, source.cost + heuristic);

        // set current number of steps
        int steps = 0;
        while (!found && nodes.Count > 0 && steps++ < maxSteps)
        {
            // dequeue node
            var node = nodes.Dequeue();

            // check if node is the destination node
            if (node == destination)
            {
                // set found to true
                found = true;
                break;
            }

            foreach (var neighbor in node.neighbors)
            {
                neighbor.visited = true;

                // calculate costy to neighbor (cost + distance)
                float cost = node.cost + node.DistanceTo(neighbor);

                // if cost < neighbor cost, add to priority queue
                if (cost < neighbor.cost)
                {
                    // set neighbor cost to cost
                    cost = neighbor.cost;
                    // set neighbor partent to node
                    neighbor.parent = node;

                    heuristic = Vector3.Distance(neighbor.transform.position, destination.transform.position);
                    // enqueue without duplicates, neighbor with cost + heuristic as priority
                    // the closer the neighbor is to the destination, the higher the priority
                    nodes.EnqueueWithoutDuplicates(node, cost + heuristic);
                }
            }
        }

        if (found)
        {
            // create parth from destination to source using node parents
            path = new List<GraphNode>();
            CreatePathFromParents(destination, ref path);
        }
        else
        {
            path = nodes.ToList();
        }

        return found;
    }

    public static void CreatePathFromParents(GraphNode node, ref List<GraphNode> path)
    {
        // while node not null
        while (node != null)
        {
            // add node to list path
            path.Add(node);
            // set node to node parent
            node = node.parent;
        }

        // reverse path
        path.Reverse();
    }
}
