// C# implemenatation of above approach
using System;
using System.Collections;
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
        static  List<Tuple<string,string,int>> graphandweight = new List<Tuple<string,string,int>> ();               
        static (int,List<string>) uniformCost(string source, string goal) {
            //Define priority queue
            List<Tuple<int,string,string,int>> nodes_q = new List<Tuple<int, string, string, int> >();
            
            //Define visted nodes dictionary
            Dictionary<string, bool> visited_nodes = new Dictionary<string, bool> ();

            //Define prvous nodes Dictionary
            Dictionary<string,List<Tuple<string,int>>> prevousNodesAndCostsDictionary = new Dictionary<string,List<Tuple<string,int>>> ();

            //Intialised priority queue with start values
            //The tuple contains = (total_cost, current_node, prev_node, link_cost)
            nodes_q.Add(new Tuple<int, string, string, int> (0,source, "-1",0));
            
            //Initialise all visited to false
            foreach (var node in _nodes)
            {
                //Initialise all visited to false
                visited_nodes[node.Value.ToString()] = false;
                
                //Set all nodes' values of previous node to null
                prevousNodesAndCostsDictionary[node.Key] = null;
            }

            while (nodes_q.Count() != 0)
            {
                // total_cost, current_node, prev_node, link_cost
                
                //Define current and set it to the top of queue value 
                Tuple<int,string,string,int> current = nodes_q[0];

                //Pop the first element in priority queue
                nodes_q.RemoveAt(0);
                
                //Set values from tuple
                int total_cost = current.Item1;
                string current_node = current.Item2;
                string prev_node  =  current.Item3;
                int link_cost = current.Item4;

                //if the current node has not been visited yet, do
                if(!visited_nodes[current_node])
                {
                    visited_nodes[current_node] = true;
                    
                    prevousNodesAndCostsDictionary[current_node] = new List<Tuple<string, int>> ();
                    
                    var tempTuple = new Tuple<string, int> (prev_node,link_cost);

                    prevousNodesAndCostsDictionary[current_node].Add(tempTuple);
                    //log($" prevousNodesAndCostsDictionary[current_node] = { string.Join("|",prevousNodesAndCostsDictionary[current_node]) }");

                    if(current_node == goal){
                        List<string> final = new List<string> ();
                        while (current_node != source)
                        {                            
                            List<string> temp = new List<string>(){};
                            foreach (var prevNodeAndCostList in prevousNodesAndCostsDictionary[current_node])
                            {
                                //prevNodeAndCostList.Item1 is a node
                                temp.Add(prevNodeAndCostList.Item1.ToString());
                            }
                            final.AddRange(temp);
                            current_node = prevousNodesAndCostsDictionary[current_node][0].Item1.ToString();

                        }                        
                        final.Reverse();
                        final.Add(goal);

                        //Replace node index values to string names
                        List<string> pathNodes = new List<string> ();
                        foreach (var node in final)
                        {
                            //Find Dictionary key (string node name)  from string value (its integer index)
                            var name = _nodes.FirstOrDefault(x => x.Value == (int.Parse(node))).Key;
                            pathNodes.Add(name);                           
                        }
                        return (total_cost, pathNodes);
                    }

                    List<Tuple<string,string,int>> currentNodeNeighbors = new List<Tuple<string, string, int>> ();

                    foreach (var graphEntry in graphandweight)
                    {
                        if(graphEntry.Item1 == current_node){
                            currentNodeNeighbors.Add(graphEntry);
                        }                        
                    }


                    foreach (var tuple in currentNodeNeighbors)
                    {
                        var neighbor = tuple.Item2;
                        var ncost = tuple.Item3;

                        if(!visited_nodes[neighbor]){
                            //set this code 
                            int this_link_cost = ncost;

                            //calculate new cost
                            int new_cost = total_cost + ncost;

                            //new_cost, neighbors, current_node, ncost
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
            //Read node names from file
            _nodes = GetNodes("nodes_1.csv");
            Console.WriteLine($"file nodes: {string.Join("--",_nodes.ToList())}");          

            //Read graph and cost from file
            graphandweight= GetGraphAndWeight("cost_1.csv");

            //Console INput values
            Console.Write("Enter name of START node = ");
            string _start = Console.ReadLine();

            Console.Write("Enter name of END node = ");
            string _end = Console.ReadLine();            

            int start = (_nodes[_start]);
            int end = _nodes[_end];

            Console.WriteLine($"Calculating uniform search from (start, end) = ({start},{end})...");        

            (int totalCost, List<string> pathTaken) =  uniformCost(start.ToString(), end.ToString());
            Console.WriteLine($"uniformCost:: totalCost :{totalCost}");
            Console.WriteLine($"uniformCost:: pathTaken :{string.Join("--->", pathTaken)}");


            Console.ReadLine();
        }
    }
}