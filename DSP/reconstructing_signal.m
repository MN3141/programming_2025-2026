clc
close all
clear
a = unifrnd(1,10);
f = unifrnd(50, 100);
foo = unifrnd(0,pi);


fs = 1000;
ts = 1/fs;
len = 1/50;
t = 0 : ts : len; 
s = a * sin(2 * pi * f * t + foo);

figure(1)
hold on
plot(t,s,"red")
grid on
title("Semnal sinusoidal in domeniul timp")
xlabel("t[s]")
ylabel("A[V]")
hold off

t_in = input("perioada: ");
a_in = input("amplitudine: ");
foo_in = input("faza initiala: ");

f_in = 1 / t_in;
s_in = a_in * sin(2 * pi * f_in * t_in + foo_in);

figure(1)
hold on
plot(t_in, s_in, "blue")
grid on
hold off
%stem(t,s)