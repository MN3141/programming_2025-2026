import numpy as np
import matplotlib.pyplot as plt

time = np.linspace(0, 10, 1000)
x_t = np.sin(2 * np.pi * time)
#h_t = np.exp(-time)
h_t = 1/2
y_t = np.convolve(x_t, h_t)

plt.figure()
plt.plot(time, x_t, label='Signal', color='blue')
plt.title('Input Signal')
plt.xlabel('Time [s]')
plt.ylabel('Amplitude')
plt.grid()
plt.legend()

plt.figure()
plt.plot(y_t, label='Convolved Signal', color='green')
plt.title('Convolved Signal')
plt.xlabel('Samples')
plt.ylabel('Amplitude')
plt.grid()
plt.legend()
plt.show()