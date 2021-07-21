// C# implemenatation of above approach
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UCS
{
    public static class UniformSerachGraph 
    {      
        //csv fiesl directory 
        static string CSV_DIR = Path.Combine(Directory.GetCurrentDirectory(),"Data");
        // graph
        static List<List<int>> graph=new List<List<int>>();
 
        // map to store cost of edges
        static Dictionary<Tuple<int,int>,int> cost= new Dictionary<Tuple<int,int>,int>();

        public static Dictionary<string, int> GetNodes(string fileNameWithExtension)
        {
            Dictionary<string, int> dict = new Dictionary<string, int> ();
            string _filePath = Path.Combine(CSV_DIR, $"{fileNameWithExtension}");
            var lines = File.ReadLines(_filePath).Select(a => a.Split(',')).ToList();
            lines.RemoveAt(0);
            lines.ForEach(x => dict.Add(x[1], int.Parse(x[0])));

            return dict;
        }

        public static List<List<int>> GetGraph(string fileNameWithExtension, int nodeCount){
             // create the graph
            graph=new List<List<int>>();
            var lines = File.ReadLines(Path.Combine(CSV_DIR, $"{fileNameWithExtension}"))
                            .Select(a => a.Split(','))
                            .ToList();
            lines.RemoveAt(0);

            for(int i=0;i< nodeCount+1;i++)
            {
                graph.Add(new List<int>());
            }

            lines.ForEach(line => graph[int.Parse(line[0])].Add(int.Parse(line[1])));

            return graph;
        }

        public static Dictionary<Tuple<int,int>,int> GetCost(string fileNameWithExtension) {
            Dictionary<Tuple<int,int>,int> cost = 
                new Dictionary<Tuple<int, int>, int> ();

            var lines = File.ReadLines(Path.Combine(CSV_DIR, $"{fileNameWithExtension}"))
                            .Select(a => a.Split(','))
                            .ToList();
            lines.RemoveAt(0);

            lines.ForEach(line => {
                cost[new Tuple<int,int>(int.Parse(line[0]), int.Parse(line[1]))] = int.Parse(line[2]);
            });

            return cost;
        }


        public static List<List<int>> CreateGraph()
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

            return graph;
        }

        public static Dictionary<Tuple<int,int>,int> CreateCost() {
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

            return cost;
        }

        public static List<int> AddGoal(int endNode){
            // goal state
        List<int> goal=new List<int>();
    
        // set the goal
        // there can be multiple goal states
        goal.Add(endNode);
        return goal;
        }
    }
}