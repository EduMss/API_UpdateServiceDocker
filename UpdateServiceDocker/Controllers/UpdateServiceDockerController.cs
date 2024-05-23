using Microsoft.AspNetCore.Mvc;
using UpdateServiceDocker.DTO;
using System.Text.Json;
using System.Diagnostics;

namespace UpdateServiceDocker.Controllers
{
    [ApiController]
    public class UpdateServiceDockerController : Controller
    {
        public ConfigsContainer config;
        public string ditScripts = "/Files-Data/Scripts/";
        public UpdateServiceDockerController()
        {
            string value = System.IO.File.ReadAllText("/Files-Data/config.json");
            config = JsonSerializer.Deserialize<ConfigsContainer>(value) ?? new ConfigsContainer();
        }


        [Route("/teste")]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Sucesso!");
        }

        [Route("/update/{porta}")]
        [HttpPut]
        public IActionResult UpdateContainerDocker(string porta)
        {
            try
            {
                Config configPorta = config.configs.ToList().FirstOrDefault(item => item.nome == porta) ?? new Config();
                if(configPorta.dirScript == null)
                {
                    return BadRequest("Error ao procurar o diretorio! Porta não encontrada!");
                }

                string Script = configPorta.dirScript;

                Run($"{ditScripts}{Script}", false);
                return Ok($"Comando executado!");
            }
            catch (Exception ex)
            {
                return BadRequest("Error ao procurar o diretorio!");
            }
        }

        [Route("/getScriptName/{porta}")]
        [HttpGet]
        public IActionResult GetScriptName(string porta)
        {
            try
            {
                Config configPorta = config.configs.ToList().FirstOrDefault(item => item.nome == porta) ?? new Config();
                if (configPorta.dirScript == null)
                {
                    return BadRequest("Error ao procurar o diretorio! Porta não encontrada!");
                }

                string Script = configPorta.dirScript;
                return Ok($"Achado: {ditScripts}{Script}");
            }
            catch (Exception ex)
            {
                return BadRequest("Error ao procurar o diretorio!");
            }
        }


        public static string Run(string cmd, bool sudo = false)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{(sudo ? "sudo " : "")}{cmd}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                };

                using (Process proc = new Process { StartInfo = psi })
                {
                    proc.Start();

                    string output = proc.StandardOutput.ReadToEnd();
                    string error = proc.StandardError.ReadToEnd();
                    proc.WaitForExit();

                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        Console.WriteLine($"Error: {error}");
                        return $"Error: {error}";
                    }

                    Console.WriteLine($"Output: {output}");
                    return output;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Command '{cmd}' failed: {exc.Message}");
                return $"Exception: {exc.Message}";
            }
        }
    }
}
