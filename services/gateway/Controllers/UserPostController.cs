using System.Net.Http.Headers;
using APIGateway.Models.Common;
using APIGateway.Models.MatchingService;
using APIGateway.Models.PostService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("aggregate")]
    [Authorize]
    public class UserPostController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserPostController> _logger;

        public UserPostController(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<UserPostController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Posts the caller has responded to, each marked with whether the donation was verified.
        /// The user is always the token's subject; there is no way to ask for someone else's list.
        /// </summary>
        [HttpGet("me/posts")]
        public async Task<IActionResult> GetMyMatchedPosts()
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var matchingBaseUrl = _configuration["Downstream:MatchingService"];
            var postBaseUrl = _configuration["Downstream:PostService"];

            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                // Downstream services validate the same token themselves.
                httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(Request.Headers.Authorization.ToString());

                var matchingUrl = $"{matchingBaseUrl}/api/matching/user/{Uri.EscapeDataString(userId)}";
                var matchingResponse = await httpClient.GetFromJsonAsync<StandardResponse<List<MatchingResponse>>>(matchingUrl);
                var postIds = matchingResponse?.Response?.Select(x => x.PostId)?.ToList();
                if (postIds == null || postIds.Count == 0)
                    return Ok(new StandardResponse<List<PostResponse>>());

                var queryString = string.Join("&", postIds.Select(id => $"PostIds={Uri.EscapeDataString(id)}"));
                var url = $"{postBaseUrl}/api/post/get-all-posts?{queryString}";

                var postResponse = await httpClient.GetFromJsonAsync<StandardResponse<PagedResponse<PostResponse>>>(url);

                if (postResponse == null || !postResponse.IsSuccess)
                    return StatusCode(StatusCodes.Status502BadGateway, "Post service returned an error.");

                postResponse.Response?.Items?.ForEach(p =>
                {
                    p.IsVerified = matchingResponse!.Response!.FirstOrDefault(m => m.PostId == p.Id.ToString())?.IsVerified ?? false;
                });

                return Ok(postResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Aggregating matched posts failed for user {UserId}", userId);
                return StatusCode(StatusCodes.Status502BadGateway, "A downstream service is unavailable.");
            }
        }
    }
}
