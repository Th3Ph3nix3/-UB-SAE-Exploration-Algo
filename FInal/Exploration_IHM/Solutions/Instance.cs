using System;
using System.IO;

/// <summary>
/// Classe représentant une instance de points et leur matrice de distances.
/// </summary>
public class Instance
{
    #region Attributs
    private int[,] mat_dis;   // Matrice des distances entre les points
    private int[,] lis_point; // Liste des coordonnées (x, y) des points
    private int taille;       // Nombre de points
    #endregion

    #region Propriétés
    // Aucune propriété ajoutée, mais vous pouvez en ajouter si nécessaire.
    #endregion

    #region Constructeur
    /// <summary>
    /// Constructeur de l'instance, initialise les points et la matrice de distances.
    /// </summary>
    /// <param name="nb">Nombre de points à générer.</param>
    public Instance(int nb)
    {
        this.mat_dis = new int[nb, nb];
        this.lis_point = new int[nb, 2];
        this.taille = nb;
        RemplirMatrice();
        ecriture();
    }
    #endregion

    #region Méthodes publiques
    /// <summary>
    /// Affiche dans la console la liste des points et la matrice des distances.
    /// </summary>
    public void affiche()
    {
        for (int i = 0; i < this.taille; ++i)
        {
            Console.Write(this.lis_point[i, 0].ToString() + "-" + this.lis_point[i, 1].ToString() + " | ");
        }
        Console.WriteLine();

        for (int i = 0; i < this.taille; ++i)
        {
            for (int j = 0; j < this.taille; ++j)
            {
                Console.Write(this.mat_dis[i, j].ToString() + " | ");
            }
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Lit une matrice de distances à partir d'un fichier texte et la retourne.
    /// </summary>
    /// <return>Retourne la matrice de distances lue depuis le fichier.</return>
    public int[,] Lecture()
    {
        string cheminFichier = "../../../../Points/points.txt";

        string[] lignes = File.ReadAllLines(cheminFichier);

        int taille = int.Parse(lignes[0]);
        int[,] matrice = new int[taille, taille];

        for (int i = 1; i < taille + 1; i++)
        {
            string[] elements = lignes[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < taille; j++)
            {
                matrice[i - 1, j] = int.Parse(elements[j]);
            }
        }
        return matrice;
    }

    /// <summary>
    /// Écrit un chemin de solution dans un fichier texte, associé à un algorithme donné.
    /// </summary>
    /// <param name="Chemin">Liste des indices du chemin parcouru.</param>
    /// <param name="taille_chemin">Longueur totale du chemin.</param>
    /// <param name="nom_algo">Nom de l'algorithme utilisé pour identifier le fichier de sortie.</param>
    public void ecriture(List<int> Chemin, int taille_chemin, string nom_algo)
    {
        string cheminFichier = "../../../../Solutions/" + nom_algo + ".txt";

        using (StreamWriter writer = new StreamWriter(cheminFichier))
        {
            foreach (int chemin in Chemin)
            {
                writer.Write((chemin + 1).ToString() + " ");
            }
            writer.WriteLine();
            writer.Write(taille_chemin.ToString());
        }
    }
    #endregion

    #region Méthodes privées
    /// <summary>
    /// Vérifie si un point (x, y) existe déjà dans la liste des points.
    /// </summary>
    /// <param name="x">Coordonnée x du point.</param>
    /// <param name="y">Coordonnée y du point.</param>
    /// <return>Retourne true si le point existe déjà, sinon false.</return>
    private bool contient(int x, int y)
    {
        for (int i = 0; i < this.taille; ++i)
        {
            if (this.lis_point[i, 0] == x || this.lis_point[i, 1] == y) return true;
        }
        return false;
    }

    /// <summary>
    /// Remplit la liste des points avec des coordonnées uniques, puis construit la matrice des distances.
    /// </summary>
    private void RemplirMatrice()
    {
        Random rnd = new Random();
        for (int i = 0; i < this.taille; ++i)
        {
            int x, y;
            do
            {
                x = rnd.Next(1000);
                y = rnd.Next(1000);
            } while (contient(x, y)); // S'assurer que le point est unique
            this.lis_point[i, 0] = x;
            this.lis_point[i, 1] = y;
        }

        for (int i = 0; i < this.taille; ++i)
        {
            for (int j = 0; j < this.taille; ++j)
            {
                if (i == j)
                    this.mat_dis[i, j] = 0;
                else
                    this.mat_dis[i, j] = distance(this.lis_point[i, 0], this.lis_point[j, 0], this.lis_point[i, 1], this.lis_point[j, 1]);
            }
        }
    }

    /// <summary>
    /// Calcule la distance euclidienne entière entre deux points.
    /// </summary>
    /// <param name="x1">Coordonnée x du premier point.</param>
    /// <param name="x2">Coordonnée x du deuxième point.</param>
    /// <param name="y1">Coordonnée y du premier point.</param>
    /// <param name="y2">Coordonnée y du deuxième point.</param>
    /// <return>La distance entière calculée entre les deux points.</return>
    private int distance(int x1, int x2, int y1, int y2)
    {
        return (int)(Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)));
    }

    /// <summary>
    /// Écrit la matrice des distances dans un fichier texte.
    /// </summary>
    private void ecriture()
    {
        string cheminFichier = "../../../../Points/points.txt";

        using (StreamWriter writer = new StreamWriter(cheminFichier))
        {
            writer.WriteLine(this.taille.ToString());

            for (int i = 0; i < this.taille; ++i)
            {
                for (int j = 0; j < this.taille; ++j)
                {
                    writer.Write(this.mat_dis[i, j].ToString() + " ");
                }
                writer.WriteLine();
            }
        }
    }
    #endregion
}
