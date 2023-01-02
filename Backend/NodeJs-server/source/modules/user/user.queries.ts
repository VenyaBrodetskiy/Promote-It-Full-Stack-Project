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
            (user_id, twitter_handle, 
            name, email,
            create_date, update_date, 
            create_user_id, update_user_id, 
            status_id) 
        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)`;
    
    public static AddNonProfitOrganization: string = `
        INSERT [non_profit_organization] 
            (user_id, name, 
            email, website,
            create_date, update_date, 
            create_user_id, update_user_id, 
            status_id) 
        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)`;
    
}