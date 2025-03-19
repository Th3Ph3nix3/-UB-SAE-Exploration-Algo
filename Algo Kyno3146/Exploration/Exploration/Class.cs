using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exploration
{
    internal class Class
    {
        public class noeud
        {
            public int numero;

            public noeud(int numero) //constructeur
            {
                this.numero = numero;
            }
        }
        public class arretes
        {
            public noeud depart;
            public noeud arrivee;
            public int count;
            public int pheromonnes;
            public arretes(noeud depart, noeud arrivee, int cout, int pheromonnes) //constructeur
            {
                this.depart = depart;
                this.arrivee = arrivee;
                this.count = count;
                this.pheromonnes = 10;
            }

            public void evaporation()
            {
                this.pheromonnes = (int)(this.pheromonnes * 0.9);
            }

        }

        public class graphe
        {
            public List<noeud> noeuds;
            public List<arretes> arretes;
        }

        public class fourmi
        {
            public noeud position;
            public List<noeud> visite;
            
            public fourmi(noeud position)
            {
                this.position = position;
                List<noeud> visite = new List<noeud>();
                visite.Add(position);
            }

            public void deplacement(List<arretes> arretes)
            {
                List<arretes> arretesPossibles = new List<arretes>();
                foreach (arretes arrete in arretes)
                {
                    if (arrete.depart == this.position)
                    {
                        // verifier si l'arrete a pour autre extremite un noeud visité
                        bool visite = false;
                        foreach (noeud n in this.visite)
                        {
                            if (n == arrete.arrivee)
                            {
                                visite = true;
                            }
                        }
                        if (!visite)
                            arretesPossibles.Add(arrete);
                    }
                }
                if (arretesPossibles.Count > 0)
                {
                    Random rand = new Random();
                    double[] coefficients = new double[arretesPossibles.Count];
                    for (int i = 0; i < arretesPossibles.Count; i++)
                    {
                        coefficients[i] = coefficient(arretesPossibles[i], arretes);
                    }
                    // faire le choix en fonction du coefficient 

                    int index = rand.Next(arretesPossibles.Count);
                    this.position = arretesPossibles[index].arrivee;
                    visite.Add(this.position);
                }
            }

            public double coefficient(arretes a, List<arretes> arretes, int x, int y)
            {
                return a.pheromonnes**x / a.count **y;
            }

            public int totalcoefficients(List<arretes> arretes)
            {
                int total = 0;
                foreach (arretes a in arretes)
                {
                    total += a.pheromonnes;
                }
                return total;
            }
        }
    }
}
