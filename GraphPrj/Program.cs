using System;

namespace GraphPrj
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            int switch_on = int.Parse(Console.ReadLine());

            switch (switch_on)
            {
                case 1:
                    Graph g = new Graph("graph.txt");
                    g.Task_Ia_3();
                    break;

                case 2:
                    Graph test1 = new Graph("Test1.txt");
                    Graph test2 = new Graph("Test2.txt");
                    Console.WriteLine(test1);
                    Console.WriteLine(test2);
                    Graph.Task_IB_4(test1, test2);
                    break;

                case 3:
                    Graph test3 = new Graph("Test3.txt");
                    Console.Write(test3);
                    if (test3.Task_II_18())
                    {
                        Console.WriteLine("Граф с циклом");
                    }
                    else
                    {
                        Console.WriteLine("Граф ацикличен");
                    }
                    break;

                case 4:
                    break;

                case 5:
                    break;

                case 6:
                    Graph gr = new Graph("graph.txt");
                    Console.WriteLine(gr);
                    Vertex u1 = new Vertex(0);
                    Vertex u2 = new Vertex(1);
                    Vertex v = new Vertex(5);
                    gr.Task_IVa_15(u1, u2, v);
                    break;

                case 7:
                    Graph g7 = new Graph("graph.txt");
                    Console.WriteLine(g7);
                    g7.Task_IVb_FU(8);
                    break;

                case 8:
                    Graph g8 = new Graph("graph.txt");
                    Console.WriteLine(g8);
                    g8.Task_IVc_6();
                    break;

                default:
                    break;
            }
        }
    }
}