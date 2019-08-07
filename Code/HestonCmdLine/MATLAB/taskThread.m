clear
% Load csv
load('taskThreadScaling.csv');

% Time plot
figure()
plot(taskThreadScaling(:, 2), taskThreadScaling(:, 3) / 1000, 'x-')
title('MC Option Pricing Performance')
ylabel('Execution Time (s)');
xlabel('Number of threads');

% Speed up plot
figure()
plot(taskThreadScaling(:, 2), taskThreadScaling(1, 3) ./ taskThreadScaling(:, 3), 'x-')
title('MC Option Pricing Spead up')
ylabel('Speed up');
xlabel('Number of threads');