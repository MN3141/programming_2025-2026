import numpy as np
from abc import ABC, abstractmethod


class Neuron(ABC):

    _bias = None
    _weights = None

    @abstractmethod
    def __init__(self):
        pass

    @abstractmethod
    def compute_output(input_vector):
        pass


class SOMNeuron(Neuron):
    def __init__(self):
        super().__init__()
        self._bias = 0
        self._weights = np.array([])

    def compute_output(input_vector):
        return 0
