using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UCS
{  
    class GFG
    {
        static string CSV_DIR = Path.Combine(Directory.GetCurrentDirectory(),"Data");
        static bool DEBUG = true;   
        static Dictionary <string,int> _nodes = new Dictionary<string,int> ();        
                   
        static (int,List<string>) uniformCost(List<Tuple<string,string,int>> graph, string source, string goal) {
            //Define priority queue
            List<Tuple<int,string,string,int>> nodes_q = new List<Tuple<int, string, string, int> >();
            
            //Define visted nodes dictionary
            Dictionary<string, bool> visited_nodes = new Dictionary<string, bool> ();

            //Define prvous nodes Dictionary
            Dictionary<string,List<Tuple<string,int>>> prevousNodesAndCostsDictionary = new Dictionary<string,List<Tuple<string,int>>> ();

            //Intialised priority queue with start values
            //The tuple contains = (total_cost, current_node, prev_node, link_cost)
            nodes_q.Add(new Tuple<int, string, string, int> (0,source, "None",0));
            
            //Initialise all visited to false
            foreach (var node in _nodes)
            {
                //Initialise all visited to false
                visited_nodes[node.Key] = false;
                
                //Set all nodes' values of previous node to null
                prevousNodesAndCostsDictionary[node.Key] = null;
            }

            while (nodes_q.Count() != 0)
            {
                //Get the topOfQueue entry and set it to the top of queue value 
                Tuple<int,string,string,int> topOfQueue = nodes_q[0];

                //Set values from tuple
                int total_cost = topOfQueue.Item1;
                string current_node = topOfQueue.Item2;
                string prev_node  =  topOfQueue.Item3;
                int link_cost = topOfQueue.Item4;

                /* Pop the first(top of queue) element from queue
                   This is because the next block is about to process it
                   and is shold no longer be at the top of the queue */
                nodes_q.RemoveAt(0);

                //if the current node has not been visited yet, do               
                if(!visited_nodes[current_node])
                {
                    //set node as visited
                    visited_nodes[current_node] = true; 

                    //initialise new list
                    prevousNodesAndCostsDictionary[current_node] = new List<Tuple<string, int>> (); 
                    
                    //Create a (prev_node,cost) tuple variable
                    Tuple<string, int> tempTuple = new Tuple<string, int> (prev_node, link_cost);

                    //Add (prev_node,cost) to current node's dictionary
                    prevousNodesAndCostsDictionary[current_node].Add(tempTuple);

                    /* If the destination node has been reached, then do
                       The results are returned in this block */
                    if(current_node == goal)
                    {                        
                        List<string> finalPathList = new List<string> ();
                        while (current_node != source)
                        {                            
                            List<string> tempNodes = new List<string>(){};
                            foreach (var prevNodeAndCostList in prevousNodesAndCostsDictionary[current_node])
                            {
                                //prevNodeAndCostList.Item1 is a node name string, it is added
                                tempNodes.Add(prevNodeAndCostList.Item1.ToString());
                            }

                            //Add the 'list' of nodes to the final list
                            finalPathList.AddRange(tempNodes);
                            
                            /*
                                Here take the first entry in the list of negihbors, picks the node name (Item1) 
                                Set the current_node value to this node. 
                                Doing so will, in the next iterations, pick up nodes, travelling back wards through 
                                the uniform cost efficent path
                            */
                            current_node = prevousNodesAndCostsDictionary[current_node][0].Item1.ToString();
                        }
                        //Reverse order as nodes were appended from destination -> source            
                        finalPathList.Reverse();

                        //Add the name of the destination node
                        finalPathList.Add(goal);

                        return (total_cost, finalPathList);
                    }

                    List<Tuple<string,string,int>> currentNodeNeighbors = new List<Tuple<string, string, int>> ();

                    /* Adds the current_node's neighbors from the graph*/
                    foreach (var graphEntry in graph)
                    {                        
                        if(graphEntry.Item1 == current_node)
                            currentNodeNeighbors.Add(graphEntry);                        
                    }

                    //log($"currentNodeNeighbors = {string.Join("|",currentNodeNeighbors)}");
                    foreach (var tuple in currentNodeNeighbors)
                    {
                        string neighbor = tuple.Item2;
                        int ncost = tuple.Item3;

                        if(!visited_nodes[neighbor]){
                            //set this cost from this node to its negihbor 
                            int this_link_cost = ncost;

                            //calculate new cost FROM source
                            int new_cost = total_cost + ncost;

                            //Add to the computed values, prev & current nodes to the queue
                            nodes_q.Add(new Tuple<int, string, string, int> (new_cost, neighbor, current_node, ncost)); 
                        }                        
                    }
                }
            }

            return (0, new List<string>());
        }
        static void log(string _input){
            if(DEBUG)
                Console.WriteLine(_input);
        }

        /// Returns the graph connection and weight values from csv file. 

        public static List<Tuple<string,string,int>> GetGraphAndWeight(string fileNameWithExtension) {
            List<Tuple<string,string,int>>gnw = 
                new List<Tuple<string,string,int>> ();

            var lines = File.ReadLines(Path.Combine(CSV_DIR, $"{fileNameWithExtension}"))
                            .Select(a => a.Split(','))
                            .ToList();
            lines.RemoveAt(0);

            lines.ForEach(line => {                
                gnw.Add(new Tuple<string,string,int>(line[0],line[1],int.Parse(line[2]) ));
            });

            return gnw;
        }
        ///Returns the Node names and indices.
        public static Dictionary<string, int> GetNodes(string fileNameWithExtension)
        {
            Dictionary<string, int> dict = new Dictionary<string, int> ();
            string _filePath = Path.Combine(CSV_DIR, $"{fileNameWithExtension}");
            var lines = File.ReadLines(_filePath).Select(a => a.Split(',')).ToList();
            lines.RemoveAt(0);
            lines.ForEach(x => dict.Add(x[0], int.Parse(x[1])));

            return dict;
        }        

        // main function
        public static void Main(params string []args)
        {        
            List<Tuple<string,string,int>> graphandweight = new List<Tuple<string,string,int>> ();
            /* Read node names from file*/
            //_nodes = GetNodes("nodes_1.csv");
            _nodes = GetNodes("taxidrivernodes.csv");            
            Console.WriteLine($"file nodes: {string.Join("--",_nodes.ToList())}");          

            /* Read graph connections and cost from file */
            //graphandweight= GetGraphAndWeight("cost_1.csv");
            graphandweight = GetGraphAndWeight("taxidrivergraph.csv");

            //Fetch Console Input values
            Console.Write("Enter name of START node = ");
            string _start = Console.ReadLine().Trim();

            Console.Write("Enter name of END node = ");
            string _end = Console.ReadLine().Trim();  

            Console.WriteLine($"Calculating uniform search from (start, end) = ({_start},{_end})...");

            //Execute uniform cost search
            (int totalCost, List<string> pathTaken) =  uniformCost(graphandweight, _start, _end);

            //Display uniform cost search results
            Console.WriteLine($"uniformCost:: totalCost :{totalCost}");
            Console.WriteLine($"uniformCost:: pathTaken :{string.Join("--->", pathTaken)}");

            Console.ReadLine();
        }
    }
}