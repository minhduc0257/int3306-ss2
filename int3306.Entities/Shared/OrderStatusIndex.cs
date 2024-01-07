namespace int3306.Entities.Shared
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