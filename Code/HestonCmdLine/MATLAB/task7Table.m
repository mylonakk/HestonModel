clear
% Load csv
load('task7.csv');
% Create Latex table
latex_table = latex(vpa(sym(task7),4));