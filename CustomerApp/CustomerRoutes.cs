namespace CustomerApp
{
    public class CustomerRoutes
    {
        internal const string ApiPrefix = "api/v1";
        internal const string Customers = ApiPrefix + "/customers";
        internal const string Customer = Customers + "/{customerId}";
        internal const string StatusUpdate = Customers + "/{customerId}/status";
        internal const string CustomerNotes = Customer + "/notes";

        internal const string Notes = ApiPrefix + "/notes";
        internal const string Note = ApiPrefix + "/notes/{noteId}";
    }
}