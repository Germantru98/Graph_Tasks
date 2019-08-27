using System;
using System.Collections.Generic;

namespace GraphPrj
{
    public class Edge : IComparable//РЕБРО
    {
        public Vertex from; //откуда
        public Vertex to; //куда
        public int weight = 1; //вес

        public Edge(Vertex From, Vertex To, int Weight)//конструктор
        {
            from = From;
            to = To;
            weight = Weight;
        }

        public Edge(int from, int to, int weight, Dictionary<Vertex, List<Edge>> VertexWeight)//конструктор
        {
            Vertex From = null;
            Vertex To = null;
            foreach (var v in VertexWeight)
            {
                if (v.Key.ID == from) From = v.Key;
                if (v.Key.ID == to) To = v.Key;
            }
            this.from = From;
            this.to = To;
            this.weight = weight;
        }

        public int CompareTo(object obj)
        {
            Edge edge = (Edge)obj;
            if (weight > edge.weight)
                return 1;
            else if (weight < edge.weight)
                return -1;
            else
                return 0;
        }
    }
}