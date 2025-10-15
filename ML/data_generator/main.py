import json
import pyqtgraph as pg
import math
import random
import numpy as np

_configs = {}
_threshold = 0.5 # to be refined later

def _check_dimensions():

    for region in _configs["regions"]:
        if len(region["dispersions"]) > len(region["coordinates"]):
            return False

    return True

def _get_configs():

    config_file = "configs.json"
    file_handle = open(config_file,"r")
    json_data = json.load(file_handle)

    global _configs
    _configs["pointsNum"] = json_data["pointsNum"]
    _configs["regions"] = json_data["regions"]
    _configs["x_domain"] = json_data["x_domain"]
    _configs["y_domain"] = json_data["y_domain"]

    if not _check_dimensions():
        print("Mismatch in dimensions!")
        return

def _compute_gauss(region_axis_value, dispersion_axis_value, random_axis_value):

    exponent = -(region_axis_value-random_axis_value)*(region_axis_value-random_axis_value)/2*dispersion_axis_value*dispersion_axis_value
    gauss = math.exp(exponent)

    return gauss

def _dump_data(x,y,regions):
    file_name = "generated_points.txt"
    file_handle = ""

def _generate_points():

# random.seed(a=None, version=2)
#     Initialize the random number generator.
#     If a is omitted or None, the current system time is used. If randomness sources are provided by the operating system, they are used instead of the system time (see the os.urandom() function for details on availability).
#     If a is an int, it is used directly.
#     With version 2 (the default), a str, bytes, or bytearray object gets converted to an int and all of its bits are used.

    remaining_points = _configs["pointsNum"]
    x_values = []
    y_values = []
    region_names = []

    for region in _configs["regions"]:
        print(f"Region: {region["name"]}")
        x_region = region["coordinates"][0] # the implementation shall be more generic in the future
        y_region = region["coordinates"][1]

        x_dispersion = region["dispersions"][0]
        y_dispersion = region["dispersions"][1]

        region_points_num = random.randint(1,remaining_points) # assign random points to each region

        for i in range(0,remaining_points):
            x_random = random.uniform(_configs["x_domain"][0],_configs["x_domain"][1])
            y_random = random.uniform(_configs["y_domain"][0],_configs["y_domain"][1])

            gauss_x = _compute_gauss(x_region,x_dispersion,x_random)
            gauss_y = _compute_gauss(y_region,y_dispersion,y_random)

            _threshold = random.random()
            if gauss_x > _threshold:
                x_values.append(x_random)

            if gauss_y > _threshold:
                y_values.append(y_random)

            region_names.append(region)

        remaining_points = remaining_points - region_points_num

        if remaining_points <= 0:
            print(f"No more points for {region["name"]}")
            break


    return (x_values,y_values,region_names)

def _plot_data(x,y):

    n = min(len(x), len(y))
    x, y = x[:n], y[:n]
    pg.plot(x,y, pen=None, symbol='o',symbolBrush='r')
    pg.exec()

_get_configs()
(x,y,regions) = _generate_points()
_plot_data(x,y)


