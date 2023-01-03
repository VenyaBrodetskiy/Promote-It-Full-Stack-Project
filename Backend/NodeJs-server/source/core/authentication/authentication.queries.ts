export class AuthenticationQueries {
    
    public static GetUserByLogin: string = 
        `SELECT [user].id, password, user_type_id
        FROM [user] 
        WHERE login = ? AND [user].status_id = ?`;
}