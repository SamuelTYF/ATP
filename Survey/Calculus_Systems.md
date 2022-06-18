# Calculus Systems

## Natural Deduction System

$$
A_1,A_2,...,A_n\vdash B
$$

$$
\vdash (A_1\wedge A_2\wedge...\wedge A_n)\to B
$$

## Sequent Calculus System (also Gentzen Systems)

$$
A_1,A_2,...,A_n\vdash B_1,B_2,...,B_m
$$

Every $A_i$ is true and at least one $B_j$ is true

$$
\vdash (A_1\wedge A_2\wedge...\wedge A_n)\to (B_1\vee B_2\vee...\vee B_m)
$$

$$
\vdash \lnot A_1\vee \lnot A_2\vee...\vee\lnot A_n\vee B_1\vee B_2\vee...\vee B_m
$$

### Reduction Tree

Prove:$((p\rightarrow r)\vee (q\rightarrow r))\rightarrow ((p\wedge q)\rightarrow r)$

![avater](https://upload.wikimedia.org/wikipedia/commons/0/0d/Sequent_calculus_proof_tree_example.png)

|                                                   Left                                                    |                                                Right                                                |
| :-------------------------------------------------------------------------------------------------------: | :-------------------------------------------------------------------------------------------------: |
|                 $$L\wedge:\frac{\Gamma,A\wedge B\vdash \Delta}{\Gamma,A,B\vdash \Delta}$$                 | $$R\wedge:\frac{\Gamma\vdash \Delta,A\wedge B}{\Gamma\vdash \Delta,A\qquad \Gamma\vdash \Delta,B}$$ |
|        $$L\vee:\frac{\Gamma,A\vee B\vdash\Delta}{\Gamma,A\vdash\Delta\qquad\Gamma,B\vdash\Delta}$$        |             $$R\vee:\frac {\Gamma \vdash \Delta ,A\vee B}{\Gamma \vdash \Delta ,A,B}$$              |
| $$L\rightarrow:\frac{\Gamma,A\rightarrow B\vdash\Delta}{\Gamma\vdash\Delta,A\qquad\Gamma,B\vdash\Delta}$$ |          $$R\rightarrow:\frac{\Gamma\vdash\Delta,A\rightarrow B}{\Gamma,A\vdash\Delta,B}$$          |
|                    $$L\lnot:\frac{\Gamma,\lnot A\vdash\Delta}{\Gamma\vdash\Delta,A}$$                     |                $$R\lnot:\frac{\Gamma \vdash\Delta,\lnot A}{\Gamma,A\vdash\Delta }$$                 |

Axioms:$\Gamma,A\vdash\Delta,A$

### LK

The Sequent Calculus LK was introduced by Gentzen in 1934.

- $A$ and $B$ denote formulae of first-order predicate logic
- $\Gamma$,$\Delta$,$\Sigma$,$\Pi$ are finite sequences of formulae, called contexts
- $t$ denotes an arbitrary term
- $x$ and $y$ denote variables
- a variable is said to occur free within a formular if it is not bound by quantifiers $\forall$,$\exist$
- $A[t/x]$ denotes the formula that is obtained by substituting the term $t$ for every free occurrence of the variable $x$ in formula $A$ with the restriction that the term $t$ must be free for the variable $x$ in $A$

|                  Axiom                   |                                                       Cut                                                        |
| :--------------------------------------: | :--------------------------------------------------------------------------------------------------------------: |
| $$\frac{\qquad }{ A\vdash A}\quad (I) $$ | $$   \frac{\Gamma\vdash\Delta, A\qquad A,\Sigma\vdash\Pi} {\Gamma,\Sigma\vdash\Delta,\Pi}\quad (\mathit{Cut}) $$ |

|                                                        Left Logical Rules:                                                        |                                                      Right Logical Rules:                                                      |
| :-------------------------------------------------------------------------------------------------------------------------------: | :----------------------------------------------------------------------------------------------------------------------------: |
|                   $${ {\frac {\Gamma ,A\vdash\Delta }{\Gamma ,A\wedge B\vdash\Delta }}\quad ({\wedge }L_{1})}$$                   |                   $${ {\frac {\Gamma\vdash A,\Delta }{\Gamma\vdash A\vee B,\Delta }}\quad ({\vee }R_{1})}$$                    |
|                   $${ {\frac {\Gamma ,B\vdash\Delta }{\Gamma ,A\wedge B\vdash\Delta }}\quad ({\wedge }L_{2})}$$                   |                   $${ {\frac {\Gamma\vdash B,\Delta }{\Gamma\vdash A\vee B,\Delta }}\quad ({\vee }R_{2})}$$                    |
|    $${ {\frac {\Gamma ,A\vdash\Delta\qquad\Sigma ,B\vdash\Pi }{\Gamma ,\Sigma ,A\vee B\vdash\Delta ,\Pi }}\quad ({\vee }L)}$$     | $${ {\frac {\Gamma\vdash A,\Delta\qquad\Sigma\vdash B,\Pi }{\Gamma ,\Sigma\vdash A\wedge B,\Delta ,\Pi }}\quad ({\wedge }R)}$$ |
| $$ \frac{\Gamma\vdash A,\Delta\qquad\Sigma, B\vdash\Pi}{\Gamma,\Sigma, A\rightarrow B\vdash\Delta,\Pi}\quad  ({\rightarrow }L) $$ |                $$  \frac{\Gamma, A\vdash B,\Delta}{\Gamma\vdash A\rightarrow B,\Delta}\quad ({\rightarrow}R) $$                |
|                          $$ \frac{\Gamma\vdash A,\Delta}{\Gamma,\lnot A\vdash\Delta}\quad  ({\lnot}L) $$                          |                         $$ \frac{\Gamma, A\vdash\Delta}{\Gamma\vdash\lnot A,\Delta}\quad ({\lnot}R) $$                         |
|                    $$ \frac{\Gamma, A[t/x]\vdash\Delta}{\Gamma,\forall x A\vdash\Delta}\quad  ({\forall}L) $$                     |                  $$ \frac{\Gamma\vdash A[y/x],\Delta}{\Gamma\vdash\forall x A,\Delta}\quad  ({\forall}R)  $$                   |
|                    $$ \frac{\Gamma, A[y/x]\vdash\Delta}{\Gamma,\exists x A\vdash\Delta}\quad  ({\exists}L) $$                     |                   $$ \frac{\Gamma\vdash A[t/x],\Delta}{\Gamma\vdash\exists x A,\Delta}\quad  ({\exists}R) $$                   |

|                                          Left Structural Rules:                                          |                                         Right Structural Rules:                                          |
| :------------------------------------------------------------------------------------------------------: | :------------------------------------------------------------------------------------------------------: |
|                $$ \frac{\Gamma\vdash\Delta}{\Gamma, A\vdash\Delta}\quad (\mathit{WL}) $$                 |                $$ \frac{\Gamma\vdash\Delta}{\Gamma\vdash A,\Delta}\quad (\mathit{WR}) $$                 |
|             $$ \frac{\Gamma, A, A\vdash\Delta}{\Gamma, A\vdash\Delta}\quad (\mathit{CL}) $$              |             $$ \frac{\Gamma\vdash A, A,\Delta}{\Gamma\vdash A,\Delta}\quad (\mathit{CR}) $$              |
| $$ \frac{\Gamma_1, A, B,\Gamma_2\vdash\Delta}{\Gamma_1, B, A,\Gamma_2\vdash\Delta}\quad (\mathit{PL}) $$ | $$ \frac{\Gamma\vdash\Delta_1, A, B,\Delta_2}{\Gamma\vdash\Delta_1, B, A,\Delta_2}\quad (\mathit{PR}) $$ |

#### Cut-Elimination Theorem

$$
\frac{\Gamma\vdash A\qquad\Pi,A\vdash B}{\Gamma,\Pi\vdash B}
$$

# Searching Algorithm

## Breadth-first

Can resolve the situation of infinit sequents

[The Search Procedure](logic_gallier.pdf)

## Depth-first



# Test Result

## Depth-first

$$
\cfrac{\color{green} \vdash Implies[Or[Implies[p,r],Implies[q,r]],Implies[And[p,q],r]]}{\cfrac{\color{green}Or[Implies[p,r],Implies[q,r]] \vdash Implies[And[p,q],r]}{\cfrac{\color{green}Or[Implies[p,r],Implies[q,r]],And[p,q] \vdash r}{\cfrac{\color{green}Or[Implies[p,r],Implies[q,r]],p,q \vdash r}{\cfrac{\color{green}Implies[p,r],p,q \vdash r}{\color{green}p,q \vdash r,p \qquad \color{green}r,p,q \vdash r} \qquad \cfrac{\color{green}Implies[q,r],p,q \vdash r}{\color{green}p,q \vdash r,q \qquad \color{green}r,p,q \vdash r}}}}}
$$

$$\cfrac{\color{red} \vdash Implies[Or[Implies[p,r],Implies[q,r]],Implies[Or[p,q],r]]}{\cfrac{\color{red}Or[Implies[p,r],Implies[q,r]] \vdash Implies[Or[p,q],r]}{\cfrac{\color{red}Or[Implies[p,r],Implies[q,r]],Or[p,q] \vdash r}{\cfrac{\color{red}Implies[p,r],Or[p,q] \vdash r}{\cfrac{\color{red}Or[p,q] \vdash r,p}{\color{green}p \vdash r,p \qquad \color{red}q \vdash r,p} \qquad \color{green}r,Or[p,q] \vdash r} \qquad \color{black}Implies[q,r],Or[p,q] \vdash r}}}
$$

## Fast Test

$$ \cfrac{\color{green} \vdash Implies[Or[Implies[p,r],Implies[q,r]],Implies[And[p,q],r]]}{\cfrac{\color{green}Or[Implies[p,r],Implies[q,r]],p,q \vdash r}{\color{green}r,p,q \vdash r \qquad \color{green}r,p,q \vdash r}}
$$

$$ \cfrac{\color{red} \vdash Implies[Or[Implies[p,r],Implies[q,r]],Implies[Or[p,q],r]]}{\cfrac{\color{red}Or[Implies[p,r],Implies[q,r]],Or[p,q] \vdash r}{\cfrac{\color{red}Or[p,q] \vdash r,p}{\color{green}p \vdash r,p \qquad \color{red}q \vdash r,p} \qquad \color{black}Or[p,q] \vdash r,q}}
$$

# Searching Algorithm for LK

$$TERM=\{<u_i,FORM(i)\}>\}=\{u_0,FORM(0)\}$$

