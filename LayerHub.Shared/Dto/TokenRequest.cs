namespace LayerHub.Shared.Dto;

public class TokenRequest
{
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
