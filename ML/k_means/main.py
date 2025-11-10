import pyqtgraph as pg
from pyqtgraph.Qt import QtCore, QtWidgets
import time
import random
import json
import numpy as np
from kmeans import Point,Centroid

configs = {}
examples = []
centroids = []
_similarity_metric = None
epoch_counter = 0
last_cost = 0
examples_plot = pg.ScatterPlotItem(size=10, pen=None,symbol="x")
centroids_plot = pg.ScatterPlotItem(size=10, pen=None,symbol="t")
timer = QtCore.QTimer()

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
        coordinates = np.array(coordinates)
        point = Point(coordinates)
        examples.append(point)

def _compute_euclid(point_coordinates,centroid_coordinates):

    if len(point_coordinates) != len(centroid_coordinates):
        raise Exception("EUCLID: Mismatch in dimensions!")
    n = len(point_coordinates)
    sum = 0

    for i in range(n):
        diff = (point_coordinates[i]-centroid_coordinates[i])**2
        sum = sum + diff

    return np.sqrt(sum)

def _random_placement():

    global centroids,examples,centroid_map

    domains = configs["domains"]
    dimensions = len(examples[0].coordinates)
    for i in range(0,configs["clustersNum"]):

        coordinates = [random.uniform(domains[dim][0],domains[dim][1]) for dim in range(0,dimensions)]
        coordinates = np.array(coordinates)
        point = Centroid(coordinates)
        centroids.append(point)
        centroids[i].colour = [random.randint(0,255) for _ in range(0,3)]


def _mean_placement():
    print("FOO")

def _place_centroids():
    global configs

    if configs["placement_method"] == "random":
        _random_placement()

def _move_centroids():
    global centroids, examples

    num = len(centroids)
    for i in range(0,num):
        sum = 0
        points_num = len(centroids[i].assigned_points)
        for j in range(0,points_num):
            sum = sum + centroids[i].assigned_points[j].coordinates
        if points_num > 0:
            new_coordinates = sum/points_num
            centroids[i].coordinates = new_coordinates



def _compute_energy_cost():

    centroid_num = configs["clustersNum"]
    sum = 0

    for i in range(0,centroid_num):
        points_num = len(centroids[i].assigned_points)
        for j in range(0,points_num):
            distance = _similarity_metric(centroids[i].coordinates,centroids[i].assigned_points[j].coordinates)
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
        centroids[centroid_index].assigned_points.append(examples[i])
        examples[i].colour = centroids[centroid_index].colour

def run_epoch():
    global epoch_counter,last_cost,centroids, examples,examples_plot,centroids_plot,timer

    num_examples = len(examples)
    num_centroids = len(centroids)
    epoch_counter = epoch_counter + 1

    print(f"Epoch:{epoch_counter}")

    _compute_distances(0,num_examples)
    _move_centroids()
    current_cost = _compute_energy_cost()

    if current_cost - last_cost == 0:
        timer.stop()
        return

    x_examples_val = []
    y_examples_val = []
    centroid_colours = []

    for i in range(0,num_centroids):
        points_num = len(centroids[i].assigned_points)
        for j in range(0,points_num):
            x_examples_val.append(centroids[i].assigned_points[j].coordinates[0])
            y_examples_val.append(centroids[i].assigned_points[j].coordinates[1])
        centroid_colours.append(centroids[i].colour)

    example_colours = [e.colour for e in examples]
    x_centroid_vals = [p.coordinates[0] for p in centroids]
    y_centroid_vals = [p.coordinates[1] for p in centroids]

    examples_plot.setData(x=x_examples_val,y=y_examples_val,brush=example_colours)
    centroids_plot.setData(x=x_centroid_vals,y=y_centroid_vals,brush=centroid_colours)

    last_cost = current_cost
    for i in range(0,num_centroids):
        centroids[i].assigned_points = [] # reset point list for next epoch

def main():

    global _similarity_metric,centroids

    start_time = time.perf_counter()
    _get_input()
    if configs["similarity_metric"] == "euclid":
        _similarity_metric = _compute_euclid

    _read_examples()
    _place_centroids()

    plot_widget = pg.plot(title="K means")

    plot_widget.addItem(examples_plot)
    plot_widget.addItem(centroids_plot)

    timer.timeout.connect(run_epoch)
    timer.start(0)

    end_time = time.perf_counter()
    print(f"Application took: {end_time-start_time} seconds.")
    pg.exec()


main()