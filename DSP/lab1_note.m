clc
close all
clear

note=  'ssDDssDssDDsfmsfmmrrmsfmmrrd';
durata='8888884888888488888848888884';
fs = 16000;
n_x = length(note);
t = []
cantec = []

for k=1:n_x
    durata_integer = str2num(durata(k));
    durata_k = 1/durata_integer;
    t_k = 0:1/fs:durata_k;
    t = [t t_k];
    switch(note(k))
        case "d"
           p_k=-9;
        case "r"
            p_k=-7;
        case "m"
            p_k=-5;
        case "f"
            p_k=-4;
        case "s"
            p_k=-2;
        case "l"
            p_k = 0;
        case "t"
            p_k = 2;
        case "D"
            p_k = 3;
        case "S"
            p_k = -14;
        otherwise
            p_k =0;
    end
    freq_k = 1.05946 ^ p_k *440;
    nota_k = sin(2*pi*freq_k*t_k);

    cantec = [cantec fade_in_out(nota_k,10,10)];
    
end
figure(1)
hold on
plot(t,cantec,"red")
grid on
title("Cantec")
xlabel("t[s]")
ylabel("Nota")
hold off