clc
close all
clear

fs = 16000;
ts = 1/fs;
len = 2;
a = 1;
t = 0 : ts : len;
f = 1;
phase = pi/2;
% time domain
s1 = a * sin(2 * pi * f * t + phase);
s2 = a * sin(2 * pi * 10*f * t + 0);
s3 = square(2*pi*f*t);
s4 = sawtooth(2*pi*f*t);
s5 = sawtooth(2*pi*f*t,1/2);

s = s5;
% freq domain
S = fft(s);
magnitudes = abs(S);
magnitudes = fftshift(magnitudes);
freq = linspace(-fs/2,fs/2,length(magnitudes));

figure(1)
hold on
plot(t,s,"red")
grid on
title("Semnal sinusoidal in domeniul timp")
xlabel("t[s]")
ylabel("A[V]")
hold off

figure(2)
hold on
plot(magnitudes,"blue")
grid on
title("Semnal sinusoidal in domeniul frecv")
xlabel("f[Hz]")
ylabel("M")
hold off