using System;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

public class Glouton {

    #region Attributs

    /// <summary>
    /// Cout du chemin
    /// </summary>
    /// <author>Barthoux Sauze Thomas</author>
    private int cout;

    /// <summary> 
    /// Point de départ
    /// </summary>
    /// <author>Barthoux Sauze Thomas</author>
    private int depart;

    /// <summary>
    /// Chemin glouton
    /// </summary>
    /// <author>Barthoux Sauze Thomas</author>
    private List<int> cheminGlouton;

    #endregion

    #region Constructeurs

    /// <summary>
    /// Constructeur de la classe Glouton
    /// </summary>
    /// <author>Barthoux Sauze Thomas</author>
    public int Depart
    {
        get
        {
            return depart;
        }
        set
        {
            depart = value;
        }
    }

    /// <summary>
    /// Cout du chemin
    /// </summary>
    /// <author>Barthoux Sauze Thomas</author>
    public List<int> CheminGlouton
    {
        get { return cheminGlouton; }
        set { cheminGlouton = value; }
    }

    /// <summary>
    /// Cout du chemin
    /// </summary>
    /// <author>Barthoux Sauze Thomas</author>
    public int Cout
    {
        get { return cout; }
        set { cout = value; }
    }

    /// <summary>
    /// Constructeur de la classe Glouton
    /// </summary>
    /// <param name="depart"></param>
    /// <author>Barthoux Sauze Thomas</author>
    public Glouton(int depart)
    {
        this.depart = depart;
        this.cheminGlouton = new List<int>();
    }

    #endregion

    #region Methodes

    /// <summary>
    /// Initialise le chemin glouton
    /// </summary>
    /// <param name="matrice"></param>
    /// <author>Barthoux Sauze Thomas</author>
    public void SetChemin(int[,] matrice)
    {
        this.cheminGlouton = new List<int>(matrice.GetLength(0));
    }

    /// <summary>
    /// Ajoute un point au chemin glouton
    /// </summary>
    /// <param name="point"></param>
    /// <author>Barthoux Sauze Thomas</author>
    public void AddChemin(int point)
    {
        this.cheminGlouton.Add(point);
    }

    /// <summary>
    /// Vérifie si le point a déjà été visité
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    /// <author>Barthoux Sauze Thomas</author>
    public bool IsVisited(int point)
    {
        return this.cheminGlouton.Contains(point);
    }

    /// <summary>
    /// Parcourt la matrice de distance et construit le chemin glouton
    /// </summary>
    /// <param name="matrice"></param>
    public void Parcour(int[,] matrice)
    {
        int depart = this.depart;
        int size = matrice.GetLength(0);
        int cout = 0;
        int tmp = 0;

        while (cheminGlouton.Count < size)
        {
            AddChemin(depart);
            int min = int.MaxValue;
            int prochain = -1;

            for (int i = 0; i < size; i++)
            {
                int distance = matrice[depart, i];
                if (!IsVisited(i))
                {
                    if (distance > 0 && distance < min)
                    {
                        min = distance;
                        tmp = i;
                        
                    }
                }
            }
            this.cout += matrice[depart, tmp];
            prochain = tmp;
            depart = prochain;
        }
    }

    #endregion

    #region Print

    /// <summary>
    /// Affichage du chemin glouton
    /// </summary>
    /// <author>>Barthoux Sauze Thomas</author>
    public void ToString() {
        int taille = this.CheminGlouton.Count;
        string chemin = "";
		for (int i = 0; i < taille; i++)
		{
            chemin += this.cheminGlouton[i];
            if (i < taille - 1)
            {
                chemin += " -> ";
            }
        }
        string message = "Le chemin glouton est : " + chemin;
        
        Console.WriteLine("Cout du chemin : " + this.cout);
        Console.WriteLine(message);
    }

    #endregion

}
