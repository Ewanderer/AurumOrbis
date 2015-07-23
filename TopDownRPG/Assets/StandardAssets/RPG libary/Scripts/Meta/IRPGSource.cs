using UnityEngine;
using System.Collections;

public interface IRPGSource:IDInterface {
	void sendMessage(string Message,IRPGSource Source);


	float this[string ValueName] {
		get;
	}
}
