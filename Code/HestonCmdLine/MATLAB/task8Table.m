clear
% Load csv
load('task8.csv');
% Create Latex table
latex_table = latex(vpa(sym(task8),4));