using System;
using System.Threading;

namespace InstaLike
{
    class Program : AInstaBot
    {


        static string searchTarget = "#explore";
        const string userName = "farmingbook";
        const string password = "botdeneme230322";
        const string LoginBox = "//*[@id=\"loginForm\"]/div/div[1]/div/label/input";
        const string PWBox = "//*[@id=\"loginForm\"]/div/div[2]/div/label/input";
        const string LoginButton = "//*[@id=\"loginForm\"]/div/div[3]";
        const string Notfications = "//*[contains(text(), 'Şimdi Değil')]";
        const string SearchBar = "//*[@id=\"react-root\"]/section/nav/div[2]/div/div/div[2]/input";
        const string ClickSearched = "//*[@id=\"react-root\"]/section/nav/div[2]/div/div/div[2]/div[3]/div/div[2]/div/div[1]";
        const string FollowButton = "//*[@id=\"react-root\"]/section/main/div/header/section/div[1]/div[1]/div/div/button";
        const string FollowButtonPublic = "//*[@id=\"react-root\"]/section/main/div/header/section/div[1]/div[1]/div/div/div/span/span[1]/button";
        const string CheckArrowButton = "//*[@id=\"react-root\"]/section/main/div/header/section/div[1]/div[1]/div/div/div/span/span[2]";


        #region Like Hashs Prop

        static string PostClassXpathVulnabilyties = $"//*[@id=\"react-root\"]/section/main/article/div[2]/div/div[1]/div[1]/a/div";

        #endregion


        //17/05


        static void Main(string[] args)
        {
            Begin();

            if (WaitElementToLoad_SEND(LoginBox, userName))
            {
                if (WaitElementToLoad_SEND(PWBox, password))
                {
                    if (WaitElementToLoad_CLICK(LoginButton))
                    {
                        if (WaitElementToLoad_CLICK(Notfications))
                        {
                            if (WaitElementToLoad_CLICK(Notfications))
                            {
                                if (searchTarget.Contains("#"))
                                {
                                    while (true)
                                    {
                                        try
                                        {
                                            searchTarget = Directives.HashTags[hashTagQuery];
                                            Thread.Sleep(100);
                                            hashTagQuery++;

                                            if (hashTagQuery >= Directives.HashTags.Count)
                                                hashTagQuery = 0;

                                            if (WaitElementToLoad_SendWithAutoClickMultiple(SearchBar, searchTarget))
                                            {
                                                WaitElementToLoad_CLICKLoop(PostClassXpathVulnabilyties, 25);
                                            }

                                        }
                                        catch (Exception)
                                        {
                                            hashTagQuery++;
                                        }

                                    }
                                }
                                else
                                if (WaitElementToLoad_SEND_AutoTap(SearchBar, searchTarget))
                                {
                                    if (WaitElementToLoad_CLICK(ClickSearched))
                                    {
                                        if (WaitElementToLoad_CHECK(CheckArrowButton))
                                        {
                                            if (WaitElementToLoad_CLICK(FollowButtonPublic))
                                            {
                                                Console.WriteLine($"DONE Following PUBLIC  !");
                                            }

                                        }
                                        else
                                        if (WaitElementToLoad_CLICK(FollowButton))
                                        {
                                            Console.WriteLine($"DONE Following PRIVATE !");
                                        }
                                    }
                                }
                            }

                        }

                    }

                }

            }

        }


    }
}
