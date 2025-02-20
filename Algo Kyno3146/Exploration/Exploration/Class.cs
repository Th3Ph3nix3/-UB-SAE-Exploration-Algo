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
            public int cout;
            public int pheromonnes;
            public arretes(noeud depart, noeud arrivee, int cout, int pheromonnes) //constructeur
            {
                this.depart = depart;
                this.arrivee = arrivee;
                this.cout = cout;
                this.pheromonnes = 0;
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
            public fourmi(noeud position)
            {
                this.position = position;
            }

            public void deplacement(List<arretes> arretes)
            {
                List<arretes> arretesPossibles = new List<arretes>();
                foreach (arretes arrete in arretes)
                {
                    if (arrete.depart == this.position)
                    {
                        arretesPossibles.Add(arrete);
                    }
                }
                if (arretesPossibles.Count > 0)
                {
                    Random rand = new Random();
                    int index = rand.Next(arretesPossibles.Count);
                    this.position = arretesPossibles[index].arrivee;
                }
            }
        }
    }
}
