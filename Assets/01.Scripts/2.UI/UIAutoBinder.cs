using UnityEngine;
using System.Reflection;

public class UIAutoBinder
{
    public static void BindUI(Object target, Transform root)
    {
        var fields = target.GetType().GetFields
        (BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        foreach (var field in fields)
        {
            var bindAttr = field.GetCustomAttribute<UIBindAttribute>();
            if (bindAttr == null) continue;

            var targetTransform = root.Find(bindAttr.Path);
            if (targetTransform == null)
            {
                Debug.Log($"UIAutoBind Path 못 찾음 {bindAttr.Path}");
                continue;
            }

            if (field.FieldType == typeof(GameObject))
            {
                field.SetValue(target, targetTransform.gameObject);
                continue;
            }

            var component = targetTransform.GetComponent(field.FieldType);
            if (component == null)
            {
                Debug.Log($"[UIAutoBinder] {bindAttr.Path}에 {field.FieldType.Name} 없음");
                continue;
            }

            field.SetValue(target, component);
        }
    }
}
