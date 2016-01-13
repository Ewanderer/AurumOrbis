import math;
mod=float(input("Lernmodifikator:"));
rank=float(input("Rangziel:"));
c=pow(10,0.5);
xp=0.0;
while True:
	xp+=1;
	a=pow((9*xp*mod)+490 ,0.5)
	if (1/9)*(-140+2*c*a) >= rank:
		break;
print("Benötigte XP:"+str(xp));

print(math.ceil(((7*rank)+(9*rank*(rank/40)))*pow(mod,-1) ) );	
