class Faehigkeit
	Bezeichnung="nonname";
	Att1=-1;
	Att2=-1;
	Att3=-1;
	mod=0.0;
	XP=0.0;
	def __init__(self, n,a1,a2,a3,m,xp):
		self.Bezeichnung=n;
		self.Att1=a1;
		self.Att2=a2;
		self.Att3=a3;
		self.mod=m;
		self.XP=xp;
	def getRank(self):
		return (1/9)*(-140+2*pow(10,0.5)*pow(9*self.XP+490,0.5));
	def calculateReward(self, Zuschlag, Komplexitaet):
		k=(Zuschlag+self.getRank)/3;
		fail=1-(((10+k+a1)/20)*((10+k+a2)/20)*((10+k+a3)/20));
		reward=3*Komplexitaet*fail;
		return reward,fail;
		
		
class CharakterBogen:
	Name="noname";
	Spieler="noone";
	#As XP-Values
	Staerke=0.0;
	Mut=0.0;
	Beweglichkeit=0.0;
	Fingerfertigkeit=0.0;
	Konstitution=0.0;
	Metabolismus=0.0;
	Intelligenz=0.0;
	Weisheit=0.0;
	Aussehen=0.0;
	Charisma=0.0;
	
def mainMenue():
	
	
	