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



/*
Character Alignments
    morality (Good/Evil)
    nature (Lawful/Chaotic)

Functions for
    Getting and setting alignment
    Checking possible actions against alignment
*/

good(bill).
chaotic(bill).

evil(jane).
lawful(jane).

%methods for setting character alignment
alignment(C, N, M):-
    nature(C,N),
    morality(C,M).

nature(C,N):-
    N == lawful,
    assert(lawful(C)),
    (chaotic(C) -> retract(chaotic(C)); true).
nature(C,N):-
    N == chaotic,
    assert(chaotic(C)),
    (lawful(C) -> retract(lawful(C)); true).


morality(C,M):-
    M == evil,
    assert(evil(C)),
    (good(C) -> retract(good(C)); true).
morality(C,M):-
    M == good,
    assert(good(C)),
    (evil(C) -> retract(evil(C)); true).




%methods for printing alignment to console
alignment(C):-
    nature(C),
    morality(C).

nature(C):-
    (lawful(C) -> write('Lawful-') ; write('Chaotic-')).

morality(C):-
    (good(C) -> writeln('Good'); writeln('Evil')).



%methods for comparing possible actions to a characters alignment
out_of_character(C,A):-
    not(natural(C,A));
    not(moral(C,A)).

moral(C,A):-
    good(C),
    not(evilAction(A)).
moral(C,A):-
    evil(C),
    not(goodAction(A)).
      
natural(C,A):-
    lawful(C),
    not(chaoticAction(A)).
natural(C,A):-
    chaotic(C).

evilActions:-
    writeln([steal, hurt, murder, lie, break_promise]).
evilAction(A):-
    member(A, [steal, hurt, murder, lie, break_promise]).

goodActions:-
    writeln([help, protect, sacrifice, donate]).
goodAction(A):-
    member(A, [help, protect, sacrifice, donate]).

chaoticActions:-
    writeln([break_rule, break_promise, disrespect, steal, lie]).
chaoticAction(A):-
    member(A, [break_rule, break_promise, disrespect, steal, lie]).
    
lawfulActions:-
    writeln([judge, tell_truth, help]).
lawfulAction(A):-
    member(A, [judge, tell_truth, help]).