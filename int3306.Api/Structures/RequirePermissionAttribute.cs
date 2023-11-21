namespace int3306.Api.Structures
{
    public class RequirePermissionAttribute : Attribute
    {
        public readonly PermissionIndex PermissionIndex;
        public RequirePermissionAttribute(PermissionIndex permissionIndex)
        {
            PermissionIndex = permissionIndex;
        }
    }
}