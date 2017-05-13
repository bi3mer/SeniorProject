using UnityEngine;
using System.Collections;

public class ConsoleOpenAction : ConsoleAction {
    public GameObject ConsoleGui;

    public override void Activate()
    {
		#if UNITY_EDITOR
			ConsoleGui.SetActive(true);
		#else
			ConsoleGui.SetActive(Debug.isDebugBuild);
		#endif
    }
}
