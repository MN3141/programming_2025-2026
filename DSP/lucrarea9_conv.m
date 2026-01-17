clc
close all
clear

x_n = [3,3,3,6,6,6,9,9,9];
h_n = [1/3,1/3,1/3];
%h_n = [1,-1];

y_n = convolutie(x_n,h_n);
y_n2 = conv(x_n,h_n,"valid");
%n_y = length(y_n);
figure(1)
hold on
plot(y_n,"red")
grid on
title("Produs Convolutie")
xlabel("n")
ylabel("M")
hold off

figure(2)
hold on
plot(y_n2,"blue")
grid on
title("Produs Convolutie")
xlabel("n")
ylabel("M")
hold off