$$FORM(0)=\{\}$$

$$AVAIL=\{u_1,u_2,...\}$$

## $Left-\exist$

Replace $A=\exist x,B$ by $B(t/x)$

where: $t=Head(AVAIL)$

$$TERM=TEARM\cup\{<t,\{\}>\}$$

$$AVAIL=Tail(AVAIL)$$

## $Left-\forall$

For each $u_k\in\{u_k|<u_k,FORM(k)>\in TERM\}$

if $A=\forall x,B\not\in FORM(k)$

Add $B(u_k/x)$ to the left side

$$FORM(k)=FORM(k)\cup\{A\}$$

## $Right-\forall$

Replace $A=\exist x,B$ by $B(t/x)$

where: $t=Head(AVAIL)$

$$TERM=TEARM\cup\{<t,\{\}>\}$$

$$AVAIL=Tail(AVAIL)$$

## $Right-\exist$

For each $u_k\in\{u_k|<u_k,FORM(k)>\in TERM\}$

if $A=\forall x,B\not\in FORM(k)$

Add $B(u_k/x)$ to the right side

$$FORM(k)=FORM(k)\cup\{A\}$$

# Test Result

$$
\cfrac{\color{green} \vdash (\exist x((P)\to(Q(x))))\to((P)\to(\exist z(Q(z))))}{\cfrac{\color{green}\exist x((P)\to(Q(x))) \vdash (P)\to(\exist z(Q(z)))}{\cfrac{\color{green}\exist x((P)\to(Q(x))),P \vdash \exist z(Q(z))}{\cfrac{\color{green}(P)\to(Q(t_1)),P \vdash \exist z(Q(z))}{\color{green}P \vdash \exist z(Q(z)),P \qquad \cfrac{\color{green}Q(t_1),P \vdash \exist z(Q(z))}{\color{green}Q(t_1),P \vdash \exist z(Q(z)),Q(t_0),Q(t_1)}}}}}
$$

