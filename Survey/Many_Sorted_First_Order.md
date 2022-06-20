# Many-Sorted First-Order Languages

A countable set $S\cup\{bool\}$ of sorts containing the special sort *bool*

For every sort $s\in S$, a countably infinite set $V_s=\{x_1,x_2,...\}$, each variable $x_i$ being of rank $(e,s)$, $V=\Cap_s V_s$

An $S\cup\{bool\}$-ranked alphabet **L** of nonlogical symbols consisting of:

1. Function symbols: A set **FS** if symbols $f_0,f_1,...$ and a rank function $r:FS\to S^+\times S$, assigning a pair $r(f)=(u,s)$

2. Constants: For every sort $s\in S$, a set $CS_s$ if symbols $c_0,c_1,...$ each of rank $(e,s)$. $CS=\Cap_s CS_s$

3. Predicate symbols: A set **PS** of symbols $P_0,P_1,...$ and a rank function $r:PS\to S^*\times\{bool\}$, assiging a pair $r(P)=(u,bool)$ called rank to each predicate symbol P. If $u=e$, P is a propositional letter.

## Definition

**$\Gamma$** be the union of the sets $V$,$CS$,$FS$,$PS$ and $\{false\}$

**$\Gamma_s$** be the subset of $\Gamma^+$

## Substitution

In a substitution $s[t/x]$ of a term t for a variable x, the sort of the term t must be equal to the sort of variable x.

> Different from OOA
