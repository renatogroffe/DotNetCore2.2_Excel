using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ExtracaoCotacoes
{
    public class PaginaCotacoes
    {
        private SeleniumConfigurations _configurations;
        private IWebDriver _driver;

        public PaginaCotacoes(SeleniumConfigurations seleniumConfigurations)
        {
            _configurations = seleniumConfigurations;

            ChromeOptions optionsFF = new ChromeOptions();
            optionsFF.AddArgument("--headless");

            _driver = new ChromeDriver(
                _configurations.CaminhoDriverChrome
                , optionsFF);
        }

        public void CarregarPagina()
        {
            _driver.Manage().Timeouts().PageLoad =
                TimeSpan.FromSeconds(60);
            _driver.Navigate().GoToUrl(
                _configurations.UrlPaginaCotacoes);
        }

        public List<Cotacao> ObterCotacoes()
        {
            List<Cotacao> cotacoes = new List<Cotacao>();

            var tableCotacoes = _driver.FindElement(
                By.ClassName("currencies"));
            var rowsCotacoes = tableCotacoes
                .FindElements(By.ClassName("info")).Take(3);
            foreach (var rowCotacao in rowsCotacoes)
            {
                var rowContent =
                    rowCotacao.FindElement(
                        By.TagName("a")).Text.Split("\r\n");
                Cotacao cotacao = new Cotacao();
                cotacao.NomeMoeda = rowContent[0];
                cotacao.Variacao = rowContent[1];
                cotacao.ValorCotacao = Convert.ToDouble(
                    rowContent[2]);

                cotacoes.Add(cotacao);
            }

            return cotacoes;
        }

        public void Fechar()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}