$$
\cfrac{\color{red} \vdash (\exist x((P)\to(Q(x))))\to((P)\to(\forall z(Q(z))))}{\cfrac{\color{red}\exist x((P)\to(Q(x))) \vdash (P)\to(\forall z(Q(z)))}{\cfrac{\color{red}\exist x((P)\to(Q(x))),P \vdash \forall z(Q(z))}{\cfrac{\color{red}(P)\to(Q(t_1)),P \vdash \forall z(Q(z))}{\color{green}P \vdash \forall z(Q(z)),P \qquad \cfrac{\color{red}Q(t_1),P \vdash \forall z(Q(z))}{\color{red}Q(t_1),P \vdash Q(t_2)}}}}}
$$

$$
\cfrac{\color{green} \vdash (\exist x(A \wedge B))\to(\exist x(A) \wedge \exist x(B))}{\cfrac{\color{green}\exist x(A \wedge B) \vdash \exist x(A) \wedge \exist x(B)}{\cfrac{\color{green}\exist x(A \wedge B) \vdash \exist x(A)}{\cfrac{\color{green}A \wedge B \vdash \exist x(A)}{\cfrac{\color{green}A,B \vdash \exist x(A)}{\color{green}A,B \vdash \exist x(A),A}}} \qquad \cfrac{\color{green}\exist x(A \wedge B) \vdash \exist x(B)}{\cfrac{\color{green}A \wedge B \vdash \exist x(B)}{\cfrac{\color{green}A,B \vdash \exist x(B)}{\color{green}A,B \vdash \exist x(B),B}}}}}
$$

