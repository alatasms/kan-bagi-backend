namespace UserService.Application.Security
{
    /// <summary>Realm roles defined in deploy/keycloak/bloodapp-realm.json.</summary>
    public static class Roles
    {
        public const string Donor = "donor";
        public const string HospitalStaff = "hospital_staff";
        public const string Admin = "admin";
    }
}
