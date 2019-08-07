clear
% Load csv
load('task3.csv');
% Create Latex table
latex_table = latex(vpa(sym(task3(:, 1:3)),4));