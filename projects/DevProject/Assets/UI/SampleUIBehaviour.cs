using UnityEngine;
using UnityEngine.UIElements;

public class SampleUIBehaviour : MonoBehaviour
{
    [SerializeField]
    private UIDocument document;
    protected VisualElement root;

    void Start()
    {
        root = document.rootVisualElement;

        TextField textField = root.Q<TextField>("LogText");

        root.Q<Button>("ButtonA").clicked += () =>
        {
            textField.value += "Button A was clicked\n";
        };
        root.Q<Button>("ButtonB").clicked += () =>
        {
            textField.value += "Button B was clicked\n";
        };
    }
}
