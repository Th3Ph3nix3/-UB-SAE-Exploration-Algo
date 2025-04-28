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
            public arretes(noeud depart, noeud arrivee, int count) //constructeur
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
            public List<arretes> chemin;

            public fourmi(noeud position)
            {
                this.position = position;
                this.visite = new List<noeud>();
                visite.Add(position);
                this.chemin = new List<arretes>();
            }

            public arretes deplacement(List<arretes> arretes)
            {
                List<arretes> arretesPossibles = new List<arretes>();
                double alpha = 1.0;
                double beta = 1.0;
                Random rand = new Random();

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
                    List<double> poids = new List<double>();
                    double total = 0;

                    // calcul des poids
                    foreach (arretes a in arretesPossibles)
                    {
                        double tau = a.pheromonnes;
                        double eta = 1.0 / a.count;
                        double valeur = Math.Pow(tau, alpha) * Math.Pow(eta, beta);
                        poids.Add(valeur);
                        total += valeur;
                    }
                    // tirage d'un nombre aleatoire entre 0 et total
                    double r = rand.NextDouble() * total;

                    // selection de l'arrete
                    double cumul = 0;
                    for (int i = 0; i < arretesPossibles.Count; i++)
                    {
                        cumul += poids[i];
                        if (cumul >= r)
                        {
                            return arretesPossibles[i];
                        }
                    }

                    // securite suplémentaire 
                    return arretesPossibles[0]; // si rien n'est trouvé, on retourne la première arrete

                }
                return null; // si aucune arrete n'est trouvée
            }

            public void pheromones()
            {
                int Q  = 100; 
                int somme = 0;
                foreach (arretes a in this.chemin)
                {
                    somme += a.count;
                }
                foreach (arretes a in this.chemin)
                {
                    a.pheromonnes += (int)(Q / somme);
                }
            }
        }
    }
}
