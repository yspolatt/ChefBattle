using UnityEngine;


public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
   public static T Instance { get; private set; }
   protected virtual void Awake()
   {
       
           Instance = this as T;
       
   }
   protected virtual void OnDestroy()
   {
       if (Instance == this)
       {
           Instance = null;
           Destroy(gameObject);
       }
   }

}
public class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        base.Awake();
    }
}
