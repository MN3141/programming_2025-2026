import json
import random
import copy
import genetic_algorithm as ga
from node import Node

config_data = {}
nodes = {}

def read_input():
    global config_data
    config_file = "network.json"
    with open(config_file, "r") as f:
        config_data = json.load(f)

def setup_nodes():

    global nodes
    for city, neighbours in config_data.items():
        node_obj = Node(city)
        for neighbour, cost in neighbours.items():
            node_obj.add_path(neighbour, cost)
        nodes[city] = node_obj

def compute_cost(route):

    total_cost = 0
    for i in range(len(route) - 1):
        a, b = route[i], route[i + 1]
        if b not in nodes[a].neighbours:
            return None  # invalid route
        total_cost += nodes[a].neighbours[b]
    # Return to start
    a, b = route[-1], route[0]
    if b not in nodes[a].neighbours:
        return None
    total_cost += nodes[a].neighbours[b]
    return total_cost

def create_individual_func():
    route = list(nodes.keys())
    random.shuffle(route)
    return route

def fitness_func(individual):
    cost = compute_cost(individual)
    if cost is None:
        return -1e6
    return -cost

def selection_func(population, fitness_scores, tournament_size=3):
    selected = []
    for _ in range(2):
        competitors = random.sample(list(zip(population, fitness_scores)), tournament_size)
        winner = max(competitors, key=lambda x: x[1])[0]
        selected.append(winner)
    return selected[0], selected[1]

def crossover_func(parent1, parent2):
    size = len(parent1)
    a, b = sorted(random.sample(range(size), 2))
    child1 = [None] * size
    child2 = [None] * size

    child1[a:b] = parent1[a:b]
    child2[a:b] = parent2[a:b]

    fill_pos = b
    for gene in parent2[b:] + parent2[:b]:
        if gene not in child1:
            if fill_pos >= size:
                fill_pos = 0
            child1[fill_pos] = gene
            fill_pos += 1

    fill_pos = b
    for gene in parent1[b:] + parent1[:b]:
        if gene not in child2:
            if fill_pos >= size:
                fill_pos = 0
            child2[fill_pos] = gene
            fill_pos += 1

    return child1, child2

def mutation_func(individual):
    a, b = random.sample(range(len(individual)), 2)
    individual[a], individual[b] = individual[b], individual[a]
    return individual

def main():
    read_input()
    setup_nodes()

    ga_instance = ga.GeneticAlgorithm(
        population_size=50,
        fitness_func=fitness_func,
        create_individual_func=create_individual_func,
        selection_func=selection_func,
        crossover_func=crossover_func,
        mutation_func=mutation_func,
        crossover_rate=0.9,
        mutation_rate=0.01,
        elitism_count=2
    )

    best_route, best_fitness = ga_instance.run(num_generations=200)
    print("\nBest route found:", best_route)
    print("Total cost:", -best_fitness)

main()