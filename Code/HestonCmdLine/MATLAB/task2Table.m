clear
% Load csv
load('task2.csv');
% Create Latex table
latex_table = latex(vpa(sym(task2),4));