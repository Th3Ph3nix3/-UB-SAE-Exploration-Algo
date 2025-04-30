using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Fourmi;

namespace Exploration_IHM
{
    /// <summary>
    /// Logique d'interaction pour Graphe.xaml
    /// </summary>
    public partial class Graphe : Window
    {
        private int[,] matrix;

        public Graphe(int[,] matrice, List<int> glouton, List<int> grasp_bas, List<int> grasp_mod,
                      List<int> VNS_only, List<int> VNS_grasp, List<int> fourmis, string HK_string)
        {
            InitializeComponent();
            matrix = matrice;
            int n = matrix.GetLength(0);
            double centerX = 300, centerY = 300, radius = 200;
            Point[] positions = new Point[n];

            // Convertir la chaîne HK_string en liste d'entiers
            List<int> HK = HK_string.Split(new[] { ' ', '-', '>' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(s => int.Parse(s))
                                    .ToList();

            // Création des sommets (cercles) placés en cercle
            for (int i = 0; i < n; i++)
            {
                double angle = 2 * Math.PI * i / n;
                double x = centerX + radius * Math.Cos(angle);
                double y = centerY + radius * Math.Sin(angle);
                positions[i] = new Point(x, y);

                Ellipse node = new Ellipse { Width = 30, Height = 30, Fill = Brushes.Blue };
                Canvas.SetLeft(node, x - 15);
                Canvas.SetTop(node, y - 15);
                myCanvas.Children.Add(node);

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
                Canvas.SetTop(label, y - 15 + 4);
                myCanvas.Children.Add(label);
            }

            // Liste des chemins avec leur couleur semi-transparente
            var chemins = new List<(List<int> chemin, Color couleur)>
        {
            (glouton, Color.FromArgb(125, 0, 0, 0)),           // Glouton : gris foncé
            (grasp_bas, Color.FromArgb(125, 255, 0, 0)),       // GRASP basique : rouge
            (grasp_mod, Color.FromArgb(125, 0, 255, 0)),       // GRASP modifié : vert
            (VNS_only, Color.FromArgb(125, 0, 0, 255)),        // VNS seul : bleu
            (VNS_grasp, Color.FromArgb(125, 255, 255, 0)),     // VNS+GRASP : jaune
            (fourmis, Color.FromArgb(125, 255, 0, 255)),       // Fourmis : magenta
            (HK, Color.FromArgb(125, 255, 165, 0))             // Held-Karp : orange
        };

            // Dessiner uniquement les arêtes présentes dans les parcours, avec superposition des couleurs
            foreach (var (chemin, couleur) in chemins)
            {
                if (chemin != null && chemin.Count > 1)
                {
                    for (int k = 0; k < chemin.Count - 1; k++)
                    {
                        int i = chemin[k];
                        int j = chemin[k + 1];

                        if (matrix[i, j] != 0)
                        {
                            Line edge = new Line
                            {
                                X1 = positions[i].X,
                                Y1 = positions[i].Y,
                                X2 = positions[j].X,
                                Y2 = positions[j].Y,
                                Stroke = new SolidColorBrush(couleur),
                                StrokeThickness = 4
                            };
                            myCanvas.Children.Add(edge);
                        }
                    }
                }
            }
        }
    }

}

