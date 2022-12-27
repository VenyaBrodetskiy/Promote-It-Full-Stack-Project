export class CampaignQueries {

    public static GetAllCampaigns: string = `
        SELECT [id], [hashtag], [landing_page], [name]
        FROM [dbo].[campaign]
        INNER JOIN [non_profit_organization] ON [campaign].[user_id] = [non_profit_organization].[user_id]
        WHERE [campaign].[status_id] = ?`;

    public static AddCampaign: string = `
        INSERT campaign 
            (hashtag,  landing_page, user_id, 
            create_date, update_date, 
            create_user_id, update_user_id, 
            status_id) 
        VALUES (?, ?, ?, ?, ?, ?, ?, ?)`;

}