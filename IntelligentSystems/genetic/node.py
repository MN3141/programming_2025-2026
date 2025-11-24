class Node:
    def __init__(self,name):
        self.name = name
        self.neighbours = {}

    def add_path(self,neighbour_name,cost):
        self.neighbours[neighbour_name] = cost