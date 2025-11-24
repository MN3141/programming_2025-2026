# Genetic Algorithms

## What is the issue?

The Travelling salesman problem is an old mathematical problem that has the following question:
```
Given a list of cities and the distances between each pair of cities, what is the shortest possible route that visits each city exactly once and returns to the origin city?
```
## How to model the problem?
We shall consider the the cities (or streets since this problem can be applied to other situations as well) as nodes in a graph the are *connected to each other* and have and associated cost (in our case we shall use the hours that Google Maps estimates in hours). Each node has an assigned name and a list of neighbours and associated costs. In Python this would look like:
```python
class Node:
    def __init__(self,name):
        self.name = name
        self.neighbours = {}

    def add_path(self,neighbour_name,cost):
        self.neighbours[neighbour_name] = cost
```

## Used configuration
```json
{
    "Sibiu":{
        "Aiud":1.36,
        "Victoria":0.57,
        "Tritenii de Jos":1.50,
        "Petrosani":2.20
    },
    "Aiud":{
        "Sibiu":1.36,
        "Victoria":2.41,
        "Tritenii de Jos":0.47,
        "Petrosani":2.8
    },
    "Victoria":{
        "Aiud":2.41,
        "Sibiu":0.57,
        "Tritenii de Jos":2.43,
        "Petrosani":3.36
    },
    "Tritenii de Jos":{
        "Aiud":0.47,
        "Sibiu":1.50,
        "Victoria":2.43,
        "Petrosani":2.46
    },
    "Petrosani":{
        "Aiud":2.8,
        "Sibiu":2.20,
        "Victoria":3.36,
        "Tritenii de Jos":2.26
    }
}
```
## How is the solution represented?
The solution shall be represented as list cities in the optimum order to be visited (we could also say that this is the optimum permutation).
An example the best found individual is:
```
['Victoria', 'Sibiu', 'Petrosani', 'Tritenii de Jos', 'Aiud']
```
## How to choose fit individuals?
The two most common decision policies are random selection and tournament. While the former is the simplest policy, it does not guarantee that the desired traits are chosen. Though, it could be improved by randomly selecting amongts the *n* best candidates (where `n` is the tournament size), thus increasing the chance of having said traits.
## How to introduce mutation?
In order to ensure diversity (and thus better chances of finding a good candidate) amongts the population, some sort of mutation is needed to be applied. The simplest form would be just to take an individual and randomly swap two nodes/cities from it. This policy has been chosen due to it's simplicity and the fact that the solution is composed of unique city names (leaving no room for improvement).