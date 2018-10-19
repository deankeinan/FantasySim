person(bill).
person(jane).
action(steal).
action(help_authorities).

good(bill).
lawful(bill).

evil(jane).
chaotic(jane).

evil(steal).
chaotic(steal).

good(help_authorities).
lawful(help_authorities).

immoral(character,act):-
     (conflicts_morality(character,act); conflicts_nature(character,act)).

conflicts_morality(character,act):-
     (
       good(character), evil(act);
       evil(character), good(act)
     ).

conflicts_nature(character,act):-
      (
        chaotic(character), lawful(act));
        lawful(character), chaotic(act))
     ).
