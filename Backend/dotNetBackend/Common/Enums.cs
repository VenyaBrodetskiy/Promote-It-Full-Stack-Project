namespace dotNetBackend.Common
{
    public enum Statuses 
    {
        Active = 1,
        NonActive = 2
    }

    public enum TransactionStates
    {
        Ordered = 1,
        Shipped = 2,
        Donated = 3
    }

    public enum UserTypes
    {
        BusinessOwner = 1,
        SocialActivist = 2,
        NonprofitOrganization = 3,
        ProlobbyOwner = 4,
        System = 5
    }
}
