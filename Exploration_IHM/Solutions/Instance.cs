using System;
using System.IO;

class Instance
{
    private int[,] mat_dis;
    private int[,] lis_point;
    private int taille;

    public Instance(int nb)
    {
        this.mat_dis = new int[nb,nb];
        this.lis_point = new int[nb,2];
        this.taille = nb;
        RemplirMatrice();
        ecriture();
    }

    private bool contient(int x, int y)
    {
        for(int i=0; i<this.taille; ++i)
        {
            if (this.lis_point[i,0] == x || this.lis_point[i,1] == y) return true;
        }
        return false;
    }

    private void RemplirMatrice()
    {
        Random rnd = new Random();
        for(int i=0; i<this.taille; ++i)
        {
            int x;
            int y;
            do
            {
                x = rnd.Next(1000);
                y = rnd.Next(1000);
            } while (contient(x, y));
            this.lis_point[i, 0] = x;
            this.lis_point[i, 1] = y;
        }

        for(int i=0; i<this.taille; ++i)
        {
            for(int j=0; j<this.taille; ++j)
            {
                if (i == j) this.mat_dis[i, j] = 0;
                else this.mat_dis[i, j] = distance(this.lis_point[i, 0], this.lis_point[j, 0], this.lis_point[i, 1], this.lis_point[j, 1]);
            }
        }
    }

    private int distance(int x1, int x2, int y1, int y2)
    {
        return (int)(Math.Sqrt((x2-x1)*(x2-x1)+(y2-y1)*(y2-y1)));
    }

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

    public int[,] Lecture()
    {
        string cheminFichier = "../../../../Points/points.txt";

        string[] lignes = File.ReadAllLines(cheminFichier);

        int taille = int.Parse(lignes[0]);
        int[,] matrice = new int[taille, taille];

        for (int i = 1; i < taille + 1; i++)
        {
            string[] elements = lignes[i].Split(' ');
            for (int j = 0; j < taille; j++)
            {
                matrice[i - 1, j] = int.Parse(elements[j]);
            }
        }
        return matrice;
    }
}
