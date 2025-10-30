import numpy as np
import matplotlib.pyplot as plt
from scipy import signal
import mplcursors

start_time = 0
end_time = 10
time = np.linspace(start_time, end_time,1000)

#time_signal = np.sin(2 * np.pi * 0.5 * time)
# time_signal = signal.square(2 * np.pi * 0.5 * time)
# time_signal = signal.sawtooth(2 * np.pi * 0.5 * time)
time_signal = signal.sawtooth(2 * np.pi * 0.5 * time, 1/2)
freq_signal = np.fft.fft(time_signal)

plt.figure()
plt.plot(time, time_signal, label='Original Signal', color='blue')
plt.title('Time Signal 0')
plt.xlabel('Time [s]')
plt.ylabel('Amplitude')
plt.grid()
plt.legend()

plt.figure()
frequencies = np.fft.fftfreq(len(time), d=(time[1] - time[0]))
plt.plot(frequencies, np.abs(freq_signal), label='Frequency Domain', color='red')
plt.title('Frequency Spectrum 0')
plt.xlabel('Frequency [Hz]')
plt.ylabel('Magnitude')
plt.grid()
plt.legend()

mplcursors.cursor(hover=True)
plt.show()
