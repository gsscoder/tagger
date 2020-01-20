using System.Reflection;

static class ConstructorInfoExtensions
{
    public static bool ValidateNames(this ConstructorInfo ctorInfo, object dynamicLayout)
    {
        if (dynamicLayout == null) return true;

        var @params = ctorInfo.GetParameters();
        var props = dynamicLayout.GetType().GetProperties();
        if (@params.Length != props.Length) {
            return false;
        }
        for (var i = 0; i < @params.Length; i++) {
            if (!@params[i].Name.Equals(props[i].Name)) {
                return false;
            }
        }
        return true;
    }
}