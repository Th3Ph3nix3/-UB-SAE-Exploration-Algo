using GRASP; // Si l'espace de noms est 'GRASP'

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Bonjour voici l'algo du GRASP !");

        Instance test = new Instance(10);
        int[,] matrice = test.Lecture();
        test.affiche();

        Console.WriteLine("\n\n");
        Console.WriteLine("Algo GRASP");

        // Instanciation de l'objet GRASP
        // Ajout du namespace car evite un conflit de nom
        GRASP.GRASP algo = new GRASP.GRASP(matrice);
        algo.Parcour(matrice);
        algo.ToString();

    }
}
