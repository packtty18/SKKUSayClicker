using UnityEngine;

public class FirebaseInitializer : GlobalSingleton<FirebaseInitializer>
{
    protected override void Init()
    {
        FirebaseService.InitializeAsync();
    }

}
