module(array).
export(array_length/2).
end_module(array).
body(array).
array_length(Array,Length):-inner_length(Array,0,Length).
inner_length([],Length,Length).
inner_length([_|T],Prev,Length):-
    Next is Prev+1,
    inner_length(T,Next,Length).
end_body(array).