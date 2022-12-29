export class UserQueries {

    public static AddUser: string = `
        INSERT [user] 
            (login, password, user_type_id,
            create_date, update_date, 
            create_user_id, update_user_id, 
            status_id) 
        VALUES (?, ?, ?, ?, ?, ?, ?, ?)`;
    
    public static AddSocialActivist: string = `
        INSERT [social_activist] 
            (user_id, twitter_handle, email,
            address, phone_number,
            create_date, update_date, 
            create_user_id, update_user_id, 
            status_id) 
        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)`;
    
    public static AddBusinessOwner: string = `
        INSERT [business_owner] 
            (user_id, twitter_handle, name, email,
            create_date, update_date, 
            create_user_id, update_user_id, 
            status_id) 
        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)`;
    
    public static AddNonProfitOrganization: string = `
        INSERT [non_profit_organization] 
            (user_id, name, email, website,
            create_date, update_date, 
            create_user_id, update_user_id, 
            status_id) 
        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)`;
    
    // delete below ->
    
    public static GetAll: string = `
        SELECT u.id, first_name, last_name, [login], r.role_name
        FROM [user] AS u
        INNER JOIN user_to_role u_r ON u_r.user_id = u.id
        INNER JOIN [role] r ON r.id = u_r.role_id
        WHERE u.status_id = ? AND u_r.status_id = ? AND r.status_id = ?
    `
    public static UpdateUserById: string = `
        UPDATE [user] 
        SET first_name = ?, last_name = ?, update_date = ?, update_user_id = ? 
        WHERE id = ? AND status_id = ?`;
    
    public static AddRolesToUser: string = `
        INSERT user_to_role
        (user_id, role_id, 
        create_date, update_date, 
        create_user_id, update_user_id, 
        status_id)
        VALUES
        (?, ?, ?, ?, ?, ?, ?)`;
    public static DeleteUserById: string = `
        UPDATE [user] 
        SET update_date = ?, update_user_id = ?, status_id = ? 
        WHERE id = ? AND status_id = ?`;
    public static DeleteRolesOfUser: string = `
        UPDATE user_to_role
        SET update_date = ?, update_user_id = ?, status_id = ?
        WHERE user_id = ? AND status_id = ?`;

}