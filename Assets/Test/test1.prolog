person(bill).
person(jane).
lovers(bill,jane).

beNice(X,Y):- \+ fight(X,Y).

goal(X,Y):- beNice(X,Y).

wants(bill, oranges).
wants(jane, apples).

disagreement(X,Y):-
wants(X,A),
wants(Y,B),
does_not_want(X,B),
does_not_want(Y,A),
different_things(A,B).

does_not_want(X,Y):- 
    \+ wants(X,Y).

different_things(A,B):-
    A \= B.

fight(X,Y):-
    lovers(X,Y),
    disagreement(X,Y).

needs(bill, someAnswers).
needs(jane, apology).

has(bill, nothing).
has(jane, nothing).

