namespace int3306.Api.Structures
{
    public enum OrderStatusIndex
    {
        WaitingForConfirm = 1,
        Cancelled,
        Preparing,
        Shipping,
        Shipped
    }
}