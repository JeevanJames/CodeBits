using System;
using System.Net;

using Xunit;

namespace CodeBits.Tests
{
    public sealed class WebScrapperTests
    {
        [Fact]
        public void DevTest()
        {
            var ws = new WebScrapper();
            ws.Get("http://moodle.cbayleap.com/");
            foreach (Cookie cookie in ws.Cookies)
                Console.WriteLine("{0}={1} ({2})", cookie.Name, cookie.Value, cookie.Domain);
            Console.WriteLine();
            ws.Get("http://moodle.cbayleap.com/login");
            foreach (Cookie cookie in ws.Cookies)
                Console.WriteLine("{0}={1} ({2})", cookie.Name, cookie.Value, cookie.Domain);
            Console.WriteLine();
            ws.Post("http://moodle.cbayleap.com/login/index.php", QueryString.Create("username", "admin").Append("password", "Pa55word$"));
            foreach (Cookie cookie in ws.Cookies)
                Console.WriteLine("{0}={1} ({2})", cookie.Name, cookie.Value, cookie.Domain);
        }
    }
}