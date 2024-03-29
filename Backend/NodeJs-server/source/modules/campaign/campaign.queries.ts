export class CampaignQueries {

    public static GetAllCampaigns: string = `
        SELECT [id], [hashtag], [landing_page], [non_profit_organization].[name] as non_profit_organization_name
        FROM [dbo].[campaign]
        INNER JOIN [non_profit_organization] ON [campaign].[user_id] = [non_profit_organization].[user_id]
        WHERE [campaign].[status_id] = ?`;
    
    public static GetAllCampaignsWitnProducts: string = `
        SELECT 
            [campaign].[id], 
            [hashtag], 
            [landing_page], 
            [non_profit_organization].[name] as non_profit_organization_name, 
            [title] as product_title, 
            [business_owner].[name] as business_owner_name, 
            [product_qty]
            [price]
        FROM [dbo].[campaign]
        INNER JOIN [non_profit_organization] ON [campaign].[user_id] = [non_profit_organization].[user_id]
        LEFT JOIN [product_to_campaign_qty] ON [campaign].[id] = [product_to_campaign_qty].[campaign_id]
        LEFT JOIN [product] ON [product_to_campaign_qty].[product_id] = [product].[id]
        LEFT JOIN [business_owner] ON [product].[user_id] = [business_owner].[user_id]
        WHERE [campaign].[status_id] = ?`;
    
    public static GetAllProductsForCampaign: string = `
        SELECT 
            [campaign].[id] as [id], 
            [product].[id] as [product_id],
            [product].[title] as product_title, 
            [product].[price] as product_price, 
            [product_qty] as product_qty, 
            [business_owner].[name] as campany_name
        FROM [dbo].[campaign]
        INNER JOIN [non_profit_organization] ON [campaign].[user_id] = [non_profit_organization].[user_id]
        LEFT JOIN [product_to_campaign_qty] ON [campaign].[id] = [product_to_campaign_qty].[campaign_id]
        LEFT JOIN [product] ON [product_to_campaign_qty].[product_id] = [product].[id]
        LEFT JOIN [business_owner] ON [product].[user_id] = [business_owner].[user_id]
        WHERE [campaign].[id] = ? and [campaign].[status_id] = ? and [product_qty] != 0`;
    
    
    
    public static getCampaignsByNonProfitId: string = `
        SELECT [id], [hashtag], [landing_page], [non_profit_organization].[name] as non_profit_organization_name
        FROM [dbo].[campaign]
        INNER JOIN [non_profit_organization] ON [campaign].[user_id] = [non_profit_organization].[user_id]
        WHERE [campaign].[user_id] = ? AND [campaign].[status_id] = ?`;

    public static AddCampaign: string = `
        INSERT campaign 
            (hashtag,  landing_page, user_id, 
            create_date, update_date, 
            create_user_id, update_user_id, 
            status_id) 
        VALUES (?, ?, ?, ?, ?, ?, ?, ?)`;

}