% Option Price parametric K, S
figure();
subplot(1, 2, 1);
[rho2, v2] = meshgrid(linspace(-4, 4, n), linspace(0, 8, n));
Z = zeros(n, n, n);
for i = 1 : n
    for rho_i = 1 : n
        for v_i = 1 : n
            Z(rho_i, v_i, i) = upperBound(phi2(i), v2(v_i), r1, sigma1, rho2(rho_i), kappa1, theta1, K1, S1, T1);
        end
    end
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
surf(rho2, v2, Z2);

title('Upper Bound Parametric - 1e-3');
xlabel('Rho');
ylabel('V');
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
surf(rho2, v2, Z2);

title('Upper Bound Parametric - 1e-5');
xlabel('Rho');
ylabel('V');
zlabel('Upper bound Index');
