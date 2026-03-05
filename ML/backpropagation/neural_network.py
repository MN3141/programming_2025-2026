import random
import math


class NeuralNetwork:
    def __init__(
        self, learning_mode, hidden_num, out_num, x, y, error_threshold, learning_rate
    ):
        self.learning_mode = learning_mode
        self.x = x
        self.y = y
        self.learning_rate = learning_rate
        self.error_threshold = error_threshold

        self.input_num = len(x[0])
        self.hidden_num = hidden_num
        self.out_num = out_num

        self.w12 = [
            [random.uniform(0.01, 0.2) for _ in range(self.input_num)]
            for _ in range(hidden_num)
        ]
        self.w23 = [
            [random.uniform(0.01, 0.2) for _ in range(hidden_num)]
            for _ in range(out_num)
        ]

        self.b2 = [random.random() for _ in range(hidden_num)]
        self.b3 = [random.random() for _ in range(out_num)]

        self.total_dw12 = [
            [0 for _ in range(self.input_num)] for _ in range(self.hidden_num)
        ]
        self.total_dw23 = [
            [0 for _ in range(self.hidden_num)] for _ in range(self.out_num)
        ]
        self.total_db2 = [0 for _ in range(self.hidden_num)]
        self.total_db3 = [0 for _ in range(self.out_num)]

        self.out2 = []
        self.out3 = []
        self.error = float("inf")

    def _activation(self, x):
        return 1 / (1 + math.exp(-x))

    def _activation_derivative(self, out):
        return out * (1 - out)

    def _forward(self, input):
        self.out2 = []
        for h in range(self.hidden_num):
            s = 0
            for i in range(self.input_num):
                s += self.w12[h][i] * input[i]
            s += self.b2[h]
            self.out2.append(self._activation(s))

        self.out3 = []

        for o in range(self.out_num):
            s = 0
            for h in range(self.hidden_num):
                s += self.w23[o][h] * self.out2[h]
            s += self.b3[o]
            self.out3.append(self._activation(s))

        return self.out3

    def _backward(self, input, target):

        delta_output = []
        err = 0
        for o in range(self.out_num):
            err = self.out3[o] - target[o]
            delta_output.append(err)

        if self.learning_mode == "online":

            for o in range(self.out_num):
                deriv = self._activation_derivative(self.out3[o])
                for h in range(self.hidden_num):
                    delta = 2 * delta_output[o] * deriv * self.out2[h]
                    self.w23[o][h] -= self.learning_rate * delta
                delta_bias = 2 * delta_output[o] * deriv
                self.b3[o] -= self.learning_rate * delta_bias

            for h in range(self.hidden_num):
                err = 0
                for o in range(self.out_num):
                    deriv_out = self._activation_derivative(self.out3[o])
                    err += delta_output[o] * deriv_out * self.w23[o][h]
                err *= 2
                deriv_hidden = self._activation_derivative(self.out2[h])
                delta_hidden = err * deriv_hidden

                for i in range(self.input_num):
                    self.w12[h][i] -= self.learning_rate * delta_hidden * input[i]

                self.b2[h] -= self.learning_rate * delta_hidden

        else:
            for o in range(self.out_num):
                deriv = self._activation_derivative(self.out3[o])
                for h in range(self.hidden_num):
                    self.total_dw23[o][h] += delta_output[o] * self.out2[h] * deriv
                self.total_db3[o] += 2* delta_output[o] * deriv

            for h in range(self.hidden_num):
                err = 0
                for o in range(self.out_num):
                    deriv_out = self._activation_derivative(self.out3[o])
                    err += delta_output[o] * deriv_out * self.w23[o][h]
                err *= 2
                deriv_hidden = self._activation_derivative(self.out2[h])
                delta_hidden = err * deriv_hidden

                for i in range(self.input_num):
                    self.total_dw12[h][i] += delta_hidden * input[i]

                self.total_db2[h] -= delta_hidden

    def train(self):
        while self.error > self.error_threshold:
            total_error = 0

            for input, target in zip(self.x, self.y):
                out = self._forward(input)
                for o in range(self.out_num):
                    total_error += (out[o] - target[o]) ** 2
                self._backward(input, target)

            if self.learning_mode == "offline":
                for o in range(self.out_num):
                    for h in range(self.hidden_num):
                        self.w23[o][h] -= self.learning_rate * self.total_dw23[o][h]
                    self.b3[o] -= self.learning_rate * self.total_db3[o]

                for h in range(self.hidden_num):
                    for i in range(self.input_num):
                        self.w12[h][i] -= self.learning_rate * self.total_dw12[h][i]
                    self.b2[h] -= self.learning_rate * self.total_db2[h]

                self.total_dw12 = [[0] * self.input_num for i in range(self.hidden_num)]
                self.total_dw23 = [[0] * self.hidden_num for i in range(self.out_num)]
                self.total_db2 = [0] * self.hidden_num
                self.total_db3 = [0] * self.out_num

            self.error = total_error

    def predict(self, input):
        return self._forward(input)
