# SLD_Resolution

## For PL

- Goal: $\{\lnot P_1,...,\lnot P_n\}$
- Horns: $\{\lnot P_1,...,\lnot P_n,Q\}$
  
1. Have one Goal
    - split $\Sigma,\{\lnot P_1,...,\lnot P_n\}$ into $\Sigma,\lnot P_1$ and $\Sigma,\{\lnot P_2,...,\lnot P_n\}$
2. Hove no Goal
    - choice one Horn $\{\lnot P_1,...,\lnot P_n,Q\}$
    - split $\Sigma,\{\lnot P_1,...,\lnot P_n,Q\}$ into $\Sigma,\lnot Q$ and $\Sigma,\{\lnot P_1,...,\lnot P_n\}$

$$
\cfrac{\color{green}p_3,p_4,\{\lnot p_3,\lnot p_4,p_1\},\{\lnot p_3,p_2\},\{\lnot p_1,\lnot p_2\}\to}{\cfrac{\color{green}p_3,p_4,\lnot p_1,\{\lnot p_3,\lnot p_4,p_1\},\{\lnot p_3,p_2\}\to}{\color{green}p_3,p_4,p_1,\lnot p_1,\{\lnot p_3,p_2\}\to \qquad \color{green}p_3,p_4,\lnot p_1,\{\lnot p_3,p_2\},\{\lnot p_3,\lnot p_4\}\to} \qquad \cfrac{\color{green}p_3,p_4,\lnot p_2,\{\lnot p_3,\lnot p_4,p_1\},\{\lnot p_3,p_2\}\to}{\cfrac{\color{green}p_3,p_4,p_1,\lnot p_2,\{\lnot p_3,p_2\}\to}{\color{green}p_3,p_4,p_1,p_2,\lnot p_2\to \qquad \color{green}p_3,p_4,p_1,\lnot p_2,\lnot p_3\to} \qquad \color{green}p_3,p_4,\lnot p_2,\{\lnot p_3,p_2\},\{\lnot p_3,\lnot p_4\}\to}}
$$