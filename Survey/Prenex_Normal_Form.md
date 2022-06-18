# Prenex Normal Form

Every formula in classical logic is equivalent to a formula in prenex normal form.

$$
Q_1x_1Q_2x_2...Q_nx_nB
$$

$$
Q_i\in\{\forall,\exist\}
$$

$B$ is a formula without quantifiers

## Quivalent

### $A=B\vee C$

$$B\equiv Q_1x_1Q_2x_2...Q_mx_mB'$$

$$C\equiv R_1y_1R_2y_2...R_ny_nC'$$

$$A\equiv Q_1x_1Q_2x_2...Q_mx_mR_1y_1R_2y_2...R_ny_n(B'\vee C')$$

### $A=\lnot B$

$$B\equiv Q_1x_1Q_2x_2...Q_mx_mB'$$

$$A\equiv R_1x_1R_2x_2...R_mx_m\lnot B'$$

$$
R_i=\begin{cases}
\forall&Q_i=\exist\\
\exist&Q_i=\forall
\end{cases}
$$

### $A=\forall x,B$

$$B\equiv Q_1x_1Q_2x_2...Q_mx_mB'$$

$$A\equiv \forall xQ_1x_1Q_2x_2...Q_mx_mB'$$

$$
\cfrac{\color{black} \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1)))) \vee \lnot (P(y) \wedge \lnot (\exist x_2(P(x_2))))}{\cfrac{\color{black} \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1)))),\lnot (P(y) \wedge \lnot (\exist x_2(P(x_2))))}{\cfrac{\color{black}P(y) \wedge \lnot (\exist x_2(P(x_2))) \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1))))}{\cfrac{\color{black}P(y),\lnot (\exist x_2(P(x_2))) \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1))))}{\cfrac{\color{black}P(y) \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1)))),\exist x_2(P(x_2))}{\cfrac{\color{black}P(y) \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1)))),\exist x_2(P(x_2)),P(t_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(t_0,x_1)))}{\cfrac{\color{black}P(y) \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1)))),\exist x_2(P(x_2)),P(t_0),\lnot (\exist x_1(Q(x_1) \wedge R(t_0,x_1)))}{\cfrac{\color{black}P(y),\exist x_1(Q(x_1) \wedge R(t_0,x_1)) \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1)))),\exist x_2(P(x_2)),P(t_0)}{\cfrac{\color{black}P(y),Q(t_1) \wedge R(t_0,t_1) \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1)))),\exist x_2(P(x_2)),P(t_0)}{\cfrac{\color{black}P(y),Q(t_1),R(t_0,t_1) \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1)))),\exist x_2(P(x_2)),P(t_0)}{\cfrac{\color{black}P(y),Q(t_1),R(t_0,t_1) \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1)))),\exist x_2(P(x_2)),P(t_0),P(t_1) \vee \lnot (\exist x_1(Q(x_1) \wedge R(t_1,x_1)))}{\cfrac{\color{black}P(y),Q(t_1),R(t_0,t_1) \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1)))),\exist x_2(P(x_2)),P(t_0),P(t_1),\lnot (\exist x_1(Q(x_1) \wedge R(t_1,x_1)))}{\cfrac{\color{black}P(y),Q(t_1),R(t_0,t_1),\exist x_1(Q(x_1) \wedge R(t_1,x_1)) \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1)))),\exist x_2(P(x_2)),P(t_0),P(t_1)}{\cfrac{\color{black}P(y),Q(t_1),R(t_0,t_1),Q(t_2) \wedge R(t_1,t_2) \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1)))),\exist x_2(P(x_2)),P(t_0),P(t_1)}{\cfrac{\color{black}P(y),Q(t_1),R(t_0,t_1),Q(t_2),R(t_1,t_2) \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1)))),\exist x_2(P(x_2)),P(t_0),P(t_1)}{\cfrac{\color{black}P(y),Q(t_1),R(t_0,t_1),Q(t_2),R(t_1,t_2) \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1)))),\exist x_2(P(x_2)),P(t_0),P(t_1),P(t_2) \vee \lnot (\exist x_1(Q(x_1) \wedge R(t_2,x_1)))}{\cfrac{\color{black}P(y),Q(t_1),R(t_0,t_1),Q(t_2),R(t_1,t_2) \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1)))),\exist x_2(P(x_2)),P(t_0),P(t_1),P(t_2),\lnot (\exist x_1(Q(x_1) \wedge R(t_2,x_1)))}{\cfrac{\color{black}P(y),Q(t_1),R(t_0,t_1),Q(t_2),R(t_1,t_2),\exist x_1(Q(x_1) \wedge R(t_2,x_1)) \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1)))),\exist x_2(P(x_2)),P(t_0),P(t_1),P(t_2)}{\cfrac{\color{black}P(y),Q(t_1),R(t_0,t_1),Q(t_2),R(t_1,t_2),Q(t_3) \wedge R(t_2,t_3) \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1)))),\exist x_2(P(x_2)),P(t_0),P(t_1),P(t_2)}{\cfrac{\color{black}P(y),Q(t_1),R(t_0,t_1),Q(t_2),R(t_1,t_2),Q(t_3),R(t_2,t_3) \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1)))),\exist x_2(P(x_2)),P(t_0),P(t_1),P(t_2)}{\color{black}P(y),Q(t_1),R(t_0,t_1),Q(t_2),R(t_1,t_2),Q(t_3),R(t_2,t_3) \vdash \exist x_0(P(x_0) \vee \lnot (\exist x_1(Q(x_1) \wedge R(x_0,x_1)))),\exist x_2(P(x_2)),P(t_0),P(t_1),P(t_2),P(t_3) \vee \lnot (\exist x_1(Q(x_1) \wedge R(t_3,x_1)))}}}}}}}}}}}}}}}}}}}}
$$

## Issues

Infinit Recursion