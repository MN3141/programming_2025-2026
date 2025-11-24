

clc
close all
clear

a = 1;
f0 = 1;

f1 = f0;
fs = 16000;
durata = 2;

s1 = 4*a/pi;
t = 0:1/fs:durata;
signal1 = s1*sin(2*pi*f0*t);

s3 = 4*a/(3*pi);
f3 = 3*f0; % k*f0

foo = s1*square(2*pi*f0*t);

%fourier_sum = signal1+signal2;

fourier_sum = 0;
harmonics = 500;
for k=1:harmonics

    % fenomen Gibbs

    if rem(k,2)==1
     sk = 4*a/(k*pi);
    fk = k*f0;

    signal_k = sk*sin(2*pi*fk*t);
    fourier_sum = fourier_sum + signal_k;
    end

end

figure(1)
hold on
plot(t,fourier_sum,"red")
grid on
title("Semnal sinusoidal in domeniul timp")
xlabel("t[s]")
ylabel("A[V]")
hold off 

figure(2)
hold on
plot(t,foo)
hold off

