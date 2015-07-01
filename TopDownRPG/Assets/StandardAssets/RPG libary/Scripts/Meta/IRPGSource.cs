using UnityEngine;
using System.Collections;

public interface IRPGSource {
	void sendMessage(string Message,IRPGSource Source);
	string getID();

	float this[string ValueName] {
		get;
	}
}
