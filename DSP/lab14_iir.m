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

freq_taiere = 1150;
theta = 2*pi*freq_taiere/fs;
m2 = 0.95;
B = [1 -2*cos(theta) 1];
A = [1, -2*m2*cos(theta), m2*m2];

data2 = filter(B,A,data);
freq2 = fftshift(abs(fft(data2)));
figure(2)
plot(freq,freq2,"red");
title("Scorpions Spectru 2 magnitudine");
xlabel("f[Hz]");
ylabel("Mg");
hold off

%freqz(B,A,"whole",fs);
