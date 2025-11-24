function y = convolutie(x,h)
n_x = length(x);
n_h = length(h);

h_extins = [zeros(1,n_x) , fliplr(h),zeros(1,n_x)];

for i=0:1:n_x+n_h-2
    esantion_start = n_x + n_h - i;
    esantion_stop = esantion_start + n_x -1;
    h_selectat = h_extins(esantion_start:esantion_stop);
    y(i+1) = sum(x.*h_selectat);
end