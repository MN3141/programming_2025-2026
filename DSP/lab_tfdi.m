clc
close all
clear

fs = 16000;
f = 50;
f2 = 2*f;
f3 = 100*f;
ts = 1/fs;
len = 2;
t = 0 : ts : len; 
s0 = sin(2 * pi * f * t);
s1 = sin(2 * pi * f2 * t);
s2 = sin(2 * pi * f3 * t);

sum_timp = s0 +s2 +s1;
sum_freq = fftshift(abs(fft(sum_timp)));
freq = linspace(-fs/2,fs/2,length(sum_freq));
f_taiere = 4800;
f_taiere2 = 5200;


figure(1)
hold on
plot(t,sum_timp,"red")
grid on
title("Semnal sinusoidal in domeniul timp")
xlabel("t[s]")
ylabel("A[V]")
hold off


figure(2)
hold on
plot(freq,sum_freq,"red")
grid on
title("Semnal sinusoidal in domeniul frecventa")
xlabel("f[Hz]")
ylabel("Mg")
hold off

N = 1000; %ordin filtru
n = (-N-1)/2 : (N-1)/2;
h_n = (2*f_taiere/fs)*sinc(2*f_taiere*n/fs); % Trece sus

h_n_fts = sinc(n) - h_n;
h_n_ob = sinc(n) - (2*f_taiere2/fs)*sinc(n*2*f_taiere2/fs) +(2*f_taiere/fs)*sinc(n*2*f_taiere/fs);

figure(3)
hold on
plot(n,h_n_ob,"green")
grid on
title("Raspuns intern")
xlabel("n")
ylabel("A")
hold off

H_n = fftshift(abs(fft(h_n_ob)));
samples = linspace(-fs/2,fs/2,length(H_n));

figure(4)
hold on
plot(samples,H_n,"yellow")
grid on
title("Caracteristica filtru")
xlabel("n")
ylabel("Mg")
hold off

y_n = conv(sum_timp,h_n,"same");
Y_n = fftshift(abs(fft(y_n)));

figure(5)
hold on
plot(freq,Y_n,"red")
grid on
title("Semnal iesire")
xlabel("f")
ylabel("Mg")
hold off


