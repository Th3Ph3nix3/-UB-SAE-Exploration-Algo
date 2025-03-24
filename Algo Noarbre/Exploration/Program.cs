using System;

internal class Program
{
    private static void Main()
    {
        Instance test = new Instance(10);
        int[,] matrice = Program.Lecture("points.txt");
    }
    public static int[,] Lecture(string nom)
    {
        string cheminFichier = "../../../../../Points/" + nom;

        string[] lignes = File.ReadAllLines(cheminFichier);

        int taille = int.Parse(lignes[0]);
        int[,] matrice = new int[taille, taille];

        for (int i = 1; i < taille+1; i++)
        {
            string[] elements = lignes[i].Split(' ');
            for (int j = 0; j < taille; j++)
            {
                matrice[i-1, j] = int.Parse(elements[j]);
            }
        }
        return matrice;
    }
}