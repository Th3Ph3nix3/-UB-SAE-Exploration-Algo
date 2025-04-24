/**
 * @file Held-Karp-V2.cpp
 * @brief Solves the Traveling Salesman Problem (TSP) using the Held-Karp dynamic programming algorithm.
 * @author Th3_Ph3nix (Hugo)
 * @date 24/04/25
 * 
 * This program reads an adjacency matrix representing the distances between cities directly from 
 * the command-line arguments. It then applies the Held-Karp algorithm (dynamic programming approach)
 * to compute the minimum cost Hamiltonian cycle (optimal tour visiting all cities exactly once and 
 * returning to the starting point). The program outputs the optimal path and its cost to a file, 
 * and also records the execution time in a separate text file.
 * 
 * The program can be either individually in command line or with the graphical interface provided
 * with the rest of the algorithms as part of the project. If you want use the terminal and g++ for
 * compilation and execute do :
 *      For compilation : g++ -std=c++11 -O2 -Wall -Wextra -pedantic -o Held-Karp-V2 Held-Karp-V2.cpp
 *      For execute : ./Held-Karp-V2 012 102 220
 *      And if the program don't want execute try : chmod +x Held-Karp-V2
*/

#include <iostream>
#include <vector>
#include <limits>
#include <fstream>
#include <string>
#include <algorithm>
#include <chrono>

/**
 * Initializes DP tables for single-node subsets in TSP solution
 * @param g Dynamic programming cost table (modified)
 * @param prev Path reconstruction table (modified)
 * @param n Number of nodes in graph
 * @param d Distance matrix between nodes
 */
void init_size(std::vector<std::vector<int>> &g, std::vector<std::vector<int>> &prev, 
              int n, const std::vector<std::vector<int>>& d) {
    for (int i = 1; i < n; i++) {
        g[1 << i][i] = d[0][i];
        prev[1 << i][i] = 0;
    }
}

/**
 * Finds the optimal last node in Hamiltonian cycle before returning to start
 * @param n Number of nodes
 * @param g DP cost table
 * @param min_total [out] Minimum cycle cost
 * @param last_node [out] Optimal final node before returning to start
 * @param d Distance matrix
 * @param all_nodes Bitmask representing all nodes
 */
void search_last_node(int n, std::vector<std::vector<int>> &g, int &min_total, 
                     int &last_node, const std::vector<std::vector<int>>& d, int &all_nodes) {
    for (int i = 1; i < n; ++i) {
        int cost = g[all_nodes ^ 1][i] + d[i][0];
        if (cost < min_total) {
            min_total = cost;
            last_node = i;
        }
    }
}

/**
 * Reconstructs optimal path using predecessor table
 * @param current Last node in path before return to start
 * @param chemin [out] Vector to store optimal path
 * @param prev Predecessor table for path reconstruction
 * @param mask Bitmask of remaining nodes during reconstruction
 */
void opti_way(int current, std::vector<int> &chemin, 
             std::vector<std::vector<int>> &prev, int mask) {
    while (current != 0) {
        chemin.push_back(current);
        int prec = prev[mask][current];
        mask ^= (1 << current);
        current = prec;
    }
    std::reverse(chemin.begin(), chemin.end());
}

/**
 * Writes TSP solution to output file
 * Format: 
 * - First line: space-separated node indices in visit order
 * - Second line: total cost of optimal tour
 */
void write_output(std::vector<int> &chemin, int &min_total) {
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
}

/**
 * Validates command-line arguments
 * @return 1 if invalid arguments, 0 otherwise
 */
int test_arg(int argc, char* argv[]) {
    if (argc < 2) {
        std::cerr << "Usage: " << argv[0] << " ligne1 ligne2 ... ligneN" << std::endl;
        std::cerr << "Exemple: " << argv[0] << " 012 102 220" << std::endl;
        return 1;
    }
    return 0;
}

