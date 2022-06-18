# Craig Interpolation without Equality

If a formula $A\to B$ is valid, then there is a fomula $I$

$$A\to I,I\to B$$

$$FV(I)\subseteq FV(A)\cup FV(B)$$

# Herbrand's Theorem

## Remove Free Variables

$$A=G_1x_1G_2x_2...G_nx_n A'$$

$$FV(A)=\{y_1,y_2,...,y_m\}$$

$A$ is provable iff $\forall y_1\forall y_2...\forall y_m A$ is provable

## Skolem Function

Remove a variable $x_i$ bound by an occurrence $Q_i$ of q universal quantifier by $f_i$ which is called Skolem Function

Using $a$,$f(a)$,$g(f(a))$ to record the order in which the $\forall$ right rule were applied

$Q_1x_1Q_2x_2...Q_nx_n(M)$

For $\exist x_r$, $\forall x_{s_1}\forall x_{s_2}...\forall x_{s_n}$, $1\leq s_1<s_2,...,s_n<r$, $x_r=f(x_{s_1},x_{s_2},...,x_{s_n})$

Equivalent in the sense of incompatibility

## Substitution

$$\sigma:V\to TERM_L$$

$$
\bar{\sigma}(x)=
\begin{cases}
\sigma(x)&x\in V\\
\sigma(c)&c\text{ is constant}\\
f(\bar{\sigma}(t_1),\bar{\sigma}(t_2)...)&x=f(t_1,t_2,...)
\end{cases}
$$

if $\sigma(x)\neq x$ is call the support of the substitution

If $\sigma$ has finite support $\{y_1,y_2,...\}$ and $\sigma(y_i)=s_i$

$$
\bar{\sigma}(t)=t[s_1/y_1,s_2/y_2,...]
$$

## Functional Form

$$
A=Q_nx_n...Q_1x_1C
$$

$$
\exist y_1\exist y_2...\exist y_m C[r_1/z_1,...,r_p/z_p]
$$

$$
\{y_1,y_2,...,y_m\}\cup\{z_1,z_2,...,z_p\}=\{x_1,...,x_n\}
$$

$\{z_1,...,z_p\}$ is the set of variables which are universally quantified in $A$

$r_i$ is rooted with the Skolem function $f_i^A$



首先使用Skolem Function消除存在量词，变成Skolem范式，那么就可以将公式表示为子句的集合，这个变换是在不相容的意义下保持等价的。也就是说证明公式G满足，就是思考如何证明\lnot G的Skolem范式不可满足，类比右部形式的全称量词化简规则，每次可以挑选新的变量进行拓展，例如Team_0={v,f(v),f^n(v)}，那么就可以构造包含全部可能性的集合，称作Herbrand域，（用Herbrand域中的元素替换全称量词后，就相当于对全称量词的一种指派，这被称作是基础实例），同时可以构造在Herbrand域下的解释，使得在原论域可满足等价为在H域下可满足，同时可以证明Skolem范式不可满足当且仅当在任何的H域赋值下恒为假，之后便证明了Herbrand定理。
