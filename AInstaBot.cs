using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Diagnostics;
using System.Threading;

namespace InstaLike
{
    public abstract class AInstaBot
    {

        enum AutomationType
        {
            Follow,
            HashTagLike
        }

        enum BotState
        {
            Run,
            Sleep
        }

        private const int WaitCommentLimitationToPass = 10;
        private const int CommentLimitation = 15;
        private static IWebDriver driver;
        private static IWebElement element = null;

        protected static int commentQuery;
        protected static int hashTagQuery;
        protected static int totaLike;
        protected static int sessionCommented;

        protected static void Begin()
        {
            try
            {
                Proxy proxy = new Proxy();
                proxy.Kind = ProxyKind.Manual;
                driver = new ChromeDriver();
                driver.Manage().Window.Maximize();
                //(driver as ChromeDriver).NetworkConditions = new OpenQA.Selenium.Chromium.ChromiumNetworkConditions()
                //{
                //    DownloadThroughput = 25 * 10000,
                //    UploadThroughput = 10 * 10000,
                //    Latency = TimeSpan.FromMilliseconds(50)
                //};
                driver.Navigate().GoToUrl("https://www.instagram.com/accounts/login/");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static bool WaitElementToLoad_CLICK(string Xpath)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                OpenQA.Selenium.Support.UI.WebDriverWait wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(15));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(Xpath)));

                element = driver.FindElement(By.XPath(Xpath));
                element.Click();

