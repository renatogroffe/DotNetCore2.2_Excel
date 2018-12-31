using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ExtracaoCotacoes
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json");
            var configuration = builder.Build();


            Console.WriteLine("Iniciando a extração das cotações...");

            DateTime dataHoraExtracao = DateTime.Now;

            var seleniumConfigurations = new SeleniumConfigurations();
            new ConfigureFromConfigurationOptions<SeleniumConfigurations>(
                configuration.GetSection("SeleniumConfigurations"))
                    .Configure(seleniumConfigurations);

            var pagina = new PaginaCotacoes(seleniumConfigurations);
            pagina.CarregarPagina();
            var cotacoes = pagina.ObterCotacoes();


            Console.WriteLine("Gerando o arquivo .xlsx (Excel) com as cotações...");
            var excelConfigurations = new ExcelConfigurations();
            new ConfigureFromConfigurationOptions<ExcelConfigurations>(
                configuration.GetSection("ExcelConfigurations"))
                    .Configure(excelConfigurations);

            string arquivoXlsx = ArquivoExcelCotacoes.GerarArquivo(
                excelConfigurations, dataHoraExtracao, cotacoes);
            Console.WriteLine($"O arquivo {arquivoXlsx} foi gerado com sucesso!");

            Console.ReadKey();
        }
    }
}