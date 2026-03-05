clc
close all
clear

filename = "Scorpions_sine.wav";
[data,fs] = audioread(filename);
ts = 1/fs;

y_freq = fftshift(abs(fft(data)));
freq = linspace(-fs/2,fs/2,length(data));

figure(1)
hold on
grid on
plot(freq,y_freq,"blue");
title("Scorpions Spectru magnitudine");
xlabel("f[Hz]");
ylabel("Mg");
hold off

freq_taiere = 4050;
m2 = 0.95;
theta = 2*pi*freq_taiere/fs;
B = [1 -2*cos(theta) 1];
A = [1, -2*m2*cos(theta), m2*m2];

data2 = filter(B,A,data);

freq_taiere2 = 1150.02;
theta2 = 2*pi*freq_taiere2/fs;

B = [1 -2*cos(theta2) 1];
A = [1, -2*m2*cos(theta2), m2*m2];

data3 = filter(B,A,data2);
freq3 = fftshift(abs(fft(data3)));
%sound(data3,fs);
figure(2)
plot(freq,freq3,"red");
title("Scorpions Spectru Fitrat");
xlabel("f[Hz]");
ylabel("Mg");
hold off

freqz(B,A,"whole",fs);
