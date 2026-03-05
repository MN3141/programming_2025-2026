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
x = a*sin(2*pi*f*t + alpha);

y = zeros(length(x));
y(1) = 0;
for i=2:1:length(x)
    y(i) = x(i) + 2*x(i-1) -y(i-1);
end

figure(1)
hold on
plot(t,x);
title("x[n] in domeniul timp");
xlabel("t[s]")
ylabel("A[V]")
grid on;
hold off

figure(2)
hold on
plot(t,y);
title("y[n] in domeniul timp");
xlabel("t[s]")
ylabel("A[V]")
grid on;
hold off

Y = fft(y);
magnitudes = abs(Y);
magnitudes = fftshift(magnitudes);
freq = linspace(-fs/2,fs/2,length(magnitudes));
figure(3)
hold on
plot(freq,magnitudes);
title("y[n] in domeniul frecventa");
xlabel("f[Hz]")
ylabel("Mg")
grid on;
hold off

A = [1, 1];
B = [1,2];
sys = tf(B, A, 1/fs);
figure(4)
pzmap(sys);
title("Diagrama pol-zero")
%zgrid;