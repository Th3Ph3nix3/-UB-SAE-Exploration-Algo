using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Fourmi;

namespace Exploration_IHM
{
    /// <summary>
    /// Interaction logic for Graphe.xaml
    /// This window visualizes a graph and highlights several paths (from different algorithms) with distinct colors.
    /// </summary>
    public partial class Graphe : Window
    {
        private int[,] matrix; // The adjacency matrix representing the graph

        /// <summary>
        /// Constructor for the Graphe window.
        /// </summary>
        /// <param name="matrice">Adjacency matrix of the graph</param>
        /// <param name="glouton">Path from the greedy algorithm</param>
        /// <param name="grasp_bas">Path from basic GRASP</param>
        /// <param name="grasp_mod">Path from modified GRASP</param>
        /// <param name="VNS_only">Path from VNS only</param>
        /// <param name="VNS_grasp">Path from VNS initialized with GRASP</param>
        /// <param name="fourmis">Path from Ant Colony algorithm</param>
        /// <param name="HK_string">Path from Held-Karp algorithm as a string</param>
        public Graphe(
            int[,] matrice,
            List<int> glouton,
            List<int> grasp_bas,
            List<int> grasp_mod,
            List<int> VNS_only,
            List<int> VNS_grasp,
            List<int> fourmis,
            string HK_string)
        {
            InitializeComponent();
            matrix = matrice;
            int n = matrix.GetLength(0);

            // Parameters for node placement (circle layout)
            double centerX = 300, centerY = 300, radius = 200;
            Point[] positions = new Point[n];

            // Parse the Held-Karp path string into a list of integers
            List<int> HK = HK_string
                .Split(new[] { ' ', '-', '>' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.Parse(s))
                .ToList();

            // Draw nodes (vertices) in a circle
            for (int i = 0; i < n; i++)
            {
                double angle = 2 * Math.PI * i / n;
                double x = centerX + radius * Math.Cos(angle);
                double y = centerY + radius * Math.Sin(angle);
                positions[i] = new Point(x, y);

                // Draw the node as a blue circle
                Ellipse node = new Ellipse { Width = 30, Height = 30, Fill = Brushes.Blue };
                Canvas.SetLeft(node, x - 15);
                Canvas.SetTop(node, y - 15);
                myCanvas.Children.Add(node);

                // Draw the node label (vertex number)
                TextBlock label = new TextBlock
                {
                    Text = i.ToString(),
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    Width = 30,
                    Height = 30,
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Canvas.SetLeft(label, x - 15);
                Canvas.SetTop(label, y - 15 + 4); // Adjust for vertical centering
                myCanvas.Children.Add(label);
            }

            // List of all paths to display, each with a distinct semi-transparent color
            var paths = new List<(List<int> path, Color color)>
            {
                (glouton,   Color.FromArgb(125, 0, 0, 0)),         // Greedy: dark gray
                (grasp_bas, Color.FromArgb(125, 255, 0, 0)),       // GRASP basic: red
                (grasp_mod, Color.FromArgb(125, 0, 255, 0)),       // GRASP modified: green
                (VNS_only,  Color.FromArgb(125, 0, 0, 255)),       // VNS only: blue
                (VNS_grasp, Color.FromArgb(125, 255, 255, 0)),     // VNS+GRASP: yellow
                (fourmis,   Color.FromArgb(125, 255, 0, 255)),     // Ant Colony: magenta
                (HK,        Color.FromArgb(125, 255, 165, 0))      // Held-Karp: orange
            };

            // Draw only the edges that appear in at least one path, allowing color superposition
            foreach (var (path, color) in paths)
            {
                if (path != null && path.Count > 1)
                {
                    for (int k = 0; k < path.Count - 1; k++)
                    {
                        int i = path[k];
                        int j = path[k + 1];

                        // Optional: check that the edge exists in the matrix
                        if (matrix[i, j] != 0)
                        {
                            Line edge = new Line
                            {
                                X1 = positions[i].X,
                                Y1 = positions[i].Y,
                                X2 = positions[j].X,
                                Y2 = positions[j].Y,
                                Stroke = new SolidColorBrush(color),
                                StrokeThickness = 4 // Thicker for better visibility and overlap
                            };
                            myCanvas.Children.Add(edge);
                        }
                    }
                }
            }
        }
    }
}