/**
 * Parses adjacency matrix from command-line arguments
 * @param n Matrix dimension (n x n)
 * @param argv Command-line arguments containing matrix rows
 * @param matrice [out] Adjacency matrix to populate
 * @return 1 on parsing error, 0 on success
 */
int filling_matrice(int n, char* argv[], std::vector<std::vector<int>> &matrice) {
    for (int i = 0; i < n; ++i) {
        std::string ligne(argv[i + 1]);
        if (ligne.size() != n) {
            std::cerr << "Erreur : chaque ligne doit contenir exactement " << n << " caractères." << std::endl;
            return 1;
        }
        for (int j = 0; j < n; ++j) {
            if (!isdigit(ligne[j])) {
                std::cerr << "Erreur : chaque caractère doit être un chiffre." << std::endl;
                return 1;
            }
            matrice[i][j] = ligne[j] - '0';
        }
    }
    return 0;
}

/**
 * Solves TSP using Held-Karp dynamic programming algorithm
 * @param d Distance matrix between nodes
 * @return Minimum tour cost
 */
int tsp(const std::vector<std::vector<int>>& d) {
    int n = d.size();
    int all_nodes = (1 << n) - 1;

    // DP tables: g[subset][k] = cost of shortest path through subset ending at k
    std::vector<std::vector<int>> g(1 << n, std::vector<int>(n, std::numeric_limits<int>::max()));
    std::vector<std::vector<int>> prev(1 << n, std::vector<int>(n, -1));
    
    init_size(g, prev, n, d);

    // Main DP loop: process subsets by increasing size
    for (int subset_size = 2; subset_size < n; ++subset_size) {
        for (int subset = 0; subset < (1 << n); ++subset) {
            if (__builtin_popcount(subset) == subset_size && (subset & (1 << 0)) == 0) {
                for (int k = 1; k < n; k++) {
                    if (subset & (1 << k)) {
                        int min_cost = std::numeric_limits<int>::max();
                        int best_predecessor = -1;
                        for (int m = 1; m < n; m++) {
                            if (subset & (1 << m) && m != k) {
                                int cost = g[subset ^ (1 << k)][m] + d[m][k];
                                if (cost < min_cost) {
                                    min_cost = cost;
                                    best_predecessor = m;
                                }
                            }
                        }
                        g[subset][k] = min_cost;
                        prev[subset][k] = best_predecessor;
                    }
                }
            }
        }
    }

    int min_total = std::numeric_limits<int>::max();
    int last_node = -1;
    search_last_node(n, g, min_total, last_node, d, all_nodes);

    std::vector<int> chemin;
    int mask = all_nodes ^ 1;
    int current = last_node;
    opti_way(current, chemin, prev, mask);

    write_output(chemin, min_total);

    return min_total;
}

/**
 * Program entry point
 * Handles input parsing, execution timing, and result output
 */
int main(int argc, char* argv[]) {
    if(test_arg(argc,argv) == 1) return 1;

    int n = argc - 1;
    std::vector<std::vector<int>> matrice(n, std::vector<int>(n));
    if (filling_matrice(n, argv, matrice) == 1) return 1;

    /* For show the matrice when we want debug the program
    std::cout << "Matrice lue depuis les arguments :" << std::endl;
    for (const auto& ligne : matrice) {
        for (const auto& valeur : ligne) {
            std::cout << valeur << " ";
        }
        std::cout << std::endl;
    }*/

    // Execution time measurement
    auto start = std::chrono::high_resolution_clock::now();
    int result = tsp(matrice);
    auto end = std::chrono::high_resolution_clock::now();
    std::chrono::duration<double> elapsed = end - start;

    std::cout << "Le coût minimum du TSP est : " << result << std::endl;
    std::cout << "Temps d'exécution : " << elapsed.count() << " secondes." << std::endl;

    // Write timing data
    std::ofstream temps("temps_execution.txt");
    if (temps.is_open()) {
        temps << elapsed.count() << std::endl;
        temps.close();
    } else {
        std::cerr << "Impossible d'ouvrir temps_execution.txt pour écrire le temps." << std::endl;
    }

    return 0;
}