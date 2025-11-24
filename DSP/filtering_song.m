clc
close all
clear

% Check https://www.mathworks.com/help/matlab/ref/audioread.html

filename = "Scorpions_sine.wav";
[original_data,fs] = audioread(filename);
ts = 1/fs;
duration = length(original_data)*ts;
t = 0:ts:duration-ts;
figure(1)
hold on
plot(t,original_data,"red")
grid on
title("Original Song")
xlabel("t[s]")
ylabel("A")
hold off

song_freq = fftshift(abs(fft(original_data)));
freq = linspace(-fs/2,fs/2,length(song_freq));

figure(2)
hold on
grid on
plot(freq,song_freq,"blue");
title("Original song")
xlabel("f[Hz]");
ylabel("Mg");
hold off


f_sinus = 7;
delta = 100;
f_taiere = f_sinus-delta;
f_taiere2 = f_sinus+delta
N = 1001; %ordin filtru
n = (-N-1)/2 : (N-1)/2;
h_n_ob = sinc(n) - (2*f_taiere2/fs)*sinc(n*2*f_taiere2/fs) +(2*f_taiere/fs)*sinc(n*2*f_taiere/fs);


figure(3)
hold on
grid on
plot(n,h_n_ob,"blue");
title("Filter")
xlabel("n");
ylabel("A");
hold off

new_song = conv(original_data,h_n_ob,"same"); % de ce ordinea variabilelor conteaza??
t2 = 0:ts:(length(new_song)/fs)-ts;
figure(4)
hold on
grid on
plot(t2,new_song,"blue");
title("Edited song")
xlabel("t[s]");
ylabel("A");
hold off

new_song_freq = fftshift(abs(fft(new_song)));
freq = linspace(-fs/2,fs/2,length(new_song_freq));

figure(5)
hold on
grid on
plot(freq,new_song_freq,"red");
title("Edited song")
xlabel("f[Hz]");
ylabel("Mg");
hold off
%sound(new_song, fs);

