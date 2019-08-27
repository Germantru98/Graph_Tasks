using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GraphPrj
{
    public class Graph
    {
        //представление графа в виде списка смежности
        private Dictionary<Vertex, List<Edge>> VertexWeight = new Dictionary<Vertex, List<Edge>>();

        private Dictionary<Vertex, bool> visited = new Dictionary<Vertex, bool>();

        //представление графа в виде списка ребер
        private List<Edge> Edge = new List<Edge>();

        private Random R = new Random();

        //конструкторы:
        public Graph() : this(2, 1)
        {
        }

        //конструктор по умолчанию
        public Graph(string FileName)
        {
            StreamReader file = new StreamReader(FileName);
            string[] vertex;
            string[] StrFromFile = file.ReadToEnd().Split('\n');
            //Создать все вершины
            foreach (var s in StrFromFile)
            {
                vertex = s.Split(' ');
                //Первое число - ID вершины "откуда"
                Vertex v = new Vertex(int.Parse(vertex[0]));
                //Добавить вершину
                AddVertex(v);
            }

            foreach (var s in StrFromFile)
            {
                vertex = s.Split(' ');
                //Первое число - ID вершины "откуда"
                //Далее пары
                int from = int.Parse(vertex[0]);
                for (int k = 1; k < vertex.Length; k += 2)
                {
                    //Вершина "куда"
                    int to = int.Parse(vertex[k]);
                    //Вес
                    int weight = int.Parse(vertex[k + 1]);
                    Edge edge = new Edge(from, to, weight, VertexWeight);
                    AddEdge(edge);
                }
            }

            file.Close();
        }

        //конструктор-копия
        public Graph(Graph G)
        {
            VertexWeight = new Dictionary<Vertex, List<Edge>>(G.VertexWeight);
            Edge = new List<Edge>(G.Edge);
        }

        public Graph(int N, int M, bool oriented = false)//oriented == true - граф ориентированный, иначе - неориентированный
        {
            //заполнение списока смежности
            //заполнение вершин
            for (int i = 0; i < N; i++)
                AddVertex(new Vertex(i));
            //заполнение ребер
            //если число ребер превышает допустимое значение - установить максимум
            if (oriented)//для ориентированного
            {
                if (M > N * (N - 1))
                    M = N * (N - 1);
            }
            //для неориентированного
            else
            {
                if (M > N * (N - 1) / 2)
                    M = N * (N - 1) / 2;
            }
            for (int i = 0; i < M; i++)
            {
                //взять 2 несовпадающие вершины
                int r1 = R.Next(N);
                // Здесь следить, чтобы R.Next не оказался == предыдущему
                int r2 = R.Next(N);
                while (r1 == r2)
                    r2 = R.Next(N);

                KeyValuePair<Vertex, List<Edge>> K = VertexWeight.ElementAt(0);
                //Найти такую, чтобы вершина была r1
                foreach (var v in VertexWeight)
                    if (v.Key.ID == r1)
                        K = v;

                //найти ребро r2
                bool exists = false;
                //выполняется проверка наличия ребра
                foreach (var l in K.Value)
                    if (l.to.ID == r2)
                        exists = true;
                if (exists)
                {
                    i--;
                    continue;
                }
                //Добавить ребро "туда" и "обратно", если надо

                int Weight = R.Next(10) + 1;
                Edge edg = new Edge(r1, r2, Weight, VertexWeight);

                AddEdge(edg);

                if (!oriented) //Не ориенированный - значит, и обратное ребро тоже есть
                {
                    edg = new Edge(r2, r1, Weight, VertexWeight);
                    AddEdge(edg);
                }
            }
            //заполнить список ребер
        }

        public void ToEdgeList()//из списка смеж -> список ребер
        {
            Edge.Clear();
            foreach (var key in VertexWeight)
                foreach (var v in key.Value)
                    Edge.Add(v);
        }

        //красивый вывод
        public override string ToString()
        {
            string result = "";
            foreach (var key in VertexWeight)
            {
                string s = key.Key.ID.ToString() + " : ";
                foreach (var v in key.Value)
                    s += v.to.ID.ToString() + " ";
                result += s + Environment.NewLine;
            }
            return result;
        }

        //метод добавления вершины
        public void AddVertex(Vertex vertex)
        {
            VertexWeight.Add(vertex, new List<Edge>());
        }

        //метод добавления ребра в ориентированном графе
        public void AddEdge(Edge edge)
        {
            VertexWeight[edge.from].Add(edge);
        }

        //метод удаления ребра
        public void DeleteEdge(int from, int to)
        {
            //Найти исходящую вершину
            foreach (var v in VertexWeight)
            {
                if (v.Key.ID != from) continue;
                //Найти в ее списке ребер нужное
                foreach (var e in v.Value)
                    if (e.to.ID == to)
                    {
                        //Удалить
                        v.Value.Remove(e);
                        break;
                    }
            }
        }

        //метод удаления вершины
        public void DeleteVertex(int ID)
        {
            //удалить ребра, идущие в вершину
            foreach (var v in VertexWeight)
                foreach (var e in v.Value)
                    if (e.to.ID == ID)
                    {
                        v.Value.Remove(e);
                        break;
                    }

            //Найти и удалить саму вершину
            foreach (var v in VertexWeight)
                if (v.Key.ID == ID)
                {
                    VertexWeight.Remove(v.Key);
                    break;
                }
        }

        public void GetVertexFrom()//выводит предшественников для всех вершин из листа Edge
        {
            ToEdgeList();
            int i = 0;
            foreach (var item in Edge)
            {
                Console.WriteLine("({2}) для {0} предшественник {1}", item.to.ID, item.from.ID, i);
                i++;
            }
        }

        public int VertexDegree(Vertex v)
        {
            int Rate = 0;
            try
            {
                List<Edge> tmp = VertexWeight[v];
                int vertex = v.ID;
                Rate += tmp.Count;
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Вершины {0} нет в графе", v.ID);
            }
            return Rate;
        }

        public void OutVertexDegree()//находит степени для всех вершин
        {
            int num;

            ToEdgeList();

            foreach (var v in VertexWeight)
            {
                num = 0;

                foreach (var e in Edge)
                {
                    if (e.from.ID == v.Key.ID)
                    {
                        num++;
                    }
                }

                Console.WriteLine("Для вершины {0} степень исхода равна {1}", v.Key.ID, num);
            }
        }

        public int VertexRate(Vertex v)//находит степень вершины
        {
            int Rate = 0;
            //находим полустепень исхода данной вершины
            Rate += VertexDegree(v);
            //находим полустепень захода данной вершины
            int num = 0;
            foreach (var item in VertexWeight)
            {
                List<Edge> tmp = item.Value;
                foreach (var item1 in tmp)
                {
                    if (item1.to.ID == v.ID)
                    {
                        num++;
                        break;
                    }
                }
            }
            Rate += num;
            return Rate;
        }

        public void Task_Ia_3()//находит степени для всех вершин
        {
            int num;

            ToEdgeList();

            foreach (var v in VertexWeight)
            {
                num = 0;

                foreach (var e in Edge)
                {
                    if (e.to.ID == v.Key.ID)
                    {
                        num++;
                    }
                }

                Console.WriteLine("Для вершины {0} степень захода равна {1}", v.Key.ID, num);
            }
        }

        public bool IsPredcessor(Vertex vertex1, Vertex vertex2)//сделать
        {
            bool isPred = false;
            bool isContainVertex1 = VertexWeight.ContainsKey(vertex1);
            bool isContainVertex2 = VertexWeight.ContainsKey(vertex2);
            if (isContainVertex1 && isContainVertex2)
            {
                int V1 = vertex1.ID;
                int V2 = vertex2.ID;
                bool isVertex1 = false;
                bool IsVertex2 = false;
                List<Edge> tmp = new List<Edge>();
                foreach (var item in VertexWeight)
                {
                    tmp = item.Value;
                    foreach (var pred in tmp)
                    {
                        if (pred.to.ID == V1)
                        {
                            isVertex1 = true;
                        }
                        if (pred.to.ID == V2)
                        {
                            IsVertex2 = true;
                        }
                    }
                    if (isVertex1 == true && IsVertex2 == true)
                    {
                        Console.WriteLine("Вершина {0} является предшественником для вершин {1} и {2}", item.Key.ID, V1, V2);
                        break;
                    }
                    isVertex1 = false;
                    IsVertex2 = false;
                }
                if (isVertex1 == false && IsVertex2 == false)
                    Console.WriteLine("Вершины не имеют общего предшественника");
            }
            else
            {
                if (isContainVertex1 == false)
                    Console.WriteLine("Вершины {0} нет в графе", vertex1.ID);
                if (isContainVertex2 == false)
                    Console.WriteLine("Вершины {0} нет в графе", vertex2.ID);
            }

            return isPred;
        }

        public Dictionary<int, List<Vertex>> ComponentSearching()//находит все компоненты графа
        {
            Dictionary<int, List<Vertex>> result = new Dictionary<int, List<Vertex>>();
            List<Vertex> comp = new List<Vertex>();
            int pos = 1;
            void Dfs(Vertex v)
            {
                visited[v] = true;
                comp.Add(v);
                foreach (var to in VertexWeight[v])
                {
                    if (!visited[to.to])
                        Dfs(to.to);
                }
            }

            foreach (var init in VertexWeight.Keys)
            {
                visited.Add(init, false);
            }
            foreach (var item in VertexWeight)
            {
                if (!visited[item.Key])
                {
                    comp.Clear();
                    Dfs(item.Key);
                    List<Vertex> tmp = new List<Vertex>();
                    foreach (var copy in comp)
                    {
                        tmp.Add(copy);
                    }
                    result.Add(pos, tmp);
                    pos++;
                }
            }
            return result;
        }

        public void NewRemovedGraph()
        {
            Dictionary<Vertex, List<Edge>> NewGraph = new Dictionary<Vertex, List<Edge>>();
            foreach (var item in VertexWeight)
            {
                double deg = VertexRate(item.Key);
                if ((deg % 2 == 0) && (deg != 0))
                {
                    Console.WriteLine("Ключ {0} нужно удалить т.к его степень = {1}", item.Key.ID, deg);
                }
                else
                    NewGraph.Add(item.Key, item.Value);
            }
            VertexWeight = NewGraph;
        }

        public void SortEdgesByWeight()//сортирует лист ребер по возрастанию
        {
            ToEdgeList();
            Edge.Sort();
        }

        public void KruskalMethod()
        {
            Dictionary<Vertex, bool> isContained = new Dictionary<Vertex, bool>();//инициализируем словарь для проверки на принадлежность ребер разным множествам
            foreach (var newd in VertexWeight.Keys)
            {
                isContained.Add(newd, false);
            }
            int N = VertexWeight.Count;
            List<Edge> carcase = new List<Edge>();
            SortEdgesByWeight();

            foreach (var edge in Edge)
            {
                if (isContained[edge.from] == false && isContained[edge.to] == false)
                {
                    isContained[edge.from] = true;
                    isContained[edge.to] = true;
                    carcase.Add(edge);
                }
                else if ((isContained[edge.from] == true && isContained[edge.to] == false) || (isContained[edge.from] == false && isContained[edge.to] == true))
                {
                    if (isContained[edge.from] == false)
                        isContained[edge.from] = true;
                    if (isContained[edge.to] == false)
                        isContained[edge.to] = true;
                    carcase.Add(edge);
                }
            }
            int weight = 0;
            Console.WriteLine("Кол-во ребер в каркасе-{0}", carcase.Count);
            foreach (var show in carcase)
            {
                weight += show.weight;
                Console.WriteLine("Ребро каркаса {0}--{1} с весом {2}", show.from.ID, show.to.ID, show.weight);
            }
            Console.WriteLine("Сумарный вес каркаса: {0}", weight);
        }

        public int[,] AdjMatrixOfWeights(int s)
        {
            int n = VertexWeight.Count;
            int[,] Matrix = new int[n, n];
            foreach (var k in VertexWeight.Keys)
                foreach (var item in VertexWeight[k])
                {
                    int i = k.ID;
                    int j = item.to.ID;
                    Matrix[i, j] = item.weight;
                }
            if (s == 0)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (Matrix[i, j] == 0)
                        {
                            Matrix[i, j] = int.MaxValue;
                            //Console.Write(Matrix[i, j] + " ");
                        }
                        else
                        {
                            //Console.Write(Matrix[i,j]+" ");
                        }
                    }
                    //Console.WriteLine();
                }
            }
            if (s == 1)
            {
            }

            return Matrix;
        }

        public void Dijkstra(Vertex vertex, int N)//IVa - 8
        {
            int n = VertexWeight.Count;
            int st = vertex.ID;
            int[,] w = AdjMatrixOfWeights(0);
            bool[] visited = new bool[n];
            int[] D = new int[n];
            for (int i = 0; i < n; i++)
            {
                D[i] = w[st, i];
                visited[i] = false;
            }
            D[st] = 0;
            int index = 0, u = 0;
            for (int i = 0; i < n; i++)
            {
                int min = int.MaxValue;
                for (int j = 0; j < n; j++)
                {
                    if (!visited[j] && D[j] < min)
                    {
                        min = D[j];
                        index = j;
                    }
                }
                u = index;
                visited[u] = true;
                for (int j = 0; j < n; j++)
                {
                    if (!visited[j] && w[u, j] != int.MaxValue && D[u] != int.MaxValue && (D[u] + w[u, j] < D[j]))
                    {
                        D[j] = D[u] + w[u, j];
                    }
                }
            }
            Console.WriteLine("Стоимость пути из начальной вершины до остальных(Алгоритм Дейкстры)");
            for (int i = 0; i < n; i++)
            {
                if (D[i] != int.MaxValue)
                    Console.WriteLine(st + " -> " + i + " = " + D[i]);
                else
                    Console.WriteLine(st + " -> " + i + " = " + "маршрут недоступен");
            }
            for (int i = 0; i < n; i++)
            {
                if (D[i] < N && i != vertex.ID)
                { Console.WriteLine("Расстояние от заданной вершины {0} до {1} меньше N", vertex.ID, i); }
            }
        }

        public int min(int a, int b)
        {
            if (a < b)
                return a;
            else
                return b;
        }

        public int Bellman_Ford(Vertex vertex)
        {
            int n = VertexWeight.Count;
            int[] d = new int[n];
            for (int i = 0; i < n; i++)
            {
                d[i] = int.MaxValue;
            }
            d[vertex.ID] = 0;
            ToEdgeList();
            int m = Edge.Count();
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (d[Edge[j].from.ID] < int.MaxValue)
                    {
                        d[Edge[j].to.ID] = min(d[Edge[j].to.ID], d[Edge[j].from.ID] + Edge[j].weight);
                    }
                }
            }
            int maxweight = int.MinValue;
            for (int i = 0; i < n; i++)
            {
                if (d[i] > maxweight && d[i] != int.MaxValue)
                    maxweight = d[i];
            }
            Console.WriteLine("Эксцентриситет вершины {0} = {1}", vertex.ID, maxweight);
            return maxweight;
        }

        public int RadiusB_F()
        {
            int Radius = int.MaxValue;
            foreach (var item in VertexWeight.Keys)
            {
                int tmp = Bellman_Ford(item);
                if (tmp < Radius && tmp != 0)
                {
                    Radius = tmp;
                }
            }
            return Radius;
        }

        public void ClearB_F(Vertex vertex)
        {
            int min(int a, int b)
            {
                if (a < b)
                    return a;
                else
                    return b;
            }
            int n = VertexWeight.Count;
            int[] d = new int[n];
            for (int i = 0; i < n; i++)
            {
                d[i] = int.MaxValue;
            }
            d[vertex.ID] = 0;
            ToEdgeList();
            int m = Edge.Count();
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (d[Edge[j].from.ID] < int.MaxValue)
                    {
                        d[Edge[j].to.ID] = min(d[Edge[j].to.ID], d[Edge[j].from.ID] + Edge[j].weight);
                    }
                }
            }
            for (int i = 0; i < n; i++)
            {
                if (i != vertex.ID)
                {
                    if (d[i] != int.MaxValue)
                        Console.WriteLine(vertex.ID + " -> " + i + " = " + d[i]);
                    else
                        Console.WriteLine(vertex.ID + " -> " + i + " = " + "маршрут недоступен");
                }
            }
        }

        public void FU()
        {
            int[,] d = AdjMatrixOfWeights(1);
            int n = VertexWeight.Count;
            int k, i, j;
            for (k = 0; k < n; k++)
                for (i = 0; i < n; i++)
                    for (j = 0; j < n; j++)
                    {
                        if (d[i, k] > 0 && d[k, j] > 0)
                        {
                            if (d[i, j] == -1) d[i, j] = d[i, k] + d[k, j];
                            else d[i, j] = min(d[i, j], d[i, k] + d[k, j]);
                        }
                    }
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < n; j++)
                {
                    Console.Write(d[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        private bool Dfs(Vertex v)
        {
            visited[v] = true;
            bool check = false;
            foreach (var to in VertexWeight[v])
            {
                if (!visited[to.to])
                {
                    Dfs(to.to);
                }
                else
                {
                    check = true;
                }
            }
            return check;
        }

        public static void Task_IB_4(Graph g1, Graph g2)
        {
            Graph newGraph = new Graph();
            newGraph.VertexWeight.Clear();
            foreach (var vertex1 in g1.VertexWeight.Keys)
            {
                newGraph.AddVertex(vertex1);
            }
            foreach (var vertex2 in g2.VertexWeight.Keys)
            {
                if (!newGraph.VertexWeight.ContainsKey(vertex2))
                {
                    newGraph.AddVertex(vertex2);
                }
            }
            g1.ToEdgeList();
            g2.ToEdgeList();
            foreach (var edge1 in g1.Edge)
            {
                newGraph.AddEdge(edge1);
            }
            foreach (var edge2 in g2.Edge)
            {
                foreach (var edge in newGraph.Edge)
                {
                    if (edge2.from.ID != edge.from.ID && edge2.to.ID != edge.to.ID || edge2.from.ID == edge.from.ID && edge2.to.ID != edge.to.ID ||
                        edge2.from.ID != edge.from.ID && edge2.to.ID == edge.to.ID)
                    {
                        newGraph.AddEdge(edge2);
                    }
                }
            }
            Console.WriteLine(newGraph);
        }

        public bool Task_II_18()
        {
            visited.Clear();
            bool flag = false;
            foreach (var vertex in VertexWeight.Keys)
            {
                visited.Add(vertex, false);
            }
            foreach (var vertex in VertexWeight.Keys)
            {
                flag = Dfs(vertex);
                if (flag)
                {
                    return true;
                }
                else
                {
                    continue;
                }
            }
            return flag;
        }
    }
}