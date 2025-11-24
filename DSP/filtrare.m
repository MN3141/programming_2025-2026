clc
close all
clear

foo = 0 % radiani
f = 3
fs = 48000;
ts = 1/fs;
len = 2;
t = 0 : ts : len; 
s = sin(2 * pi * f * t + foo);

h_n = [1/5,1/5,1/5,1/5,1/5];
f_zgomot = 20000;
zgomot = sin(2*pi*f_zgomot*t);

s2 = s + zgomot;
figure(1)
hold on
plot(t,s2,"red")
grid on
title("Semnal perturbat")
xlabel("t[s]")
ylabel("A")
hold off

s3 = conv(s2,h_n,"same");
figure(2)
hold on
plot(t,s3,"blue")
grid on
title("Semnal filtrat")
xlabel("t[s]")
ylabel("A")
hold off