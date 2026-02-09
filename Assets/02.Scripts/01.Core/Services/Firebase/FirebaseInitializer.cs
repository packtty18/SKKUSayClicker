using Cysharp.Threading.Tasks;

#if !UNITY_WEBGL || UNITY_EDITOR
public class FirebaseInitializer : GlobalSingleton<FirebaseInitializer>
{
    protected override void Init()
    {
        FirebaseService.InitializeAsync().Forget();
    }

}
#endif