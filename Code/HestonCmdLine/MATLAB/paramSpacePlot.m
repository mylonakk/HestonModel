clear
close all
n = 100;
rng = 1:n;
phi2 = linspace(0, 2000, n);
% Default Heston params
r1 = 0.025;
theta1 = 0.0398;
kappa1 = 1.5768;
sigma1 = 0.5751;
rho1 = -0.5711;
v1 = 0.0175;
S1 = 100;
K1 = 100;
T1 = 1;
% Set to 1 to enable validation with matlab build in function
validation = 0;

P1 = @(phi, v, r, sigma, rho, kappa, theta, K, S, T) real((exp(-phi.*log(K).*1i) .* Phi(1, T, log(S), phi, v, r, sigma, rho, kappa, theta)) ./ (phi.*1i));
P2 = @(phi, v, r, sigma, rho, kappa, theta, K, S, T) real((exp(-phi.*log(K).*1i) .* Phi(2, T, log(S), phi, v, r, sigma, rho, kappa, theta)) ./ (phi.*1i));

upperBound = @(phi, v, r, sigma, rho, kappa, theta, K, S, T) P1(phi, v, r, sigma, rho, kappa, theta, K, S, T);

% Plot parametric for K and S
plot_K_S;
% Plot parametric for kappa and theta
plot_kappa_theta;
% Plot parametric for Rho and V
plot_Rho_V;
% plot parametric for Rho and r
plot_Rho_r


if validation
    % Validation
    
    P1Int = @(phi) P1(phi, v1, r1, sigma1, rho1, kappa1, theta1, K1, S1, T1);
    P2Int = @(phi) P2(phi, v1, r1, sigma1, rho1, kappa1, theta1, K1, S1, T1);

    price = S1* (0.5 + integral(P1Int,0,inf) / pi) - ...
            K1*exp(-r1*T1)*(0.5 + integral(P2Int,0,inf) / pi );

    goldPrice = optByHestonNI(r1,S1,0,365,'call',K1,v1,theta1,kappa1,sigma1,rho1, 'basis', 3);

    error = abs(goldPrice - price);
    fprintf('Abs error %e \n', error);
end

% Eqs
function res = d(j, phi, sigma, rho, b)
    u = [0.5, -0.5];
    res = sqrt((rho*sigma*phi*1i - b(j)).^2 - sigma.^2*(2*u(j)*phi*1i - phi.^2));
end

function res = g(j, phi, sigma, rho, b)
    res = (b(j) - rho*sigma*phi*1i - d(j, phi, sigma, rho, b)) ./ ...
          (b(j) - rho*sigma*phi*1i + d(j, phi, sigma, rho, b));
end

function res = C(j, tau, phi, r, alpha, sigma, rho, b)
    res = r * phi * tau * 1i + (alpha / sigma^2) * ...
          ( (b(j) - rho*sigma*phi*1i - d(j, phi, sigma, rho, b)) *tau - ...
          2 * log( (1 - g(j, phi, sigma, rho, b).*exp(-tau*d(j, phi, sigma, rho, b))) ./ (1 - g(j, phi, sigma, rho, b))) ); 
end

function res = D(j, tau, phi, sigma, rho, b)
    res = (b(j) - rho*sigma*phi*1i - d(j, phi, sigma, rho, b)) .* ...
          ( (1 - exp(-tau*d(j, phi, sigma, rho, b)) ) ./ (1 - g(j, phi, sigma, rho, b).*exp(-tau*d(j, phi, sigma, rho, b)))) ./ sigma^2;
end

function res = Phi(j, tau, x, phi, v, r, sigma, rho, kappa, theta)
    b = [kappa - rho.*sigma, kappa];
    alpha = kappa .* theta;

    res = exp(C(j, tau, phi, r, alpha, sigma, rho, b) + D(j, tau, phi, sigma, rho, b)*v + phi*x*1i);
end