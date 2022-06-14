# Resolution

In mathematical logic and automated theorem proving, resolution is a rule of inference leading to a refutation complete theorem-proving technique for sentences in propositional logic and first-order logic.

#

$$
\cfrac{\color{green} \vdash ((A \vee P) \wedge (B \vee \lnot P)) \to (A \vee B)}{\cfrac{\color{green}(A \vee P) \wedge (B \vee \lnot P) \vdash A \vee B}{\cfrac{\color{green}A \vee P,B \vee \lnot P \vdash A \vee B}{\cfrac{\color{green}A \vee P,B \vee \lnot P \vdash A,B}{\color{green}A,B \vee \lnot P \vdash A,B \qquad \cfrac{\color{green}P,B \vee \lnot P \vdash A,B}{\color{green}P,B \vdash A,B \qquad \cfrac{\color{green}P,\lnot P \vdash A,B}{\color{green}P \vdash A,B,P}}}}}}
$$

## Clause

$$
C=(C_1-\{L\})\cup(C_2-\{\lnot L\})
$$

$L$ is a Literal such that $L\in C_1,\lnot L\in C_2$

$C$ is satisified iff $C_1$ and $C_2$ are both satisified.

## Resolution DAG

$$
S=\{C_1,C_2,...,C_n\}
$$

$$
D=\{(t_1,R_1),...,(t_m,R_m)\}
$$
