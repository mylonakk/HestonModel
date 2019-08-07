clear
T = 1; % maturity
N = ceil(364 * T); % time steps
MCrep = 1e5; % MC number of paths
S = 100;
K = 100;
r = 0.1;
v = 0.04;
sigma = 0.4;
rho = 0.5;
kappa = 2;
theta = 0.06;

% Feller Condition
if 2*kappa*theta <= sigma^2
    error('Feller Condition not satisfied.');
end

% MC constants
alpha = (4*kappa*theta - sigma^2) / 8;
beta = - kappa / 2;
gamma = sigma / 2;

tau = T / N;

C = 0;
for i = 1 : MCrep
    s = zeros(N, 1);
    s(1) = S;
    y = zeros(N, 1);
    y(1) = sqrt(v);
    
    x1 = normrnd(0, 1,N, 1);
    x2 = normrnd(0, 1,N, 1);
    
    for j = 2 : N
        dz1 = sqrt(tau) * x1(j);
        dz2 = sqrt(tau) * (rho * x1(j) + sqrt(1 - rho^2) * x2(j));
        y(j) = (y(j-1) + gamma * dz2) / (2 - 2*beta*tau) + ...
               sqrt( (y(j-1) + gamma * dz2)^2 / (4*(1 - beta*tau)^2) + ...
               alpha * tau / (1 - beta*tau) );
        s(j) = s(j - 1) + r * s(j-1) * tau + y(j-1)*s(j-1)*dz1;
    end
    
    C = C + max(s(end) - K, 0);
end

price = exp(-r*T) * C / MCrep;
goldPrice = optByHestonNI(r,100,0,365,'call',100,v,theta,kappa,sigma,rho, 'basis', 3);

error = abs(goldPrice - price)



