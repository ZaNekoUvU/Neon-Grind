using UnityEngine;

public interface INeonGrindListener
{
    void OnEvent(NeonGrindEvents eventType, Component sender, object param = null);
}
