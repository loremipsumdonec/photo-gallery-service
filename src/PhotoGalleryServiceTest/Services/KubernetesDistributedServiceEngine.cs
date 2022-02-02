using Boilerplate.Features.Testing.Services;
using CliWrap;
using CliWrap.Buffered;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoGalleryServiceTest.Services
{
    internal class KubernetesDistributedServiceEngine
        : DistributedServiceEngine
    {
        private readonly string _path;
        private readonly string _namespace;

        public KubernetesDistributedServiceEngine(
            string path, 
            string @namespace, 
            IReadinessProbe readinessProbe)
            : base(readinessProbe)
        {
            _path = path;
            _namespace = @namespace;
        }

        public override async Task StartAsync()
        {
            var deployments = Directory.GetFiles(_path, "*.yml");

            List<Task> tasks = new();

            foreach (string file in deployments)
            {
                tasks.Add(DeployAsync(file));
            }

            Task.WaitAll(tasks.ToArray());
            await WaitForPodsAsync();

            await base.StartAsync();        
        }

        public override Task StopAsync()
        {
            return Task.CompletedTask;
        }
        
        private Task DeployAsync(string deployment)
        {
            return Cli.Wrap("kubectl")
                .WithArguments($"apply -n {_namespace} -f {deployment}")
                .ExecuteBufferedAsync();
        }

        private async Task WaitForPodsAsync()
        {
            int current = 0;
            int timeout = 15000;
            int sleep = 200;
            bool status;

            do
            {
                status = false;

                foreach (var pod in await GetPodsAsync()) 
                {
                    if (!IsPodRunning(pod))
                    {
                        status = true;
                    }

                    if (current > timeout)
                    {
                        throw new TimeoutException($"Timed out on waiting for pods");
                    }
                }

                await Task.Delay(sleep);
                current += sleep;
            }
            while (status);
        }

        private bool IsPodRunning(Dictionary<string, string> pod)
        {
            if (pod == null)
            {
                return false;
            }

            return pod["pod_status"].Equals("Running", StringComparison.InvariantCultureIgnoreCase);
        }

        private async Task<List<Dictionary<string, string>>> GetPodsAsync()
        {
            var result = await Cli.Wrap("kubectl")
                .WithArguments($"get pod -o=custom-columns=pod_name:.metadata.name,pod_status:.status.phase,pod_ip:.status.hostIP,deployment_name:.metadata.labels.app --namespace {_namespace}")
                .ExecuteBufferedAsync();

            return Parse(result);
        }

        private List<Dictionary<string, string>> Parse(BufferedCommandResult result) 
        {
            var services = new List<Dictionary<string, string>>();
            List<string> properties = new List<string>();

            foreach (string line in result.StandardOutput.Split("\n"))
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                var values = line.Split(' ')
                    .Select(t => t.Trim())
                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToList();

                if (properties.Count == 0)
                {
                    properties.AddRange(values);
                    continue;
                }

                Dictionary<string, string> service = new Dictionary<string, string>();
                services.Add(service);

                for (int index = 0; index < properties.Count; index++)
                {
                    service[properties[index]] = values[index];
                }
            }

            return services;
        }
    }
}
