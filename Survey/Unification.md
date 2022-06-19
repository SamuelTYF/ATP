# Unification

Giving two trees $t_1$ , $t_2$, we want to find a substitution $\sigma$, such that 
$$
\sigma[t_1]=\sigma[t_2]
$$

## Most General Unifier

$$
\sigma(B_1)=...=\sigma(B_m)=\sigma(C_1)=...=\sigma(C_n)
$$

## Unification Example

$$
tree_1=f(x,f(x,y))
$$

$$
tree_2=f(g(y),f(g(a),z))
$$

$$
\sigma=\{(g(a)/x,a/y,a/z)\}
$$

$$
tree=f(g(a),f(g(a),a))
$$

# Ground Resolution

| From | $\{\lnot P(x_0,c_0),\lnot P(x_0,x_1),\lnot P(x_1,x_0)\}$ | $\{P(x_0,S_2(x_0,x_1)),P(x_0,c_0)\}$ | $\{P(S_2(x_0,x_1),x_0),P(x_0,c_0)\}$ | $P(c_0,S_2(c_0,c_0))$ | $P(S_2(c_0,c_0),c_0)$ | $\{\lnot P(c_0,c_0),\lnot P(S_2(c_0,c_0),c_0)\}$ | $\lnot P(c_0,S_2(c_0,c_0))$ |
| :-: | :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| $\{\lnot P(x_0,c_0),\lnot P(x_0,x_1),\lnot P(x_1,x_0)\}$ |  |  |  |  |  |  |  |
| $\{P(x_0,S_2(x_0,x_1)),P(x_0,c_0)\}$ | $$\{\lnot P(c_0,c_0),P(c_0,c_0)\}\qquad P(c_0,S_2(c_0,c_0))$$ |  |  |  |  |  |  |
| $\{P(S_2(x_0,x_1),x_0),P(x_0,c_0)\}$ | $$\{\lnot P(c_0,c_0),P(c_0,c_0)\}\qquad P(S_2(c_0,c_0),c_0)$$ |  |  |  |  |  |  |
| $P(c_0,S_2(c_0,c_0))$ | $$\{\lnot P(c_0,S_2(c_0,c_0)),P(c_0,S_2(c_0,c_0))\}\qquad\{\lnot P(c_0,c_0),\lnot P(S_2(c_0,c_0),c_0)\}$$ |  |  |  |  |  |  |
| $P(S_2(c_0,c_0),c_0)$ | $$\{\lnot P(S_2(c_0,c_0),c_0),P(S_2(c_0,c_0),c_0)\}\qquad\lnot P(c_0,S_2(c_0,c_0))$$ |  |  |  |  |  |  |
| $\{\lnot P(c_0,c_0),\lnot P(S_2(c_0,c_0),c_0)\}$ |  | $$\{P(c_0,c_0),\lnot P(c_0,c_0)\}\qquad\{\lnot P(S_2(c_0,c_0),c_0),P(c_0,S_2(c_0,x_1))\}$$ | $$\{P(c_0,c_0),\lnot P(c_0,c_0)\}\qquad\{\lnot P(S_2(c_0,c_0),c_0),P(S_2(c_0,x_1),c_0)\}$$ |  | $$\{P(S_2(c_0,c_0),c_0),\lnot P(S_2(c_0,c_0),c_0)\}\qquad\lnot P(c_0,c_0)$$ |  |  |
| $\lnot P(c_0,S_2(c_0,c_0))$ |  | $$\{P(c_0,S_2(c_0,c_0)),\lnot P(c_0,S_2(c_0,c_0))\}\qquad P(c_0,c_0)$$ |  | $$\{P(c_0,S_2(c_0,c_0)),\lnot P(c_0,S_2(c_0,c_0))\}\qquad\{\}$$ |  |  |  |