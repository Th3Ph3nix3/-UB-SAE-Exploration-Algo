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
using System.IO;
using static Exploration.Class;
using Fourmi;
using static Fourmi.Fourmis;

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
        int nombre;
        bool reussit = int.TryParse(TexBox_Nombre.Text, out nombre);
        if (!reussit)
        {
            Text_erreur.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            Text_erreur.Text = "La valeur saissi doit être un nombre entier non une chaine de caractère";
            return;
        }
        else if (nombre <= 0)
        {
            Text_erreur.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            Text_erreur.Text = "Veuillez entrer un nombre positif";
            return;
        }
        Text_erreur.Text = "";

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

        #region Grasp Basique
        Stopwatch Grasp_B = new Stopwatch();
        Grasp_B grasp_B = new Grasp_B(matrice);

        Grasp_B.Start();
        grasp_B.Parcour(matrice);
        Grasp_B.Stop();

        Chemin_Grasp_B.Text = string.Join(" -> ", grasp_B.CheminGrasp);
        Taille_Grasp_B.Text = grasp_B.Cout.ToString();
        Tps_Grasp_B.Text = Grasp_B.ElapsedMilliseconds.ToString() + " ms";
        #endregion

        #region Grasp Modifié
        Stopwatch Grasp_M = new Stopwatch();
        Grasp_M grasp_M = new Grasp_M(matrice);

        Grasp_M.Start();
        grasp_M.Parcour(matrice);
        Grasp_M.Stop();

        Chemin_Grasp_M.Text = string.Join(" -> ", grasp_M.CheminGrasp);
        Taille_Grasp_M.Text = grasp_M.Cout.ToString();
        Tps_Grasp_M.Text = Grasp_M.ElapsedMilliseconds.ToString() + " ms";
        #endregion

        #region VNS sans grasp
        VNS vns = new VNS(matrice);
        Stopwatch Vns = new Stopwatch();

        Vns.Start();
        vns.TrouverCycleVNS(0);
        Vns.Stop();

        Chemin_VNS.Text = string.Join(" -> ", vns.Chemin);
        Taille_VNS.Text = vns.ObtenirCout(vns.Chemin).ToString();
        Tps_VNS.Text = Vns.ElapsedMilliseconds.ToString() + " ms";
        #endregion

        #region VNS avec grasp
        VNS vns_glouton = new VNS(matrice);
        Stopwatch Vns_glouton = new Stopwatch();

        Vns_glouton.Start();
        vns_glouton.TrouverCycleVNS(grasp_B.CheminGrasp);
        Vns_glouton.Stop();

        Chemin_VNS_Glouton.Text = string.Join(" -> ", vns_glouton.Chemin);
        Taille_VNS_Glouton.Text = vns_glouton.ObtenirCout(vns_glouton.Chemin).ToString();
        Tps_VNS_Glouton.Text = Vns_glouton.ElapsedMilliseconds.ToString() + " ms";
        #endregion

        #region Fourmis
        Stopwatch Fourmis = new Stopwatch();
        Fourmis fourmisInstance = new Fourmis();

        List<arretes> CheminFourmis = new List<arretes>();
        Fourmis.Start();
        CheminFourmis = fourmisInstance.TrouverChemin(matrice);
        Fourmis.Stop();
        List<int> chemin_foumis = fourmisInstance.Transformer_Chemin(CheminFourmis);
        int taille = fourmisInstance.Calculer_taille(CheminFourmis);

        Chemin_Fourmis.Text = string.Join(" -> ", chemin_foumis);
        Taille_Fourmis.Text = taille.ToString();
        Tps_Fourmis.Text = Fourmis.ElapsedMilliseconds.ToString() + " ms";
        #endregion

        #region Held-Karp
        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = "Held-Karp.exe";
        info.WorkingDirectory = @"../../../../C++";
        info.Arguments = $"\"{Matrice_string(matrice)}\"";
        info.UseShellExecute = true;

        Process processus = Process.Start(info);

        // Attendre que le processus se termine
        processus.WaitForExit();

        string CheminOutput = "../../../../C++/output.txt";
        string CheminTps = "../../../../C++/temps_execution.txt";

        string chemin = "";
        string cout = "";
        string tps = "";

        string[] Lignes_Output = File.ReadAllLines(CheminOutput);
        string[] Lignes_Tps = File.ReadAllLines(CheminTps);
        chemin = Lignes_Output[0];
        cout = Lignes_Output[1];
        tps = Lignes_Tps[0];
        chemin = chemin.Replace(" ", " -> ");

        Chemin_H_K.Text = chemin;
        Taille_H_K.Text = cout;
        Tps_H_K.Text = tps + " ms";
        #endregion
    }

}
