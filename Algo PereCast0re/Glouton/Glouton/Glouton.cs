using System;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

public class Glouton {
	private int depart;
	public int Depart {
		get {
			return depart;
		}
		set {
			depart = value;
		}
	}
    private List<int> cheminGlouton;
    public List<int> CheminGlouton
    {
        get { return cheminGlouton; }
        set { cheminGlouton = value; }
    }

    public Glouton(int depart) {
		this.depart = depart;
        this.cheminGlouton = new List<int>();
    }

    public void SetChemin(int[,] matrice)
    {
        this.cheminGlouton = new List<int>(matrice.GetLength(0));
    }

	public void AddChemin(int point) {
		this.cheminGlouton.Add(point);
    }

    public bool IsVisited(int point)
    {
        return this.cheminGlouton.Contains(point);
    }

    public void Parcour(int[,] matrice)
    {
        int depart = this.depart;
        int size = matrice.GetLength(0);
        int cout = 0;
    
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
                        prochain = i;
                        cout += matrice[depart, prochain];
                    }
                }
            }
            depart = prochain;
        }
        Console.WriteLine(cout);
    }
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
        
        Console.WriteLine(message);
    }

}
