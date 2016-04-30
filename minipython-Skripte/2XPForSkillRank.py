import math;
mod=float(input("Lernmodifikator:"));
rank=float(input("Rangziel:"));


print(math.ceil( -( (750*rank)/(rank-250))*(1/mod) ) );	
input();