$$
\cfrac{\color{green} \vdash (\forall x(A) \vee \forall x(B))\to(\forall x(A \vee B))}{\cfrac{\color{green}\forall x(A) \vee \forall x(B) \vdash \forall x(A \vee B)}{\cfrac{\color{green}\forall x(A) \vdash \forall x(A \vee B)}{\cfrac{\color{green}\forall x(A),A \vdash \forall x(A \vee B)}{\cfrac{\color{green}\forall x(A),A \vdash A \vee B}{\color{green}\forall x(A),A \vdash A,B}}} \qquad \cfrac{\color{green}\forall x(B) \vdash \forall x(A \vee B)}{\cfrac{\color{green}\forall x(B),B \vdash \forall x(A \vee B)}{\cfrac{\color{green}\forall x(B),B \vdash A \vee B}{\color{green}\forall x(B),B \vdash A,B}}}}}
$$

$$
\cfrac{\color{green} \vdash ((\exist x(A))\to(\forall x(B)))\to(\forall x((A)\to(B)))}{\cfrac{\color{green}(\exist x(A))\to(\forall x(B)) \vdash \forall x((A)\to(B))}{\cfrac{\color{green} \vdash \forall x((A)\to(B)),\exist x(A)}{\cfrac{\color{green} \vdash (A)\to(B),\exist x(A)}{\cfrac{\color{green}A \vdash B,\exist x(A)}{\color{green}A \vdash B,\exist x(A),A}}} \qquad \cfrac{\color{green}\forall x(B) \vdash \forall x((A)\to(B))}{\cfrac{\color{green}\forall x(B),B \vdash \forall x((A)\to(B))}{\cfrac{\color{green}\forall x(B),B \vdash (A)\to(B)}{\color{green}\forall x(B),B,A \vdash B}}}}}
$$