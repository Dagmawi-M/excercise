// C# implemenatation of above approach
using System;
using System.Collections;
using System.Collections.Generic;
 
class GFG
{
 
// graph
static List<List<int>> graph=new List<List<int>>();
 
// map to store cost of edges
static Dictionary<Tuple<int,int>,int> cost= new Dictionary<Tuple<int,int>,int>();
 
// returns the minimum cost in a vector( if
// there are multiple goal states)
static List<int> uniform_cost_search(List<int> goal, int start)
{
    // minimum cost upto
    // goal state from starting
    // state
    List<int> answer=new List<int>();
 
    // create a priority queue
    List<Tuple<int, int> > queue = new List<Tuple<int, int> >();
 
    // set the answer vector to max value
    for (int i = 0; i < goal.Count; i++)
        answer.Add(int.MaxValue);
 
    // insert the starting index
    queue.Add(new Tuple<int,int>(0, start));
 
    // map to store visited node
    Dictionary<int, int> visited=new Dictionary<int,int>();
 
    // count
    int count = 0;
 
    // while the queue is not empty
    while (queue.Count > 0) {
 
        // get the top element of the
        // priority queue
        Tuple<int, int> q = queue[0];
        Tuple<int, int> p = new Tuple<int,int>(-q.Item1,q.Item2);
 
        // pop the element
        queue.RemoveAt(0);
 
 
        // check if the element is part of
        // the goal list
        if (goal.Contains(p.Item2)) {
 
            // get the position
            int index = goal.IndexOf(p.Item2);
 
            // if a new goal is reached
            if (answer[index] == int.MaxValue)
                count++;
 
            // if the cost is less
            if (answer[index] > p.Item1)
                answer[index] = p.Item1;
 
            // pop the element
            queue.RemoveAt(0);
 
            // if all goals are reached
            if (count == goal.Count)
                return answer;
        }
 
        // check for the non visited nodes
        // which are adjacent to present node
        if (!visited.ContainsKey(p.Item2))
            for (int i = 0; i < GetAdjacentNodes(p.Item2).Count; i++) {
 
                // value is multiplied by -1 so that
                // least priority is at the top
                var theCost = cost[new Tuple<int,int>(p.Item2,  GetEdge(p.Item2,i))];
                var theMatchCondition = cost.ContainsKey(new Tuple<int,int>(p.Item2,  GetEdge(p.Item2,i)));

                queue.Add(new Tuple<int,int>(
                    (p.Item1 + ( theMatchCondition ? theCost : 0))*-1,
                    GetEdge(p.Item2,i))                
                );
            }
 
        // mark as visited
        visited[p.Item2] = 1;
    }
 
    return answer;
}
static int GetEdge(int _node1, int _node2){
    return graph[_node1][_node2];
}

static List<int> GetAdjacentNodes(int _node){
   return  graph[_node];
}

// main function
public static void Main(params string []args)
{
    // create the graph
    graph=new List<List<int>>();
 
    for(int i=0;i<7;i++)
    {
        graph.Add(new List<int>());
    }
 
    // add edge
    graph[0].Add(1);
    graph[0].Add(3);
    graph[3].Add(1);
    graph[3].Add(6);
    graph[3].Add(4);
    graph[1].Add(6);
    graph[4].Add(2);
    graph[4].Add(5);
    graph[2].Add(1);
    graph[5].Add(2);
    graph[5].Add(6);
    graph[6].Add(4);
 
    // add the cost
    cost[new Tuple<int,int>(0, 1)] = 2;
    cost[new Tuple<int,int>(0, 3)] = 5;
    cost[new Tuple<int,int>(1, 6)] = 1;
    cost[new Tuple<int,int>(3, 1)] = 5;
    cost[new Tuple<int,int>(3, 6)] = 6;
    cost[new Tuple<int,int>(3, 4)] = 2;
    cost[new Tuple<int,int>(2, 1)] = 4;
    cost[new Tuple<int,int>(4, 2)] = 4;
    cost[new Tuple<int,int>(4, 5)] = 3;
    cost[new Tuple<int,int>(5, 2)] = 6;
    cost[new Tuple<int,int>(5, 6)] = 3;
    cost[new Tuple<int,int>(6, 4)] = 7;
 
    // goal state
    List<int> goal=new List<int>();
 
    // set the goal
    // there can be multiple goal states
    goal.Add(6);
 
    // get the answer
    List<int> answer = uniform_cost_search(goal, 0);
 
    // print the answer
    Console.Write("Minimum cost from 0 to 6 is = " + answer[0]);
  Console.ReadLine();
}
}