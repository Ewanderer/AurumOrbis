rank=float(input("Rangziel:"));
xp=0.0;
while True:
	xp+=1;
	if (1/11)*(-90+2*pow(5,0.5)*pow(11*xp+405,0.5))>=rank:
		break;
print("Benötigte XP:"+str(xp-107));
