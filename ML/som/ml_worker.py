import PyQt6.QtCore as qcore
import math
import numpy as np
import sys
from neuron import SOMNeuron


class MLWorker(qcore.QObject):
    resultReady = qcore.pyqtSignal(int, np.ndarray, float, float)
    _learning_coeficient = 0.7
    _radius_coeficient = 6.1

    def _init_weights(self):
        rows = len(self._neurons)
        columns = len(self._neurons[0])

        domains = np.array(self._domains)
        dmin = domains[:, 0]
        dmax = domains[:, 1]

        # place the grid at centre initially
        inner_min = dmin + 0.25 * (dmax - dmin)
        inner_max = dmin + 0.75 * (dmax - dmin)

        for i in range(rows):
            for j in range(columns):
                self._neurons[i][j]._weights = np.random.uniform(inner_min, inner_max)

    def __init__(self, epochs_num, examples, domains, grid_no):
        super().__init__()

        self._epochs_num = epochs_num
        self._epoch_count = 0
        self._learning_rate = self._learning_coeficient
        self._neuron_radius = self._radius_coeficient + 1
        self._examples = examples
        self._domains = domains

        self._neurons = [[SOMNeuron() for _ in range(grid_no)] for _ in range(grid_no)]

        self._init_weights()

    def _compute_learning_rate(self):
        self._learning_rate = self._learning_coeficient * math.exp(
            -self._epoch_count / self._epochs_num
        )

    def _compute_neuron_radius(self):
        self._neuron_radius = int(
            self._radius_coeficient * math.exp(-self._epoch_count / self._epochs_num)
            + 1
        )

    def _compute_euclid(self, point_coordinates, neuron_coordinates):

        if len(point_coordinates) != len(neuron_coordinates):
            raise Exception("EUCLID: Mismatch in dimensions!")
        n = len(point_coordinates)
        sum = 0

        for i in range(n):
            diff = (point_coordinates[i] - neuron_coordinates[i]) ** 2
            sum = sum + diff

        return np.sqrt(sum)

    def _update_neuron_weights(self, row, column, delta):
        self._neurons[row][column]._weights = (
            self._neurons[row][column]._weights + self._learning_rate * delta
        )

    def _compute_neuron_positions(
        self, winner_neuron_row, winner_neuron_column, distance
    ):

        self._compute_neuron_radius()
        rows = len(self._neurons)
        columns = len(self._neurons[0])

        row_lower_limit = winner_neuron_row - self._neuron_radius
        if row_lower_limit < 0:
            row_lower_limit = 0

        row_upper_limit = winner_neuron_row + self._neuron_radius
        if row_upper_limit > rows:
            row_upper_limit = rows

        column_lower_limit = winner_neuron_column - self._neuron_radius
        if column_lower_limit < 0:
            column_lower_limit = 0

        column_upper_limit = winner_neuron_column + self._neuron_radius
        if column_upper_limit > columns:
            column_upper_limit = columns

        for i in range(row_lower_limit, row_upper_limit):
            for j in range(column_lower_limit, column_upper_limit):
                self._update_neuron_weights(i, j, distance)

    def stepEpochSlot(self):

        num_examples = len(self._examples)
        rows = len(self._neurons)
        columns = len(self._neurons[0])

        for example in range(num_examples):

            min_distance = sys.float_info.max
            for row in range(rows):
                for column in range(columns):
                    example_coordinates = self._examples[example].coordinates
                    neuron_coordinates = self._neurons[row][column]._weights
                    distance = self._compute_euclid(
                        example_coordinates, neuron_coordinates
                    )

                    if distance < min_distance:
                        min_distance = distance
                        winner_neuron_row = row
                        winner_neuron_column = column
            vector_diff = (
                self._examples[example].coordinates
                - self._neurons[winner_neuron_row][winner_neuron_column]._weights
            )
            self._compute_neuron_positions(
                winner_neuron_row, winner_neuron_column, vector_diff
            )

        self._epoch_count += 1
        self._compute_learning_rate()
        self._compute_neuron_radius()

        neuron_positions = np.array(
            [self._neurons[i][j]._weights for i in range(rows) for j in range(columns)]
        )

        self.resultReady.emit(
            self._epoch_count,
            neuron_positions,
            self._learning_rate,
            self._neuron_radius,
        )