                if (element != null)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"=================================================================");
                    Console.WriteLine($"=================================================================");
                    Console.ForegroundColor = ConsoleColor.Green;
                    stopwatch.Stop();
                    Console.WriteLine($"PROCESS PASS WITH NOT TICKS IN => {stopwatch.Elapsed.Milliseconds} MILLISECONDS");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"=================================================================");
                    Console.WriteLine($"=================================================================");
                    return true;

                }
            }
            catch (Exception)
            {

            }


            return false;



        }
        protected static bool commentLimitAchieved = false;
        /// <summary>
        /// Main Comment-Like Loop Core
        /// </summary>
        /// <param name="XpathCore"></param>
        /// <param name="loopCount"> Warning ! loopCount must be modded by 3 == 0</param>
        public static void WaitElementToLoad_CLICKLoop(string XpathCore, int loopCount)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            OpenQA.Selenium.Support.UI.WebDriverWait wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(15));

            const string exitButton = "/html/body/div[6]/div[1]/button";
            const string likeButton = "/html/body/div[6]/div[3]/div/article/div/div[2]/div/div/div[2]/section[1]/span[1]/button";
            const string comment = "/html/body/div[6]/div[3]/div/article/div/div[2]/div/div/div[2]/section[3]/div/form/textarea";
            const string commentOpenButton = "/html/body/div[6]/div[3]/div/article/div/div[2]/div/div/div[2]/section[1]/span[2]/button";
            const string share = "/html/body/div[6]/div[3]/div/article/div/div[2]/div/div/div[2]/section[3]/div/form/button";
            const string commentBlocked = "//*[contains(text(), 'Bu gönderideki yorumlar sınırlandırıldı.')]";
            loopCount = loopCount / 3;
            Random randomTime = new Random();

            if (commentLimitAchieved)
            {
                Thread.Sleep(WaitCommentLimitationToPass * 60 * 1000);
                commentLimitAchieved = false;
            }

            for (int i = 1; i <= loopCount; i++)
            {

                for (int j = 1; j <= 3; j++)
                {
                    try
                    {
                        Console.WriteLine($"Step 1 Select Foto");

                        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath($"//*[@id=\"react-root\"]/section/main/article/div[2]/div/div[{i}]/div[{j}]")));
                        element = driver.FindElement(By.XPath($"//*[@id=\"react-root\"]/section/main/article/div[2]/div/div[{i}]/div[{j}]"));
                        element.Click();
                        
                        Console.WriteLine($"Step 2 Like Foto");
                        Thread.Sleep(randomTime.Next(10000, 15000));
                        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(likeButton)));
                        element = driver.FindElement(By.XPath(likeButton));
                        element.Click();
                        totaLike++;
                        // TODO : Switch Bot To Sleeping Mode

                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine($"=================================================================");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("    o o.o o   ");
                        Console.WriteLine("  o         o      ");
                        Console.WriteLine("  o         o     ");
                        Console.WriteLine("   o       o         ");
                        Console.WriteLine("     o    o       ");
                        Console.WriteLine("       o         ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"Total Like Count =  {totaLike}");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine($"=================================================================");

                        if (!WaitElementToLoad_CHECK(commentBlocked))
                        {

                            Thread.Sleep(200);
                            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(commentOpenButton)));
                            element = driver.FindElement(By.XPath(commentOpenButton));
                            element.Click();
                            Console.WriteLine($"Step 3.5 Click For Comment");

                            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(comment)));
                            element = driver.FindElement(By.XPath(comment));
                            element.SendKeys(Directives.BotCommentList[commentQuery]);

                            Console.WriteLine($"Step 4 Select Comment : {Directives.BotCommentList[commentQuery]}");

                            commentQuery++;
                            if (commentQuery >= Directives.BotCommentList.Count)
                                commentQuery = 0;


                            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(share)));
                            element = driver.FindElement(By.XPath(share));
                            element.Click();
                            Console.WriteLine($"Step 5 Send Comment");
                            sessionCommented++;
                        }


                    }


                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error {ex.Message}");
                    }
                    finally
                    {
                        Console.WriteLine($"Step 6 Exit PopUp Page Finally");
                        
                        if (sessionCommented >= CommentLimitation)
                        {
                            commentLimitAchieved = true;
                            sessionCommented = 0;
                        }

                        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(exitButton)));
                        element = driver.FindElement(By.XPath(exitButton));
                        element.Click();

                        
                    }

                }
            }


        }


        public static bool WaitElementToLoad_CHECK(string Xpath)
        {
            try
            {
                Console.WriteLine($"Step 3 Check Is Commentable");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                OpenQA.Selenium.Support.UI.WebDriverWait wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(Xpath)));
                element = driver.FindElement(By.XPath(Xpath));

                if (element != null)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"=================================================================");
                    Console.WriteLine($"=================================================================");
                    Console.ForegroundColor = ConsoleColor.Green;
                    stopwatch.Stop();
                    Console.WriteLine($"CHECKED PASS WITH NOT TICKS IN => {stopwatch.Elapsed.Milliseconds} MILLISECONDS");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"=================================================================");
                    Console.WriteLine($"=================================================================");
                    return true;

                }
            }
            catch (Exception)
            {
                return false;

            }

            return false;


        }

        public static bool WaitElementToLoad_SEND_AutoTap(string Xpath, string sendString)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            OpenQA.Selenium.Support.UI.WebDriverWait wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(Xpath)));

            element = driver.FindElement(By.XPath(Xpath));
            Thread.Sleep(10);
            element.SendKeys(sendString);
            Thread.Sleep(10);
            element.SendKeys(Keys.Enter);
            element.SendKeys(Keys.Enter);

            if (element != null)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"=================================================================");
                Console.WriteLine($"=================================================================");
                Console.ForegroundColor = ConsoleColor.Green;
                stopwatch.Stop();
                Console.WriteLine($"PROCESS PASS WITH NOT TICKS IN => {stopwatch.Elapsed.Milliseconds} MILLISECONDS");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"=================================================================");
                Console.WriteLine($"=================================================================");
                return true;

            }
            return false;


        }
        public static bool WaitElementToLoad_SendWithAutoClickMultiple(string Xpath, string sendString)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            OpenQA.Selenium.Support.UI.WebDriverWait wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(Xpath)));

            element = driver.FindElement(By.XPath(Xpath));
            Thread.Sleep(50);
            element.Clear();
            Thread.Sleep(50);
            element.SendKeys(sendString);
            string listPath = "//*[@id=\"react-root\"]/section/nav/div[2]/div/div/div[2]/div[3]/div/div[2]/div/div[1]";
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(listPath)));
            element = driver.FindElement(By.XPath(listPath));
            element.Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath($"//*[@id=\"react-root\"]/section/main/article/div[2]/div/div[{1}]/div[{1}]")));

            if (element != null)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"=================================================================");
                Console.WriteLine($"=================================================================");
                Console.ForegroundColor = ConsoleColor.Green;
                stopwatch.Stop();
                Console.WriteLine($"PROCESS PASS WITH NOT TICKS IN => {stopwatch.Elapsed.Milliseconds} MILLISECONDS");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"=================================================================");
                Console.WriteLine($"=================================================================");
                return true;

            }
            return false;


        }

        public static bool WaitElementToLoad_SEND(string Xpath, string sendString)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            OpenQA.Selenium.Support.UI.WebDriverWait wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(Xpath)));

            element = driver.FindElement(By.XPath(Xpath));
            element.SendKeys(sendString);

            if (element != null)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"=================================================================");
                Console.WriteLine($"=================================================================");
                Console.ForegroundColor = ConsoleColor.Green;
                stopwatch.Stop();
                Console.WriteLine($"PROCESS PASS WITH NOT TICKS IN => {stopwatch.Elapsed.Milliseconds} MILLISECONDS");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"=================================================================");
                Console.WriteLine($"=================================================================");
                return true;

            }
            return false;



        }
    }
}