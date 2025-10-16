function [y] = fade_in_out(x, percent_fade_in, percent_fade_out)
%foo
% n_in = length(x)*percent_fade_in/100;
% n_out = length(x)*percent_fade_out/100;
% x_fade_out = length(x) - n_out;

n_x = length(x);
n_in = round(percent_fade_in*n_x/100);
n_out = round(percent_fade_out*n_x/100);

x1 = x(1:n_in);
x2 = x(n_in+1: n_x-n_out);
x3 = x(n_x-n_out+1:n_x);

step1 = 1/(n_in-1);
step2 = -1/(n_out-1);
a1 = 0:step1:1;
a2 = 1:step2:0;

y1 = a1.*x1;
y2 = x2;
y3 = a2.*x3;

y = [y1 y2 y3];


