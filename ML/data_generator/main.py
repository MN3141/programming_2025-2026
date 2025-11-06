import json
import pyqtgraph as pg
import math
import random

_configs = {}

def _check_dimensions():

    # domains mismatch
    if len(_configs["domains"]) < _configs["dimensions"]:
        return False

    # dispersion and region coordinates mismatch
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
    _configs["dimensions"] = json_data["dimensions"]
    _configs["domains"] = json_data["domains"]


    if not _check_dimensions():
        raise Exception("Mismatch in dimensions!")

def _compute_gauss(region_axis_value, dispersion_axis_value, random_axis_value):

    if dispersion_axis_value == 0:
        return 0

    exponent = -((region_axis_value - random_axis_value)**2) / (2 * (dispersion_axis_value**2))
    gauss = math.exp(exponent)

    return gauss

def _dump_data(points):
    file_name = "generated_points.ml"
    file_handle = open(file_name,"w")
    random.shuffle(points)
    for line in points:
        file_handle.write(str(line["coordinates"]) + ":"+ line["region_name"])
        file_handle.write("\n")

    file_handle.close()

def _generate_points():

# random.seed(a=None, version=2)
#     Initialize the random number generator.
#     If a is omitted or None, the current system time is used. If randomness sources are provided by the operating system, they are used instead of the system time (see the os.urandom() function for details on availability).
#     If a is an int, it is used directly.
#     With version 2 (the default), a str, bytes, or bytearray object gets converted to an int and all of its bits are used.

    noise_chance = random.uniform(0,0.1)
    dimension_num = _configs["dimensions"]
    points_num = _configs["pointsNum"]
    points = []

    available_points = points_num
    regions = _configs["regions"]

    for region in regions:
        region_points_num = random.randint(1,available_points)

        region_name = region["name"]
        region_color = tuple(random.randint(0, 255) for _ in range(3))

        for _ in range(region_points_num):

            dice = random.random()

            if dice > noise_chance:
                point_coords = [random.gauss(region["coordinates"][d],
                                            region["dispersions"][d])
                                for d in range(dimension_num)]
            else:
                point_coords = [random.triangular(region["coordinates"][d],
                                            region["dispersions"][d])
                                for d in range(dimension_num)]
            points.append({
                "coordinates": point_coords,
                "region_name": region_name,
                "color": region_color
            })
        if available_points < 0:
            available_points = 0

    return points

def _plot_data(points):
    x_vals = [p["coordinates"][0] for p in points]
    y_vals = [p["coordinates"][1] for p in points]
    colors = [p["color"] for p in points]

    scatter = pg.ScatterPlotItem(x=x_vals, y=y_vals, brush=colors, size=10, pen=None)

    plot_widget = pg.plot(title="Generated Points")
    plot_widget.addItem(scatter)
    pg.exec()

random.seed(a=None, version=2)
_get_configs()
points = _generate_points()
_dump_data(points)
_plot_data(points)


