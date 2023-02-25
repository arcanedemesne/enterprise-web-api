namespace Enterprise.Solution.Email.Service
{
    public interface IHtmlContentGeneratorService
    {
        Task<string> GenerateHtmlContentFromStringTemplate<T>(string templateContent, T model, CancellationToken token);
    }
}
