# Formal System

$$
\mathcal{L}=\mathcal{L}(A,\Omega,Z,I)
$$

where:
- The alpha set $A$ is a countably infinite set of elements called proposition symbols or propositional variables.
- $\Omega$ is a finite set of elements called operator symbols or logical connectives.
  - $\Omega_0=\{\bot,\top\}$
  - $\Omega_1=\{\lnot\}$
  - $\Omega_2=\{\vee,\wedge,\leftarrow,\leftrightarrow\}$
  - $\Omega_{i}$ is the set of operator symbols of arity i.
- $Z$ is a finite set of axioms, or axiom schemata, consisting of well-formed formulas that are called inference rules when they acquire logical applications.
- $I$ is a countable set of initial points that are called axioms when they receive logical interpretations. A well-formed formula that can be inferred from the axioms is known as a theorem of the formal system.

## Formulas in Formal System

$a \in A\Rightarrow a$ is a formula of $\mathcal{L}$

$f\in \Omega_i\vee p_j (j\in \{1,2,...,i\})$ is a formular of $\mathcal{L}\Rightarrow f(p_1,p_2,...,p_i)$ is a formula of $\mathcal{L}$

