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
signal_freq = linspace(-fs/2,fs/2,length(sum_freq));

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
plot(signal_freq,sum_freq,"red")
grid on
title("Semnal sinusoidal in domeniul frecventa")
xlabel("f[Hz]")
ylabel("Mg")
hold off

N = 1000;
f_taiere = 150;
q = (-(N-1)*fs)/(2*N):fs/N:((N-1)*fs)/(2*N); 
H_n = double(abs(q) <= f_taiere);

h_final = [];
k = -(N-1)/2:(N-1)/2;
for n=-(N-1)/2:(N-1)/2
   
    h_temp = 1/N * sum(H_n.*cos(2*pi*k*n/N));
    h_final = [h_final,h_temp];
end

h_final_freq = fftshift(abs(fft(h_final)));
freq = linspace(-fs/2,fs/2,length(h_final_freq));

figure(3)
hold on
plot(freq,h_final_freq,"red")
grid on
title("Filtru")
xlabel("f[Hz]")
ylabel("Mg")
hold off

Y_n = conv(sum_timp,h_final,"same");
figure(4)
hold on
plot(signal_freq,Y_n,"red")
grid on
title("Semnal filtrat")
xlabel("f[Hz]")
ylabel("Mg")
hold off
