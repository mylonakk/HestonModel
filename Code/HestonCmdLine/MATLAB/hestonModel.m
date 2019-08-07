% Model Vars
clear
global r alpha theta kappa sigma rho v S K b u
r = 0.025;
theta = 0.0398;
kappa = 1.5768;
sigma = 0.5751;
rho = -0.5711;
v = 0.0175;
S = 100;
K = 100;
T = 1;

b = [kappa - rho*sigma, kappa];
u = [0.5, -0.5];
alpha = kappa * theta;

P1 = @(phi) real((exp(-phi*log(K)*1i) .* Phi(1, T, log(S), phi)) ./ (phi*1i));
P2 = @(phi) real((exp(-phi*log(K)*1i) .* Phi(2, T, log(S), phi)) ./ (phi*1i));
price = S* (0.5 + integral(P1,0,inf) / pi) - ...
        K*exp(-r*T)*(0.5 + integral(P2,0,inf) / pi );
    
goldPrice = optByHestonNI(r,100,0,365,'call',100,v,theta,kappa,sigma,rho, 'basis', 3);

%error = abs(goldPrice - price);

% Eqs
function res = d(j, phi)
    global sigma rho u b
    res = sqrt((rho*sigma*phi*1i - b(j)).^2 - sigma.^2*(2*u(j)*phi*1i - phi.^2));
end

function res = g(j, phi)
    global sigma rho b
    res = (b(j) - rho*sigma*phi*1i - d(j, phi)) ./ ...
          (b(j) - rho*sigma*phi*1i + d(j, phi));
end

function res = C(j, tau, phi)
    global r alpha sigma rho b
    res = r * phi * tau * 1i + (alpha / sigma^2) * ...
          ( (b(j) - rho*sigma*phi*1i - d(j, phi)) *tau - ...
          2 * log( (1 - g(j, phi).*exp(-tau*d(j, phi))) ./ (1 - g(j, phi))) ); 
end

function res = D(j, tau, phi)
    global sigma rho b 
    res = (b(j) - rho*sigma*phi*1i - d(j, phi)) .* ...
          ( (1 - exp(-tau*d(j, phi)) ) ./ (1 - g(j, phi).*exp(-tau*d(j, phi)))) ./ sigma^2;
end

function res = Phi(j, tau, x, phi)
    global v
    res = exp(C(j, tau, phi) + D(j, tau, phi)*v + phi*x*1i);
end