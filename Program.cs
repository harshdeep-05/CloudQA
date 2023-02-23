using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        IWebDriver driver = new ChromeDriver();
        driver.Navigate().GoToUrl("https://app.cloudqa.io/home/AutomationPracticeForm");

        // find all elements in the main document
        var elements = driver.FindElements(By.CssSelector("*[id], *[name], *[class], input[type='text'], input[type='email'], input[type='password'], input[type='checkbox'], input[type='radio'], select, textarea"));

        foreach (var element in elements)
        {
            try
            {
                element.SendKeys("test");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // find all iframes and switch to them
        var iframes = driver.FindElements(By.TagName("iframe")).ToList();
        for (int i = 0; i < iframes.Count; i++)
        {
            driver.SwitchTo().Frame(i);
            var iframeElements = driver.FindElements(By.CssSelector("*[id], *[name], *[class], input[type='text'], input[type='email'], input[type='password'], input[type='checkbox'], input[type='radio'], select, textarea")).ToList();
            for (int j = 0; j < 3 && j < iframeElements.Count; j++)
            {
                try
                {
                    iframeElements[j].SendKeys("test");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            driver.SwitchTo().ParentFrame();
        }

        // find all shadow roots and switch to them
        var shadowRootElements = driver.FindElements(By.CssSelector("*"))
            .Where(element => (bool)(driver as IJavaScriptExecutor)
                .ExecuteScript("return arguments[0].shadowRoot !== null", element))
            .ToList();

        for (int i = 0; i < shadowRootElements.Count; i++)
        {
            var shadowRoot = (IWebElement)(driver as IJavaScriptExecutor)
                .ExecuteScript("return arguments[0].shadowRoot", shadowRootElements[i]);
            var shadowElements = shadowRoot.FindElements(By.CssSelector("*[id], *[name], *[class], input[type='text'], input[type='email'], input[type='password'], input[type='checkbox'], input[type='radio'], select, textarea")).ToList();
            for (int j = 0; j < 3 && j < shadowElements.Count; j++)
            {
                try
                {
                    shadowElements[j].SendKeys("test");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        driver.Quit();
    }
}

