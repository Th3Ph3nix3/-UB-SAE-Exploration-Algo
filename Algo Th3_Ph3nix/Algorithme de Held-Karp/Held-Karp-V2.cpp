/**
 * @file Held-Karp-V2.cpp
 * @brief Solves the Traveling Salesman Problem (TSP) using the Held-Karp dynamic programming algorithm.
 * @author Th3_Ph3nix (Hugo)
 * @date 24/04/25
 *
 * This program reads an adjacency matrix representing the distances between cities from a single
 * command-line argument as a string (e.g., "0,1,2;1,0,2;2,2,0"). It then applies the Held-Karp
 * algorithm (dynamic programming approach) to compute the minimum cost Hamiltonian cycle (optimal
 * tour visiting all cities exactly once and returning to the starting point). The program outputs
 * the optimal path and its cost to a file, and also records the execution time in a separate text file.
 *
 * Usage:
 *      Compile: g++ Held-Karp-V2.cpp -o Held-Karp-V2
 *      Run:     ./Held-Karp-V2 "0,1,2;1,0,2;2,2,0"
 */

 #include <iostream>
 #include <vector>
 #include <limits>
 #include <fstream>
 #include <string>
 #include <algorithm>
 #include <chrono>
 #include <sstream>
 
 /**
  * Parses a matrix from a string argument of the form "0,1,2;1,0,2;2,2,0"
  * @param s Input string representing the matrix
  * @return Parsed adjacency matrix as a 2D vector
  */
 std::vector<std::vector<int>> parseMatrice(const std::string& s) {
     std::vector<std::vector<int>> matrice;
     std::stringstream ss(s);
     std::string line;
 
     while (std::getline(ss, line, ';')) {
         std::vector<int> row;
         std::stringstream lineStream(line);
         std::string val;
         while (std::getline(lineStream, val, ',')) {
             try {
                 row.push_back(std::stoi(val));
             } catch (const std::exception& e) {
                 std::cerr << "Erreur d'analyse: '" << val << " n'est pas un entier valide." << std::endl;
                 exit(1);
             }
         }
         matrice.push_back(row);
     }
     return matrice;
 }
 
 /**
  * Checks if the matrix is square (n x n)
  * @param matrice The matrix to check
  * @return true if square, false otherwise
  */
 bool isSquareMatrix(const std::vector<std::vector<int>>& matrice) {
     size_t n = matrice.size();
     for (const auto& row : matrice) {
         if (row.size() != n) return false;
     }
     return true;
 }
 
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
  * @param path [out] Vector to store optimal path
  * @param prev Predecessor table for path reconstruction
  * @param mask Bitmask of remaining nodes during reconstruction
  */
 void reconstruct_path(int current, std::vector<int> &path, 
                      std::vector<std::vector<int>> &prev, int mask) {
     while (current != 0) {
         path.push_back(current);
         int prec = prev[mask][current];
         mask ^= (1 << current);
         current = prec;
     }
     std::reverse(path.begin(), path.end());
 }
 
 /**
  * Writes TSP solution to output file
  * Format: 
  * - First line: space-separated node indices in visit order
  * - Second line: total cost of optimal tour
  * @param path The optimal path
  * @param min_total The minimum cost
  */
 void write_output(const std::vector<int> &path, int min_total) {
     std::ofstream output("output.txt");
     if (output.is_open()) {
         for (size_t i = 0; i < path.size(); ++i) {
             output << path[i];
             if (i != path.size() - 1)
                 output << " ";
         }
         output << std::endl;
         output << min_total;
         output.close();
     }
     else {
         std::cerr << "Erreur : impossible d'ouvrir output.txt pour l'écriture." << std::endl;
     }
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
 
     std::vector<int> path;
     int mask = all_nodes ^ 1;
     int current = last_node;
     reconstruct_path(current, path, prev, mask);
 
     write_output(path, min_total);
 
     return min_total;
 }
 
 /**
  * Program entry point
  * Handles input parsing, execution timing, and result output
  */
 int main(int argc, char* argv[]) {
     if (argc != 2) {
         std::cerr << "Usage: " << argv[0] << " \"0,1,2;1,0,2;2,2,0\"" << std::endl;
         return 1;
     }
 
     std::string input = argv[1];
     std::vector<std::vector<int>> matrice = parseMatrice(input);
 
     if (!isSquareMatrix(matrice)) {
         std::cerr << "Erreur : La matrice doit être carrée (n x n)." << std::endl;
         return 1;
     }
 
     // Execution time measurement
     auto start = std::chrono::high_resolution_clock::now();
     int result = tsp(matrice);
     auto end = std::chrono::high_resolution_clock::now();
     std::chrono::duration<double> elapsed = end - start;
 
     std::cout << "Cout minimum TSP : " << result << std::endl;
     std::cout << "Temps d'execution: " << elapsed.count() << " secondes." << std::endl;
 
     // Write timing data
     std::ofstream times("temps_execution.txt");
     if (times.is_open()) {
         times << elapsed.count() << std::endl;
         times.close();
     } else {
         std::cerr << "Erreur : impossible d'ouvrir temps_execution.txt pour l'écriture." << std::endl;
     }
 
     return 0;
 }
 