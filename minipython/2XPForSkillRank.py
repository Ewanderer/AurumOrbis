mod=float(input("Lernmodifikator:"));
rank=float(input("Rangziel:"));
xp=0.0;
while True:
	xp+=1;
	if (1/9)*(-140+2*pow(10,0.5)*pow(9*xp*mod+490,0.5))>=rank:
		break;
print("Benötigte XP:"+str(xp));
	
