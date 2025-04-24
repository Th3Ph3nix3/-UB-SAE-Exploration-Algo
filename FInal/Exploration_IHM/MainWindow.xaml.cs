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

    private string Matrice_string(int[,] matrice)
    {
        string matrice_en_text = "";

        for (int i = 0; i < matrice.GetLength(0); i++)
        {
            for (int j = 0; j < matrice.GetLength(1); j++)
            {
                matrice_en_text += matrice[i, j].ToString();
                if (j < matrice.GetLength(1) - 1)
                    matrice_en_text += ",";
            }
            if (i < matrice.GetLength(0) - 1)
                matrice_en_text += ";";
        }

        return matrice_en_text;
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

        #region Grasp Basique
        Stopwatch Grasp_B = new Stopwatch();
        Grasp_B grasp_B = new Grasp_B(matrice);

        Grasp_B.Start();
        grasp_B.Parcour(matrice);
        Grasp_B.Stop();

        Chemin_Grasp_B.Text = string.Join(" -> ", grasp_B.CheminGrasp);
        //Taille_Grasp_B.Text = grasp_B.Cout.ToString();
        Tps_Grasp_B.Text = Grasp_B.ElapsedMilliseconds.ToString() + " ms";
        #endregion

        #region Grasp Modifié
        Stopwatch Grasp_M = new Stopwatch();
        Grasp_M grasp_M = new Grasp_M(matrice);

        Grasp_M.Start();
        grasp_M.Parcour(matrice);
        Grasp_M.Stop();

        Chemin_Grasp_M.Text = string.Join(" -> ", grasp_M.CheminGrasp);
        //Taille_Grasp_M.Text = grasp_M.Cout.ToString();
        Tps_Grasp_M.Text = Grasp_M.ElapsedMilliseconds.ToString() + " ms";
        #endregion

        #region Fourmie

        #endregion

        #region Held-Karp
        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = "hel.exe";
        info.WorkingDirectory = @"../../../../C++";
        info.Arguments = "12 test.txt";
        info.UseShellExecute = true; // ou false si tu veux récupérer la sortie, sinon true c'est ok
        Process.Start(info);
        #endregion
    }

}
