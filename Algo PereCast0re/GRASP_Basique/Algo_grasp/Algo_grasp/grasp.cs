using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo_grasp
{
    internal class grasp
    {
        #region attributes
        private int depart;
        private List<int> cheminGrasp;
        #endregion

        #region constructor
        /// <summary>
        /// Propriété CheminGrasp
        /// </summary>
        public List<int> CheminGrasp
        {
            get { return cheminGrasp; }
            set { cheminGrasp = value; }
        }

        /// <summary>
        /// Propriété Depart
        /// </summary>
        public int Depart
        {
            get { return depart; }
            set { depart = value; }
        }

        /// <summary>
        /// Constructeur de la classe GRASP
        /// </summary>
        /// <param name="depart"></param>
        public grasp(int[,] matrice)
        {
            this.depart = 0;
            this.cheminGrasp = new List<int>(matrice.GetLength(0));
        }
        #endregion


        #region methods
        /// <summary>
        /// Ajoute un point au chemin Grasp
        /// </summary>
        /// <param name="point"></param>
        public void AddChemin(int point)
        {
            this.CheminGrasp.Add(point);
        }

        /// <summary>
        /// Compter le nombre total de points dans la matrice
        /// </summary>
        /// <param name="matrice"></param>
        /// <returns></returns>
        public int Highest(int[,] matrice,int point)
        {
            int maxi = int.MinValue;
            for (int i = 0; i < matrice.GetLength(0); i++)
            {
                if (maxi < matrice[point, i])
                {
                    maxi = matrice[point, i];
                }
            }
            return maxi;
        }

        /// <summary>
        /// Sélectionne un point aléatoire dans la matrice
        /// </summary>
        /// <param name="matrice"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public int selectPoint(int[,] matrice, int point)
        {
            int calcul = 0;
            int maxi = int.MaxValue;

            //Création d'une liste de point
            List<(int, int)> liste = new List<(int, int)>();
            for (int i = 0; i < matrice.GetLength(0); i++)
            {
                liste.Add((i, matrice[point, i]));
            }

            Random random = new Random();
            int rdm = random.Next(1, Highest(matrice, point));
            int rdm2;

            // On cherche un point qui - le random et inférieur a 0 et on renvoie cette valeur 
            for (int j = 0; j < liste.Count; j++)
            {

                calcul = liste[j].Item2 - rdm;
                if (calcul < 0 && !IsVisited(liste[j].Item1))
                {
                    point = liste[j].Item1;
                    break;
                }
                else
                {
                    if (j.Equals(liste.Count - 1))
                    {
                        rdm2 = random.Next(0, liste.Count);
                        point = liste[rdm2].Item1;
                    }
                }
            }
            return point;
        }

        /// <summary>
        /// Vérifie si le point a déjà été visité
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsVisited(int point)
        {
            return this.cheminGrasp.Contains(point);
        }

        /// <summary>
        /// Parcours la matrice et ajoute les points au chemin Grasp
        /// </summary>
        /// <param name="matrice"></param>
        public void Parcour(int[,] matrice)
        {
            int point = this.depart;
            int cout = 0;

            AddChemin(point);

            while (cheminGrasp.Count < matrice.GetLength(0))
            {
                int suivant = selectPoint(matrice, point);
                cheminGrasp.Add(suivant);

                cout += matrice[point, suivant];
                point = suivant;
            }

            Console.WriteLine("Le cout du chemin est de : " + cout);

        }

        /// <summary>
        /// Affiche le chemin grasp
        /// </summary>
        public void ToString()
        {
            int taille = this.CheminGrasp.Count;
            string chemin = "";
            for (int i = 0; i < taille; i++)
            {
                chemin += this.CheminGrasp[i];
                if (i < taille - 1)
                {
                    chemin += " -> ";
                }
            }
            string message = "Le chemin glouton est : " + chemin;

            Console.WriteLine(message);
        }


        #endregion

    }
}
