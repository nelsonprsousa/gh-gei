using System.Net.Http;

namespace OctoshiftCLI.GithubEnterpriseImporter
{
    public sealed class GithubApiFactory : ISourceGithubApiFactory, ITargetGithubApiFactory
    {
        private readonly OctoLogger _octoLogger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly EnvironmentVariableProvider _environmentVariableProvider;

        public GithubApiFactory(OctoLogger octoLogger, IHttpClientFactory clientFactory, EnvironmentVariableProvider environmentVariableProvider)
        {
            _octoLogger = octoLogger;
            _clientFactory = clientFactory;
            _environmentVariableProvider = environmentVariableProvider;
        }

        GithubApi ISourceGithubApiFactory.Create()
        {
            return ((ISourceGithubApiFactory)this).Create("https://api.github.com");
        }

        GithubApi ISourceGithubApiFactory.Create(string apiUrl)
        {
            var githubPat = _environmentVariableProvider.SourceGithubPersonalAccessToken();
            var githubClient = new GithubClient(_octoLogger, _clientFactory.CreateClient("Default"), githubPat, apiUrl);
            return new GithubApi(githubClient);
        }

        GithubApi ISourceGithubApiFactory.CreateClientNoSsl(string apiUrl)
        {
            var githubPat = _environmentVariableProvider.SourceGithubPersonalAccessToken();
            var githubClient = new GithubClient(_octoLogger, _clientFactory.CreateClient("NoSSL"), githubPat, apiUrl);
            return new GithubApi(githubClient);
        }

        GithubApi ITargetGithubApiFactory.Create()
        {
            return ((ITargetGithubApiFactory)this).Create("https://api.github.com");
        }

        GithubApi ITargetGithubApiFactory.Create(string apiUrl)
        {
            var githubPat = _environmentVariableProvider.TargetGithubPersonalAccessToken();
            var githubClient = new GithubClient(_octoLogger, _clientFactory.CreateClient("Default"), githubPat, apiUrl);
            return new GithubApi(githubClient);
        }
    }
}
