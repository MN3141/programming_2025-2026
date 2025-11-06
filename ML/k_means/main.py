import pyqtgraph as pg
import time
import random
import json
import multiprocessing as mp
from kmeans import Point,Example

configs = {}
examples = []
centroids = []
_similarity_metric = None

def _get_input():

    global configs

    config_file = "configs.json"
    file_handle = open(config_file,"r")
    configs = json.load(file_handle)
    file_handle.close()

def _read_examples():

    global configs,examples
    generated_points_file = configs["generated_points_path"]

    file_handle = open(generated_points_file,"r")
    data = file_handle.readlines()
    file_handle.close()

    for line in data:

        coordinates = line.split(":")[0]
        coordinates = coordinates[1:-1]
        coordinates = [float(num) for num in coordinates.split(",")]

        point = Example(coordinates)
        examples.append(point)

def _compute_euclid(point_coordinates,centroid_coordinates):

    if len(point_coordinates) != len(centroid_coordinates):
        raise Exception("EUCLID: Mismatch in dimensions!")
    n = len(point_coordinates)
    sum = 0

    for i in range(n):
        diff = (point_coordinates[i]-centroid_coordinates[i])**2
        sum = sum + diff

    return sum

def _random_placement():

    global centroids,examples

    domains = configs["domains"]
    dimensions = len(examples[0].coordinates)
    for i in range(0,configs["clustersNum"]):

        coordinates = [random.uniform(domains[dim][0],domains[dim][1]) for dim in range(0,dimensions)]
        point = Point(coordinates)
        centroids.append(point)
        centroids[i].colour = [random.randint(0,255) for _ in range(0,3)]


def _mean_placement():
    print("FOO")

def _place_centroids():
    global configs

    if configs["placement_method"] == "random":
        _random_placement()

def _compute_energy_cost(point_index0,point_index1):

    centroid_num = configs["clustersNum"]
    sum = 0

    for _ in range(0,centroid_num):
        for i in range(point_index0,point_index1):
            distance = _similarity_metric(examples[i].coordinates,examples[i].assigned_centroid.coordinates)
            sum = sum + distance
    return sum

def _compute_distances(point_index0,point_index1):

    min_distance = 999999

    global examples,centroids,_similarity_metric
    centroid_num = configs["clustersNum"]
    for i in range(point_index0,point_index1):
        centroid_index = -1
        for j in range(centroid_num):
            distance = _similarity_metric(examples[i].coordinates,centroids[j].coordinates)
            if distance < min_distance:
                min_distance = distance
                centroid_index = j
        examples[i].assigned_centroid = centroids[centroid_index]


def main():

    global _similarity_metric

    start_time = time.perf_counter()
    epoch_counter = 0
    _get_input()
    if configs["similarity_metric"] == "euclid":
        _similarity_metric = _compute_euclid

    _read_examples()
    _place_centroids()
    n = len(examples)
    _compute_distances(0,n)

    x_vals = [p.coordinates[0] for p in examples]
    y_vals = [p.coordinates[1] for p in examples]
    colors = [p.assigned_centroid.colour for p in examples]

    examples_plot = pg.ScatterPlotItem(x=x_vals, y=y_vals, brush=colors, size=10, pen=None,symbol="x")

    x_vals = [p.coordinates[0] for p in centroids]
    y_vals = [p.coordinates[1] for p in centroids]
    colors = [p.colour for p in centroids]
    centroids_plot = pg.ScatterPlotItem(x=x_vals, y=y_vals, brush=colors, size=10, pen=None,symbol="t")

    plot_widget = pg.plot(title="Generated Points")
    plot_widget.addItem(examples_plot)
    plot_widget.addItem(centroids_plot)

    end_time = time.perf_counter()
    print(f"Application took: {end_time-start_time} seconds.")
    pg.exec()

main()