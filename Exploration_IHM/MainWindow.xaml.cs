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

        #region Glouton
        Stopwatch Glouton = new Stopwatch();
        Glouton glouton = new Glouton(0);

        Glouton.Start();
        glouton.Parcour(matrice);
        Glouton.Stop();

        Chemin_Glouton.Text = string.Join(" -> ", glouton.CheminGlouton);
        Taille_Glouton.Text = glouton.Cout.ToString();
        Tps_Glouton.Text = Glouton.ElapsedMilliseconds.ToString() + " ms";
        #endregion

        #region VNS
        VNS vns = new VNS(matrice);
        #region sans glouton
        Stopwatch Vns = new Stopwatch();
        
        Vns.Start();
        vns.TrouverCycleVNS(0);
        Glouton.Stop();

        Chemin_VNS.Text = string.Join(" -> ", vns.Chemin);
        Taille_VNS.Text = vns.ObtenirCout(vns.Chemin).ToString();
        Tps_VNS.Text = Vns.ElapsedMilliseconds.ToString() + " ms";
        #endregion

        #region avec glouton
        Stopwatch Vns_glouton = new Stopwatch();

        Vns_glouton.Start();
        vns.TrouverCycleVNS(glouton.CheminGlouton);
        Vns_glouton.Stop();

        Chemin_VNS_Glouton.Text = string.Join(" -> ", vns.Chemin);
        Taille_VNS_Glouton.Text = vns.ObtenirCout(vns.Chemin).ToString();
        Tps_VNS_Glouton.Text = Vns_glouton.ElapsedMilliseconds.ToString() + " ms";
        #endregion
        #endregion

        #region Grasp classique

        #endregion

        #region Grasp amélioré

        #endregion

        #region Fourmie

        #endregion

        #region Held-Karp

        #endregion
    }

}
