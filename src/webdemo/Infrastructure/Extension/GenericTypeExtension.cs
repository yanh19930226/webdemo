namespace webdemo.Infrastructure.Extension
{
    public static class GenericTypeExtension
    {
        public static string GetGenericTypeName(this Type type)
        {
            string empty = string.Empty;
            if (type.IsGenericType)
            {
                string str = string.Join(",", type.GetGenericArguments().Select((t) => t.Name).ToArray());
                return type.Name.Remove(type.Name.IndexOf('`')) + "<" + str + ">";
            }
            return type.Name;
        }

        public static string GetGenericTypeName(this object @object)
        {
            return @object.GetType().GetGenericTypeName();
        }

        public static bool HasImplementedRawGeneric(this Type type, Type generic)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (generic == null)
            {
                throw new ArgumentNullException("generic");
            }
            if (type.GetInterfaces().Any(IsTheRawGenericType))
            {
                return true;
            }
            while (type != null && type != typeof(object))
            {
                if (IsTheRawGenericType(type))
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
            bool IsTheRawGenericType(Type test)
            {
                return generic == (test.IsGenericType ? test.GetGenericTypeDefinition() : test);
            }
        }
    }
}
