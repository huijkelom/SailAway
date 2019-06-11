using UnityEngine;

public class DMXControler : MonoBehaviour {

    public static DMXControler Instance;
    public MainAppGateway MainApp;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPreset(int number)
    {
        if(number < 7)
        {
            MainApp.SendMessageToMain("DMX" + number);
        }
    }
}
