namespace OpShared.Security;

public static class Permissions
{
    public static class Tenant
    {
        public const string Read   = "tenant.read";
        public const string Manage = "tenant.manage";
    }

    public static class Booking
    {
        public const string Read   = "booking.read";
        public const string Manage = "booking.manage";
        public const string Cancel = "booking.cancel";
    }

    public static class Product
    {
        public const string Read   = "product.read";
        public const string Manage = "product.manage";
    }

}