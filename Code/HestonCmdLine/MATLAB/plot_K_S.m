% Option Price parametric K, S
figure();
subplot(1, 2, 1);
[K2, S2] = meshgrid(linspace(60, 120, n));
Z = zeros(length(K2), length(S2), n);
for i = 1 : n
    Z(:, :, i) = upperBound(phi2(i), v1, r1, sigma1, rho1, kappa1, theta1, K2, S2, T1);
end
Z2 = zeros(length(K2), length(S2));
for i = 1 : length(K2)
    for j = 1 : length(S2)
        index = find(Z(i, j, :) > 1e-3, 1, 'last');
        if isempty(index)
            Z2(i, j) = 0;
            continue;
        end
        Z2(i, j) = phi2(index);
    end
end
surf(K2, S2, Z2);

title('Upper Bound Parametric - 1e-3');
xlabel('Strike Price');
ylabel('Initial Stock Price');
zlabel('Upper bound Index');

subplot(1, 2, 2);
[K2, S2] = meshgrid(linspace(80, 120, n));
Z2 = zeros(length(K2), length(S2));
for i = 1 : length(K2)
    for j = 1 : length(S2)
        index = find(Z(i, j, :) > 1e-5, 1, 'last');
        if isempty(index)
            Z2(i, j) = 0;
            continue;
        end
        Z2(i, j) = phi2(index);
    end
end
surf(K2, S2, Z2);

title('Upper Bound Parametric - 1e-5');
xlabel('Strike Price');
ylabel('Initial Stock Price');
zlabel('Upper bound Index');
axis equal