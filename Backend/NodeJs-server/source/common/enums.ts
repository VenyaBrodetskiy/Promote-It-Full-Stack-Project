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

export enum Role {
    AccessAdministrator = 1,
    RegularUser,
    AreaManager,
    NetworkAdministrator,
    StoreManager,
    Cashier
}

// export const Role = {
//     Administrator: 1,
//     RegularUser: 2,
//     AccessAdministrator: 3,
//     NetworkAdministrator: 4,
//     StoreManager: 5,
//     Cashier: 6
// }
// type RoleType = typeof Role;

export enum AppError {
    General = "General",
    ConnectionError = "ConnectionError",
    QueryError = "QueryError",
    NoData = "NoData",
    NonNumericInput = "NonNumericInput",
    InputParameterNotSupplied = "InputParameterNotSupplied",
    DeletionConflict = "DeletionConflict"
}