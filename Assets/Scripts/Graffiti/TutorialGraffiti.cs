using UnityEngine;
using System.Reflection;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class TutorialGraffiti : MonoBehaviour
{    
	[Tooltip("The text that comes before keys")]
    [SerializeField]
    private string beforeKeys;

	[Tooltip("The text that comes after the keys")]
    [SerializeField]
    private string afterKeys;

	[Tooltip("The text to place in between keys if there are multiple keys")]
    [SerializeField]
    private string betweenKeys;

	[Tooltip("The keys to access")]
    [SerializeField]
    private string[] keys;

    /// <summary>
    /// Calls setGraffitiText
    /// </summary>
	void Start ()
    {
        setGraffitiText();
    }

    /// <summary>
    /// Set the text on the graffiti based on the current control scheme
    /// </summary>
    public void setGraffitiText()
    {
        ControlScheme controlSchemeInstance = Game.Instance.GameSettingsInstance.Scheme;
        string textString = beforeKeys;
        for (int i = 0; i < keys.Length; ++i)
        {
            textString += controlSchemeInstance.GetType().GetField(keys[i]).GetValue(controlSchemeInstance);
            if (i != keys.Length - 1)
            {
                textString += betweenKeys;
            }
        }
        textString += afterKeys;

        // Replace instances of \n with the newline code \n. This is a unity quirk. But it'll let us use \n in the inspector to call for new lines
        textString = textString.Replace("\\n", "\n");
        GetComponent<TextMesh>().text = textString;
    }
}
