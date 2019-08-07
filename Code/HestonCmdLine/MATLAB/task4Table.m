clear
% Load csv
load('task3.csv');
% compute rel error
relError = abs(task3(:,3) - task3(:,4)) ./ task3(:,4);
% Create Latex table
latex_table1 = latex(vpa([sym(task3) relError],5));

load('task4.csv');
% Convergence plot (log - log)
figure
hold on
trials1 = task4(find(task4(:, 2) == 183), 1);
error1 = task4(find(task4(:, 2) == 183), 3);
plot(log10(trials1), log10(error1))
trials2 = task4(find(task4(:, 2) == 365), 1);
error2 = task4(find(task4(:, 2) == 365), 3);
plot(log10(trials2), log10(error2))
trials3 = task4(find(task4(:, 2) == 730), 1);
error3 = task4(find(task4(:, 2) == 730), 3);
plot(log10(trials3), log10(error3))
title('Convergence Plot')
xlabel('log(Number Of Trials)')
ylabel('log(relative error)')
legend('183 time steps per year', '365 time steps per year', '730 time steps per year')
axis tight