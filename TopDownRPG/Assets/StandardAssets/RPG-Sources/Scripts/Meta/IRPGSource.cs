using UnityEngine;
using System.Collections;

public interface IRPGSource {
	void SendMessage(string Message,IRPGSource Source);
}
