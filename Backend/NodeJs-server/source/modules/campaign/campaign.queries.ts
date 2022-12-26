export class CampaignQueries {

    public static AddCampaign: string = `
        INSERT campaign 
            (hashtag,  landing_page, user_id, 
            create_date, update_date, 
            create_user_id, update_user_id, 
            status_id) 
        VALUES (?, ?, ?, ?, ?, ?, ?, ?)`;

}