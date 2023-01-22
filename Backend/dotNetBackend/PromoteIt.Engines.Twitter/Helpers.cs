using PromoteIt.Engines.Twitter.Common;
using PromoteIt.Engines.Twitter.Models;

public static class Helpers
{
    public static async Task<List<CampaignInfo>> GetAllCampaigns(ILogger logger, HttpClient httpClient)
    {
        try
        {
            logger.LogInformation("Getting all campaigns. Sending request to {accessor}", Endpoints.AccessorGetCampaigns);
            var campaignsResponse = await httpClient.GetAsync(Endpoints.AccessorGetCampaigns);
            campaignsResponse.EnsureSuccessStatusCode();

            List<CampaignInfo>? campaigns = await campaignsResponse.Content.ReadFromJsonAsync<List<CampaignInfo>>();
            if (campaigns == null || campaigns.Count == 0)
            {
                logger.LogError($"Couldn't read campaigns from response body");
                throw new Exception($"Couldn't read campaigns from response body");
            }
            return campaigns;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            throw;
        }
    }

    public static async Task<List<SocialActivistDTO>> GetAllSocialActivists(ILogger logger, HttpClient httpClient)
    {
        try
        {
            logger.LogInformation("Getting all social activists. Sending request to {accessor}", Endpoints.AccessorGetSocialActivists);
            var socialActivistsResponse = await httpClient.GetAsync(Endpoints.AccessorGetSocialActivists);
            socialActivistsResponse.EnsureSuccessStatusCode();

            List<SocialActivistDTO>? socialActivists = await socialActivistsResponse.Content.ReadFromJsonAsync<List<SocialActivistDTO>>();
            if (socialActivists == null)
            {
                logger.LogError($"Couldn't read social activists from response body");
                throw new Exception($"Couldn't read social activists from response body");
            }

            return socialActivists;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            throw;
        }
    }
}