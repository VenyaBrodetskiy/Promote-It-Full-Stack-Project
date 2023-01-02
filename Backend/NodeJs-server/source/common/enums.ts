export enum Statuses {
    Active = 1,
    NotActive = 2,
}

export enum UserType {
    businessOwner = 1,
    socialActivist,
    nonProfitOrganization,
    prolobbyOwner,
    system
}

export enum AppError {
    General = "General",
    ConnectionError = "ConnectionError",
    QueryError = "QueryError",
    NoData = "NoData",
    NonNumericInput = "NonNumericInput",
    InputParameterNotSupplied = "InputParameterNotSupplied",
    DeletionConflict = "DeletionConflict"
}