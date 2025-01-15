using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour, IShootable
{
    public UnityEvent on_click;

    public void Shoot()
    {
        on_click.Invoke();
    }

}
