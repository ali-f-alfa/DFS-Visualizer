using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace finall
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Node> ans = new List<Node>();
        public List<int>[] Graph;
        public int nodes;
        public int edges;
        public static Ellipse[] MyNodes;
        public Label[] MyLabels;

        public MainWindow()
        {
            InitializeComponent();
            MyNodes = new Ellipse[10] { N1, N2, N3, N4, N5, N6, N7, N8, N9, N10 };
            MyLabels = new Label[10] { L1, L2, L3, L4, L5, L6, L7, L8, L9, L10 };
            ResetUI();

        }
        public static void nonRecursivePostOrder(Node root)
        {
            var toVisit = new Stack<Node>();
            var visitedAncestors = new Stack<Node>();
            toVisit.Push(root);
            while (toVisit.Count > 0)
            {
                var node = toVisit.Peek();
                if (node.Children.Count > 0)
                {
                    if (visitedAncestors.PeekOrDefault() != node)
                    {
                        visitedAncestors.Push(node);
                        toVisit.PushReverse(node.Children);
                        continue;
                    }
                    visitedAncestors.Pop();
                }

                Anim(node);
                



                toVisit.Pop();
                if (!ans.Contains(node))
                {
                    ans.Add(node);
                }

            }
        }

        private static void Anim(Node node)
        {
            MyNodes[node.Id].Fill = new SolidColorBrush(Colors.OrangeRed);
            //Task.Delay(1000).Wait();
            MessageBox.Show(node.Id.ToString());
            //Thread.Sleep(1000);
            MyNodes[node.Id].Fill = new SolidColorBrush(Colors.SlateGray);

        }

        public Node makeTree(int n, int e)
        {
            List<Node> graph = new List<Node>();
            for (int i = 0; i < n; i++)
            {
                graph.Add(new Node(i));
            }
            for (int i = 0; i < e; i++)
            {
                string[] toks = InputGraph.GetLineText(i).Replace("\r\n", "").Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int n1 = int.Parse(toks[0]);
                int n2 = int.Parse(toks[1]);
                graph[n1].addEdge(graph[n2]);
            }

            return graph[0];
        }
        //###############################################################################

        public static Shape DrawLinkArrow(Point p1, Point p2)
        {
            p1 = new Point(p1.X + 17.5, p1.Y + 17.5);
            p2 = new Point(p2.X + 17.5, p2.Y + 17.5);

            GeometryGroup lineGroup = new GeometryGroup();
            double theta = Math.Atan2((p2.Y - p1.Y), (p2.X - p1.X)) * 180 / Math.PI;

            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            Point p = new Point(p1.X + ((p2.X - p1.X) / 1.6), p1.Y + ((p2.Y - p1.Y) / 1.6));
            pathFigure.StartPoint = p;

            Point lpoint = new Point(p.X + 6, p.Y + 15);
            Point rpoint = new Point(p.X - 6, p.Y + 15);
            LineSegment seg1 = new LineSegment();
            seg1.Point = lpoint;
            pathFigure.Segments.Add(seg1);

            LineSegment seg2 = new LineSegment();
            seg2.Point = rpoint;
            pathFigure.Segments.Add(seg2);

            LineSegment seg3 = new LineSegment();
            seg3.Point = p;
            pathFigure.Segments.Add(seg3);
            pathGeometry.Figures.Add(pathFigure);

            RotateTransform transform = new RotateTransform();
            transform.Angle = theta + 90;
            transform.CenterX = p.X;
            transform.CenterY = p.Y;
            pathGeometry.Transform = transform;
            lineGroup.Children.Add(pathGeometry);

            LineGeometry connectorGeometry = new LineGeometry();
            connectorGeometry.StartPoint = p1;
            connectorGeometry.EndPoint = p2;
            lineGroup.Children.Add(connectorGeometry);
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
            path.Data = lineGroup;
            path.StrokeThickness = 3;
            path.Stroke = path.Fill = Brushes.Black;

            return path;
        }

        public static Shape DrawArrowBetweenNodes(Ellipse e1, Ellipse e2)
        {
            return DrawLinkArrow(new Point(e1.Margin.Left, e1.Margin.Top), new Point(e2.Margin.Left, e2.Margin.Top));
        }


        public void Button_Click(object sender, RoutedEventArgs e)
        {
            
            ResetUI();
            CreatGraph();
            for (int i = 0; i < nodes; i++)
            {
                MyNodes[i].Visibility = Visibility.Visible;
                MyLabels[i].Visibility = Visibility.Visible;
            }
            for (int i = 0; i < nodes; i++)
            {
                foreach (int j in Graph[i])
                {
                    canvas.Children.Add(DrawArrowBetweenNodes(MyNodes[i], MyNodes[j]));
                }
            }

            var root = makeTree(nodes, edges);
            nonRecursivePostOrder(root);

            ans.Reverse();
            Result.Content = "Topological Sort order : \n";
            foreach(var node in ans)
            {
                Result.Content += node.Id.ToString() +" ";
            }
            
        }

        public void ResetUI()
        {
            foreach (var label in MyLabels)
                label.Visibility = Visibility.Hidden;
            foreach (var node in MyNodes)
                node.Visibility = Visibility.Hidden;
        }

        public void CreatGraph()
        {
            nodes = int.Parse(Node.Text);
            edges = int.Parse(Edge.Text);

            Graph = new List<int>[nodes];
            //create empty lists inside array
            for (int i = 0; i < nodes; i++)
                Graph[i] = new List<int>();

            //create our adjacency lists Graph
            for (int i = 0; i < edges; i++)
            {
                string[] toks = InputGraph.GetLineText(i).Replace("\r\n", "").Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int n1 = int.Parse(toks[0]);
                int n2 = int.Parse(toks[1]);
                Graph[n1].Add(n2);
            }
        }

        private void InputGraph_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        //public void CreateEllipse(double width, double height, double desiredCenterX, double desiredCenterY)
        //{
        //    Ellipse ellipse = new Ellipse { Width = width, Height = height };
        //    double left = desiredCenterX - (420 / 2) - (width / 2);
        //    double top = desiredCenterY - (793.6 / 2) - (height / 2);

        //    ellipse.Margin = new Thickness(0, -350, 0, 0);
        //    //ellipse.SetValue(Canvas.LeftProperty, left);
        //    //ellipse.SetValue(Canvas.TopProperty, top);


        //    ellipse.Fill = Brushes.Black;
        //    canvas.Children.Add(ellipse);
        //}


        //public static void DFS(int start, int nodes, int edges, List<int>[] graph)
        //{
        //    bool[] visited = new bool[nodes];

        //    //For DFS use stack
        //    Stack<int> stack = new Stack<int>();
        //    visited[start] = true;
        //    stack.Push(start);

        //    while (stack.Count != 0)
        //    {
        //        start = stack.Pop();
        //        Console.Write(start + " ");
        //        foreach (int i in graph[start])
        //        {
        //            if (!visited[i])
        //            {
        //                visited[i] = true;
        //                stack.Push(i);
        //            }
        //            else
        //            {

        //            }
        //        }
        //    }

        //}


    }
}
