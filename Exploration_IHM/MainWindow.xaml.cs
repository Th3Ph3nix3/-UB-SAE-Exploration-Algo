using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Exploration_IHM;

/// <summary>  
/// Interaction logic for MainWindow.xaml  
/// </summary>  
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ZoneTexte(object sender, TextChangedEventArgs e)
    {
        TexteIndication.Visibility = string.IsNullOrEmpty(TexBox_Nombre.Text)
        ? Visibility.Visible
        : Visibility.Collapsed;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        int nombre = int.Parse(TexBox_Nombre.Text);
        Instance instance = new Instance(nombre);
        int[,] matrice = instance.Lecture();

        /// Glouton complété a valider
        #region Glouton
        Stopwatch Glouton = new Stopwatch();
        Glouton.Start();

        Glouton glouton = new Glouton(0);
        glouton.Parcour(matrice);

        Glouton.Stop();
        Chemin.Text = "Chemin Glouton : " + string.Join(" -> ", glouton.CheminGlouton);
        Taille_Chemin.Text = "Taille : " + glouton.Cout;
        Temps.Text = "Glouton : " + Glouton.ElapsedMilliseconds + " ms";
        #endregion

        #region Grasp classique

        #endregion

        #region Grasp amélioré

        #endregion

        #region Fourmie

        #endregion

        #region Hazard

        #endregion

        #region Recherche local

        #endregion

        #region Held-Karp

        #endregion
    }

}
