// C# implemenatation of above approach
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UCS
{
    /*
    See https://www.geeksforgeeks.org/uniform-cost-search-dijkstra-for-large-graphs/
    and also 
    https://www.baeldung.com/cs/find-path-uniform-cost-search
    */
    class GFG
    {
        //enable/disbale Console.WriteLine
        static bool DEBUG = true;
        
        // graph
        static List<List<int>> graph=new List<List<int>>();

        //nodes
        static Dictionary <string,int> _nodes=new Dictionary<string,int> ();
       static  List<Tuple<string,string,int>> graphandweight = new List<Tuple<string,string,int>> ();
        
        // map to store cost of edges
        static Dictionary<Tuple<int,int>,int> cost= new Dictionary<Tuple<int,int>,int>();
        static int weight(int _node1, int _node2){
            log($"weight (n1,n2) = ({_node1},{_node2})");
            return graph[_node1][_node2];
        }

        static (int,List<string>) uniformCost(string source, string goal) {
            List<Tuple<int,string,string,int>> nodes_q = new List<Tuple<int, string, string, int> >();
            Dictionary<string, bool> visited_nodes = new Dictionary<string, bool> ();
            Dictionary<int,List<Tuple<int,int>>> prev_nodes =
                    new Dictionary<int,List<Tuple<int,int>>> ();

            nodes_q.Add(new Tuple<int, string, string, int> (0,source.ToString(), "-1",0));

            foreach (var node in _nodes)
            {
                visited_nodes[node.Value.ToString()] = false;
                prev_nodes[node.Value] = null;
            }

            while (nodes_q.Count() != 0)
            {
                // total_cost, current_node, prev_node, link_cost
                Tuple<int,string,string,int> current = nodes_q[0];
                nodes_q.RemoveAt(0);
                
                int total_cost = current.Item1;
                string current_node = (current.Item2);
                string prev_node  =  (current.Item3);
                int link_cost = current.Item4;

                if(!visited_nodes[current_node])
                {
                    visited_nodes[current_node] = true;
                    
                    prev_nodes[int.Parse(current_node)] = new List<Tuple<int, int>> ();
                    
                    var tempTuple = new Tuple<int, int> (int.Parse(prev_node),link_cost);

                    prev_nodes[int.Parse(current_node)].Add(tempTuple);

                    if(current_node == goal){
                        List<string> final = new List<string> ();
                        while (current_node != source)
                        {
                            List<string> temp = new List<string>(){current_node};
                            foreach (var i in prev_nodes[int.Parse(current_node)])
                            {
                                temp.Add(i.Item1.ToString());
                            }
                            final.AddRange(temp);
                            current_node =prev_nodes[int.Parse(current_node)][0].Item1.ToString();

                        }
                        final.Reverse();
                        return (total_cost, final);
                    }

                    var currentNodeNeighbors = graphandweight.Where(x => x.Item1 == current_node).ToList();
                    foreach (var item in currentNodeNeighbors)
                    {
                        var neighbor = item.Item2;
                        var ncost = item.Item3;

                        if(!visited_nodes[neighbor]){
                            int this_link_cost = ncost;
                            int new_cost = total_cost + ncost;

                            //new_cost, neighbors, current_node, ncost
                            nodes_q.Add(new Tuple<int, string, string, int> (new_cost, neighbor, current_node, ncost)); 
                        }
                        
                    }

                }

            }


            return (0, new List<string>());
        }
        static (int,List<int>) ucs(int S, int D, int nodeCount){
            //Cost[] <- inifnity
            List<int> cost = new List<int> ();
            for (int i = 0; i < nodeCount; i++)
                cost.Add(int.MaxValue);
            
            //parent[] <- NULL
            List<string> parent= new List<string> ();
             for (int i = 0; i < nodeCount; i++)
                parent.Add(null);

            //priority queue to store the explored nodes sorted in the ascending order according to their cost value
            // priority_queue <- {}
            List<Tuple<int, int> > priority_queue = new List<Tuple<int, int> >();

            //priority_queue.add(S)
            priority_queue.Add(new Tuple<int, int>(S,0));

            //cost[S] <- 0;
            cost[S]=0;

            //while ¬priority_queue.empty() do
            while (priority_queue.Count > 0)
            {
                //current <- priority_queue.front()
                Tuple<int,int> current = priority_queue[0];

                //priority_queue.pop()
                priority_queue.RemoveAt(0);
                
                //foreach child ∈ G[current] do
                foreach (var child in graph[current.Item1])
                {
                    //if cost[child] > cost[current] + weight(current, child) then
                    if(cost[child] > cost[current.Item1] + weight(current.Item1, child))
                    {
                        //cost[child] <- cost[current] + weight(current, child)
                        cost[child] = cost[current.Item1] + weight(current.Item1, child);

                        //priority_queue.add(child)
                        priority_queue.Add(new Tuple<int, int>(child, cost[child]));

                        //parent[child] <- current
                        parent[child] = current.Item1.ToString();

                    }//if_end
                }//foreach_end                
            }//whild_end

            //path  <- {}
           List< int > path = new List< int >();

           //node <- D
           string node = D.ToString();

            //while node != NULL do
           while (node != null)
           {
               //path.add_front(node);
               path[0] = int.Parse(node);

               //node <- parent[node]
               node = parent[int.Parse(node)];
           }

            return(cost[D], path);
        }
        
        // returns the minimum cost in a vector( if
        // there are multiple goal states)
        static int uniform_cost_search(int startNode, int endNode)
        {
            List<int> goal =new List<int>();
            goal.Add(endNode);
            
            // minimum cost upto goal state from starting state
            List<int> answer=new List<int>();

            //list of path from start to end
            List<int> paths =new List<int> ();
        
            // create a priority queue - List of (node1, node2)
            List<Tuple<int, int> > queue = new List<Tuple<int, int> >();
        
            // set the answer vector to max value
            for (int i = 0; i < goal.Count; i++)
                answer.Add(int.MaxValue);
        
            // insert the starting index
            queue.Add(new Tuple<int,int>(0, startNode));

            //insert the starting node
            paths.Add(startNode);
        
            // map to store visited node
            Dictionary<int, int> visited=new Dictionary<int,int>();
        
            // count
            int count = 0;
            int ifCount = 0;
            int whileCount =0;
            int forCount = 0;
        
            // while the queue is not empty
            while (queue.Count > 0) {
                if(queue.Count > 0){
                    log($"Queue at whilecount{whileCount} = {queue[0].Item1},{queue[0].Item2}");
                }
                // get the top element of the priority queue
                Tuple<int, int> q = queue[0];
                Tuple<int, int> p = new Tuple<int,int>(-q.Item1,q.Item2); // item1 = cost ? , item2 = goal
        
                // pop the element
                queue.RemoveAt(0);
               
        
        
                // check if the element is part of the goal list
                if (goal.Contains(p.Item2)) {
                    log($"ifCount = {ifCount}");
                    ifCount++;
        
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
                        return answer[0];
                }
                log($"whileCount = {whileCount}");
                whileCount++;
        
                // check for the non visited nodes which are adjacent to present node
                if (!visited.ContainsKey(p.Item2))
                    for (int i = 0; i < GetAdjacentNodes(p.Item2).Count; i++) {
                        //log($"forCount = {forCount}");
                        forCount++;
        
                        // value is multiplied by -1 so that least priority is at the top
                        var theCost = cost[new Tuple<int,int>(p.Item2,  GetCost(p.Item2,i))];
                        var theMatchCondition = cost.ContainsKey(new Tuple<int,int>(p.Item2,  GetCost(p.Item2,i)));

                        queue.Add(new Tuple<int,int>(
                            (p.Item1 + ( theMatchCondition ? theCost : 0))*-1,
                            GetCost(p.Item2,i))                
                        );

                       if(!paths.Contains(p.Item2) && theMatchCondition) paths.Add(p.Item2);

                        if(queue.Count > 0){
                            int c=0;
                            foreach (var item in queue)
                            {
                                log($"forCount= {forCount} | Queue[{c}] = ({item.Item1},{item.Item2})");
                                c++;
                            }
                        }
                    }
        
                // mark as visited
                visited[p.Item2] = 1;
                
                //log visited
                var nodes = visited.Where(i => i.Value == 1).Select(x => x.Key).ToList();
                string csl =   string.Join(",",nodes);
                log($"visited nodes: {csl}");

                //log paths
                log($"paths list:{string.Join(" | ",paths)}");

            }
        
            return answer[0];
        }
        static int GetCost(int _node1, int _node2){
            return graph[_node1][_node2];
        }

        static List<int> GetAdjacentNodes(int _node){
            return  graph[_node];
        }
        
        static void log(string _input){
            if(DEBUG)
                Console.WriteLine(_input);
        }

        // main function
        public static void Main(params string []args)
        {        
            _nodes = UniformSerachGraph.GetNodes("nodes_1.csv");
            Console.WriteLine($"file nodes: {string.Join("|",_nodes.ToList())}");
            //----------------------

            graph = UniformSerachGraph.GetGraph("graph_1.csv", 6);
            // int gct = 0;
            // foreach (var gitem in graph)
            // {                
            //     Console.WriteLine($"file graphs: Node Index =[{gct}] {string.Join("|",gitem.ToList())}");
            //     gct++;
            // }
            
            //------------
            cost  =UniformSerachGraph.GetCost("cost_1.csv");

            graphandweight= UniformSerachGraph.GetGraphAndWeight("cost_1.csv");

            // int cct =0;
            // foreach (var citem in cost.ToList())
            // {
            //    // Console.WriteLine($"file Cost: (Node1, Node2)= ({citem.Key.Item1},{citem.Key.Item2}) --- Cost = {citem.Value} ");
            //     cct++;
            // }
            //------------
            // graph = UniformSerachGraph.CreateGraph();
            // cost = UniformSerachGraph.CreateCost();

            Console.Write("Enter name of START node = ");
            string _start = Console.ReadLine();
            Console.Write("Enter name of ENd node = ");
            string _end = Console.ReadLine();            

            int start = (_nodes[_start]);
            int end = _nodes[_end];

            Console.WriteLine($"Calculating uniform search from (start, end) = ({start},{end})...");
        
            // get the answer
            //int answer = uniform_cost_search(start, end);          
        
            // print the answer
            //Console.Write($"Minimum cost from {start} to {end} is = {answer}");

            //(int path_cost, List<int> paths)   = ucs(start, end, _nodes.Count());
            //Console.Write($"ucs:: Minimum cost from {start} to {end} is = {path_cost}");
            //Console.WriteLine($"ucs:: path is :{string.Join("-->",paths)}");


            (int totalCost, List<string> pathTaken) =  uniformCost(start.ToString(), end.ToString());
            Console.WriteLine($"uniformCost:: totalCost :{totalCost}");
            Console.WriteLine($"uniformCost:: pathTaken :{string.Join("--->", pathTaken)}");


            Console.ReadLine();
        }
    }
}