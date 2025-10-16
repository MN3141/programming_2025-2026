import numpy as np
import matplotlib.pyplot as plt
import matplotlib.animation as animation
import json
import os
import datetime

_map = []
_configs = {}

# setup for matplot
fig = []
ax = []
mat = []
text = []

def _get_input():

    global _configs, _map

    config_file = "config.json"
    file_handle = open(config_file,"r")
    json_data = json.load(file_handle)
    file_handle.close()

    _configs["iterations"] = json_data["iterations"]
    _configs["random"] = json_data["random"]
    _configs["rows"] = len(json_data["map"])
    _configs["columns"] = len(json_data["map"][0])

    if _configs["random"] == 0:
        _map = np.array(json_data["map"], dtype=int)
    else:
        _map = _generate_map()

def _generate_map():
    return np.random.randint(0, 2, size=(_configs["rows"], _configs["columns"]))

def _check_neighbours(i,j):

    m = _configs["columns"]
    n = _configs["rows"]
    neighbour_sum = 0

    for x in range(i - 1, i + 2):
        for y in range(j - 1, j + 2):
            if (x == i and y == j):
                continue
            if 0 <= x < n and 0 <= y < m:
                neighbour_sum = neighbour_sum + _map[x,y]

    return neighbour_sum

def _check_cell(i,j):

    neighbours = _check_neighbours(i,j)
    new_value = 0

    if _map[i][j] == 1:
        if neighbours <= 1:
            new_value = 0
        elif neighbours >=4:
            new_value = 0
        elif neighbours == 2 or neighbours == 3:
            new_value = 1
    else:
        if neighbours == 3:
            new_value = 1

    return new_value

def update(frame):
    global _map

    rows, cols = _map.shape
    new_matrix = np.zeros((rows, cols), dtype=int)

    for i in range(rows):
        for j in range(cols):
            new_matrix[i][j] = _check_cell(i, j)

    _map = new_matrix
    mat.set_data(_map)
    text.set_text(f"Step {frame}")
    return [mat, text]

def main():

    _get_input()
    print(_map)
    global fig,ax,mat,text

    fig, ax = plt.subplots()
    mat = ax.matshow(_map, vmin=0, vmax=1, cmap="gray")
    text = ax.text(
        0.02, 0.95, "", color="red", transform=ax.transAxes,
        bbox={'facecolor': 'white', 'alpha': 0.6, 'pad': 2}
    )
    ani = animation.FuncAnimation(
        fig, update,
        frames=_configs["iterations"],
        interval=1000, # miliseconds
        blit=False,
        repeat=False
    )

    timestamp = datetime.datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
    path = os.path.join(f"out/conway_{timestamp}.gif")
    ani.save(path)

    ani.save(path)

main()
