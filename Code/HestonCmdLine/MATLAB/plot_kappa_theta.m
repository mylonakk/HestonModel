% Option Price parametric K, S
figure();
subplot(1, 2, 1);
[kappa2, theta2] = meshgrid(linspace(0, 4, n));
Z = zeros(length(kappa2), length(theta2), n);
for i = 1 : n
    Z(:, :, i) = upperBound(phi2(i), v1, r1, sigma1, rho1, kappa2, theta2, K1, S1, T1);
end
Z2 = zeros(n, n);
for i = 1 : n
    for j = 1 : n
        index = find(Z(i, j, :) > 1e-3, 1, 'last');
        if isempty(index)
            Z2(i, j) = 0;
            continue;
        end
        Z2(i, j) = phi2(index);
    end
end
surf(kappa2, theta2, Z2);

title('Upper Bound Parametric - 1e-3');
xlabel('Kappa');
ylabel('Theta');
zlabel('Upper bound Index');

subplot(1, 2, 2);
Z2 = zeros(n, n);
for i = 1 : n
    for j = 1 : n
        index = find(Z(i, j, :) > 1e-5, 1, 'last');
        if isempty(index)
            Z2(i, j) = 0;
            continue;
        end
        Z2(i, j) = phi2(index);
    end
end
surf(kappa2, theta2, Z2);

title('Upper Bound Parametric - 1e-5');
xlabel('Kappa');
ylabel('Theta');
zlabel('Upper bound Index');