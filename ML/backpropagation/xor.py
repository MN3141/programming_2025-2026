from neural_network import NeuralNetwork

X = [
    [0, 0],
    [0, 1],
    [1, 0],
    [1, 1]
]

Y = [
    [0.1],
    [0.9],
    [0.9],
    [0.1]
]
threshold = 10 ** -27
# learning_rate: 0.3
# online: -27
print(f"Threshold {threshold}")
xor_network = NeuralNetwork(
    learning_mode="online",
    hidden_num=2,
    out_num=1,
    x=X,
    y=Y,
    error_threshold=threshold,
    learning_rate=0.5
)

xor_network.train()

for x in X:
    print(x, "->", xor_network.predict(x))
