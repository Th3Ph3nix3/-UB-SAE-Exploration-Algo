internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Bonjour !");
        Instance test = new Instance(10);
        int[,] matrice = test.Lecture();
        test.affiche();

        Console.WriteLine("\n\n");
        Console.WriteLine("Algo Glouton");
        
        Glouton glouton = new Glouton(1);
        glouton.Parcour(matrice);
        glouton.ToString();

        test.ecriture(glouton.CheminGlouton, 10, "Glouton");

    }
}