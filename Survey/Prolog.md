# Prolog

Prolog is a logic programming language associated with artificial intelligence and computational linguistics.

Prolog has its roots in [first-order logic](First_Order_Logic.md), a formal logic, and unlike many other programming languages, Prolog is intended primarily as a declarative programming language: the program logic is expressed in terms of relations, represented as facts and rules. A computation is initiated by running a query over these relations.

The language was developed and implemented in Marseille, France, in 1972 by Alain Colmerauer with Philippe Roussel, based on Robert Kowalski's procedural interpretation of Horn clauses at University of Edinburgh.

The resolution method used by Prolog is called **SLD resolution**

## Syntax

### Data Type

Term is single data type in Prolog.

- Atom:`[a-z][0-9a-zA-Z_]*`
- Number
- Variable:`[A-Z_][0-9a-zA-Z_]*`
- Compound Term
  - Functor:`functor(arg1,arg2,...,argN)`
  - List:`[]`
  - String

### Rules

Like Methods in other languages, `A :- B` indicates $B\to A$

### Query

`?- A` return whether `A` is true

`?- A(X)` return all `X` which makes `A(X)` true

### Negation

`A :- \+ B` indicates $\lnot B\to A$

Prolog attemps to prove `B`. If a proof can be found, then `A` fails. If no proof can be found, then `A` succeeds.

### Array

```Prolog
[H|T]=[1,2,3]
H=1
T=[2,3]

[H|T]=[1]
H=1
T=[]
```

### States

1. Call: Execute first query
2. Exit: If Query succeeds
3. Redo: From Exit, execute next query
4. Fail: If Query fails

```
Goal: door(kitchen, R), location(T,R)
1 CALL door(kitchen, R)
1 EXIT (2) door(kitchen, office)
2 CALL location(T, office)
2 EXIT (1) location(desk, office)
R = office
T = desk ;
2 REDO location(T, office)
2 EXIT (8) location(computer, office)
R = office
T = computer ;
2 REDO location(T, office)
2 FAIL location(T, office)
1 REDO door(kitchen, R)
1 EXIT (4) door(kitchen, cellar)
2 CALL location(T, cellar)
2 EXIT (4) location(‘washing machine’, cellar)
R = cellar
T = ‘washing machine’ ;
2 REDO location(T, cellar)
2 FAIL location(T, cellar)
1 REDO door(kitchen, R)
1 FAIL door(kitchen, R)
no
```

## SWI-Prolog

<https://github.com/SWI-Prolog/swipl-devel>