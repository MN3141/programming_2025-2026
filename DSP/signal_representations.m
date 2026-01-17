clc
close all
clear

a = 230;
f = 50;
fs = 5*f;
ts = 1/fs;
alpha = 0; % defazaj
len = 1;
t = 0 : ts : len; 
s = a*sin(2*pi*f*t + alpha);

figure(1)
hold on
plot(t,s);
title("Semnal priza domeniu timp");
xlabel("t[s]")
ylabel("A[V]")
grid on;
hold off

S = fft(s);
magnitudes = abs(S);
magnitudes = fftshift(magnitudes);
freq = linspace(-fs/2,fs/2,length(magnitudes));
figure(2)
hold on
plot(freq,magnitudes);
title("Semnal priza domeniu frecventa");
xlabel("f[Hz]")
ylabel("Mg")
grid on;
hold off

alpha = 2 * pi * f / fs;
den = [1, -2*cos(alpha), 1];
num = [0, a*sin(alpha), 0];
sys = tf(num, den, 1/fs);
pzmap(sys);
%zgrid;