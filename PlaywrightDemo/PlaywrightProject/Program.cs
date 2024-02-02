using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

class Program
{
    public static async Task Main()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        var page = await browser.NewPageAsync();
        await page.GotoAsync("https://quotes.toscrape.com/");

        var heading = await page.QuerySelectorAsync("//h1/a");
        if (heading != null)
        {
            Console.WriteLine(await heading.InnerTextAsync());
        }

        var login = await page.QuerySelectorAsync("[href=\"/login\"]");
        if (login != null)
        {
            await login.ClickAsync();
        }

        var user_input = await page.QuerySelectorAsync("[id=\"username\"]");
        if (user_input != null)
        {
            await user_input.FillAsync("userstring");
        }

        var pass_input = await page.QuerySelectorAsync("//*[text()=\"Password\"]/following-sibling::*");
        if (pass_input != null)
        {
            await pass_input.FillAsync("text");
        }

        await Task.Delay(4000);

        var submitButton = await page.QuerySelectorAsync("[type=\"submit\"]");
        if (submitButton != null)
        {
            await submitButton.ClickAsync();
        }

        string selector = "//*[@href=\"/logout\"]";
        IElementHandle? logout;
        try
        {
            logout = await page.WaitForSelectorAsync(selector);
        }
        catch
        {
            Console.WriteLine("login failed");
            return;
        }
        if (logout != null)
        {
            Console.WriteLine(await logout.InnerTextAsync());
        }

        var quotes = await page.QuerySelectorAllAsync("[class=\"quote\"]");
        foreach (var quote in quotes)
        {
            var textElement = await quote.QuerySelectorAsync(".text");
            if (textElement != null)
            {
                Console.WriteLine(await textElement.InnerTextAsync());
            }
        }

        // Crea un await take screenshoot para tomar una captura de pantalla
        await page.ScreenshotAsync(new PageScreenshotOptions { Path = "screenshot.png" });

        
        await Task.Delay(4000);

        await browser.CloseAsync();
    }
}