#include <iostream>
#include <vector>
#include <limits>
#include <fstream>
#include <string>
#include <sstream>
#include <algorithm>

//Sous-programme pour renvoyer la première ligne
int obtenirNbPoints(){
    std::ifstream point("../../Points/points.txt");

    // Vérifier si le fichier est ouvert
    if (!point) {
        std::cout << "Erreur : impossible d'ouvrir le fichier." << std::endl;
        return 0;
    }

    // Lire la première ligne
    std::string ligne;
    if (std::getline(point, ligne)) {
        try {
            // Convertir la ligne en entier
            int nombre = std::stoi(ligne);
            std::cout << "Le nombre sur la première ligne est : " << nombre << std::endl;
            point.close();
            return nombre;
        } catch (const std::invalid_argument& e) {
            std::cout << "Erreur : la première ligne ne contient pas un entier valide." << std::endl;
        } catch (const std::out_of_range& e) {
            std::cout << "Erreur : l'entier est hors de portée." << std::endl;
        }
    } else {
        std::cout << "Erreur : impossible de lire la première ligne." << std::endl;
    }

    // Fermer le fichier
    point.close();
    return 0;
}

int initMatrice(std::vector<std::vector<int>>& matrice){
    // Ouvrir le fichier
    std::ifstream fichier("../../Points/points.txt");
    if (!fichier) {
        std::cout << "Erreur : impossible d'ouvrir le fichier." << std::endl;
        return 1;
    }

    // Ignorer la première ligne
    std::string premiereLigne;
    std::getline(fichier, premiereLigne);

    // Lire les lignes restantes du fichier
    std::string ligne;
    int i = 0;
    while (std::getline(fichier, ligne) && i < matrice.size()) {
        std::istringstream stream(ligne); // Créer un flux pour la ligne
        int valeur, j = 0;
        while (stream >> valeur && j < matrice[i].size()) {
            matrice[i][j] = valeur;
            ++j;
        }
        ++i;
    }


    fichier.close();
    return 1;
}

// Fonction pour calculer le coût minimum du TSP
int tsp(const std::vector<std::vector<int>>& d) {
    int n = d.size();
    int all_nodes = (1 << n) - 1; // Tous les nœuds sont inclus dans le masque

    // Tableau pour stocker les coûts minimums
    std::vector<std::vector<int>> g(1 << n, std::vector<int>(n, std::numeric_limits<int>::max()));
    std::vector<std::vector<int>> prev(1 << n, std::vector<int>(n, -1));
    
    // Initialisation pour les ensembles de taille 1
    for (int k = 1; k < n; k++) {
        g[1 << k][k] = d[0][k];
        prev[1 << k][k] = 0;
    }

    // Remplissage du tableau g pour les ensembles de taille supérieure
    for (int s = 2; s < n; ++s) {
        for (int subset = 0; subset < (1 << n); ++subset) {
            if (__builtin_popcount(subset) == s && (subset & (1 << 0)) == 0) {
                for (int k = 1; k < n; k++) {
                    if (subset & (1 << k)) {
                        int min_cost = std::numeric_limits<int>::max();
                        int best_m = -1;
                        for (int m = 1; m < n; m++) {
                            if (subset & (1 << m) && m != k) {
                                min_cost = std::min(min_cost, g[subset ^ (1 << k)][m] + d[m][k]);
                                best_m = m;
                            }
                        }
                        g[subset][k] = min_cost;
                        prev[subset][k] = best_m;
                    }
                }
            }
        }
    }

    // Recherche du dernier nœud du chemin optimal
    int min_total = std::numeric_limits<int>::max();
    int last_node = -1;
    for (int k = 1; k < n; ++k) {
        int cost = g[all_nodes ^ 1][k] + d[k][0];
        if (cost < min_total) {
            min_total = cost;
            last_node = k;
        }
    }

    // Reconstruction du chemin optimal
    std::vector<int> chemin;
    int mask = all_nodes ^ 1; // tous les nœuds sauf 0
    int current = last_node;
    while (current != 0) {
        chemin.push_back(current);
        int prec = prev[mask][current];
        mask ^= (1 << current);
        current = prec;
    }
    std::reverse(chemin.begin(), chemin.end());

    // Écriture dans le fichier output.txt
    std::ofstream output("output.txt");
    if (output.is_open()) {
        for (size_t i = 0; i < chemin.size(); ++i) {
            output << chemin[i];
            if (i != chemin.size() - 1)
                output << " ";
        }
        output << std::endl;
        output << min_total;
        output.close();
    }
    else {
        std::cerr << "Impossible d'ouvrir output.txt pour écrire le chemin." << std::endl;
    }

    return min_total;
}

int main() {
    int nb_point = obtenirNbPoints();
    std::vector<std::vector<int>> matrice(nb_point, std::vector<int>(nb_point));

    initMatrice(matrice);

    std::cout << "Matrice lue depuis le fichier :" << std::endl;
    for (const auto& ligne : matrice) {
        for (const auto& valeur : ligne) {
            std::cout << valeur << " ";
        }
        std::cout << std::endl;
    }

    int result = tsp(matrice);
    std::cout << "Le coût minimum du TSP est : " << result << std::endl;

    return 0;
}
