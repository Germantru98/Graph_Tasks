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

                default:
                    break;
            }
        }
    }
}