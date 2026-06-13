using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ENyayPath.PICS.Core.Authorization.Permissions
{
    /// <summary>
    /// Central catalog of permission constants.
    /// Keeps code consistent and avoids typos.
    /// </summary>
    public static class PermissionNames
    {
        // --- User Management ---
        public const string Users_Create = "Users.Create";
        public const string Users_Read = "Users.Read";
        public const string Users_Update = "Users.Update";
        public const string Users_Delete = "Users.Delete";

        // --- Role Management ---
        public const string Roles_Create = "Roles.Create";
        public const string Roles_Read = "Roles.Read";
        public const string Roles_Update = "Roles.Update";
        public const string Roles_Delete = "Roles.Delete";

        // --- Feature Management ---
        public const string Features_Create = "Features.Create";
        public const string Features_Read = "Features.Read";
        public const string Features_Update = "Features.Update";
        public const string Features_Delete = "Features.Delete";

        // --- Edition Management ---
        public const string Editions_Create = "Editions.Create";
        public const string Editions_Read = "Editions.Read";
        public const string Editions_Update = "Editions.Update";
        public const string Editions_Delete = "Editions.Delete";

        // --- Organization Unit Management ---
        public const string OrgUnits_Create = "OrgUnits.Create";
        public const string OrgUnits_Read = "OrgUnits.Read";
        public const string OrgUnits_Update = "OrgUnits.Update";
        public const string OrgUnits_Delete = "OrgUnits.Delete";

        // --- Setting Management ---
        public const string Settings_Create = "Settings.Create";
        public const string Settings_Read = "Settings.Read";
        public const string Settings_Update = "Settings.Update";
        public const string Settings_Delete = "Settings.Delete";

        // --- Prisoner Management ---
        public const string Prisoners_Create = "Prisoners.Create";
        public const string Prisoners_Read = "Prisoners.Read";
        public const string Prisoners_Update = "Prisoners.Update";
        public const string Prisoners_Delete = "Prisoners.Delete";

        /// <summary>
        /// Returns all permission constants defined in this class.
        /// </summary>
        public static IEnumerable<string> GetAll()
        {
            return typeof(PermissionNames)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(fi => (string)fi.GetRawConstantValue());
        }
    }
}
