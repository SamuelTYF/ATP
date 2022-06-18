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