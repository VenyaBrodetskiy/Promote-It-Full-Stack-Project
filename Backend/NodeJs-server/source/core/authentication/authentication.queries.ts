export class AuthenticationQueries {
    
    public static GetUserByLogin: string = 
        `SELECT[user].id, password, user_type_id
        FROM[user] 
        WHERE login = 'proLobbyOwner' AND[user].status_id = 1`;

        // `SELECT [user].id, password, user_type_id, [user_type].[title]
        // FROM [user]
        // INNER JOIN [user_type] ON [user].user_type_id = [user_type].id
        // WHERE login = ? AND [user].status_id = ?`;
    // probably no need inner join of [user_type], can use just user_type_id
}