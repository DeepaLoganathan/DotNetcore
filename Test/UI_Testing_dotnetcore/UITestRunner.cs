using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Threading;
using NUnit.Framework;
namespace SeleniumMasterWebDriverDotNetTestSuiteAutomation
{
    class TestRunner
    {
        private static IWebDriver browser;
        private static String baseUrl;

        static void Main(string[] args)
        {
            //create a new test
            TestRunner test = new TestRunner();
            //execute Setup method
            test.SetUp();
            //run Launch Site and Login Method
            test.launchSiteAndLogin();
            //run Open User Setting Page Method
            test.openUserSettingPage();
            //run Change User Setting Method
            test.ChangeUserSettings();
            //run Logout Method
            test.Logout();
            //teardown the browser
            test.TearDown();
        }
        //setup
        public void SetUp()
        {
            //Initialize test result string
            testResultUtility.InitializeTestResultString("Selenium Master Login Test Suite");
            //test base url
            baseUrl = "http://www.seleniummaster.com";

            try
            {
                //use firefox browser
                browser = new FirefoxDriver();
                //set browser time out time to 60 seconds
                browser.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(60));
                //add test result to test result string when passed
                testResultUtility.AddTestPassToTestResultString("Test Setup", "Pass");

            }
            catch (Exception e)
            {
                //add message to the console
                Console.WriteLine(e.Message);
                Console.WriteLine("Cannot start Web Driver and Launch the browser");
                //add test result to test result string when failed
                testResultUtility.AddTestFailToTestResultString("Test Setup", "Fail");
            }

        }

        //Launches the Selenium Master Test Application and Login
        public void launchSiteAndLogin()
        {
            browser.Navigate().GoToUrl(baseUrl + "/seleniummastertestapp/index.php");
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (isElementPresent(By.CssSelector("img[alt=\"Selenium Master\"]")))
                        break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Thread.Sleep(1000);
            }

            browser.FindElement(By.Id("login_login_username")).Clear();
            browser.FindElement(By.Id("login_login_username")).SendKeys("test");
            browser.FindElement(By.Id("login_login_password")).Clear();
            browser.FindElement(By.Id("login_login_password")).SendKeys("XXXX"); //password is omitted
            browser.FindElement(By.Id("login_submit")).Click();
            try
            {
                Assert.AreEqual(browser.FindElement(By.CssSelector("ul.cr > li > a")).Text, "Test Selenium");
                testResultUtility.AddTestPassToTestResultString("Launch Site And Login Test", "Pass");
            }

            catch
            {
                testResultUtility.AddTestFailToTestResultString("Launch Site And Login Test", "Fail");
            }
        }

        //Navigates to the User Settings page
        public void openUserSettingPage()
        {
            browser.FindElement(By.LinkText("Settings")).Click();

            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (isElementPresent(By.Id("login_login_username")))
                        break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Thread.Sleep(1000);
            }

            browser.FindElement(By.Id("login_login_username")).Clear();
            browser.FindElement(By.Id("login_login_username")).SendKeys("test");
            browser.FindElement(By.Id("login_login_password")).Clear();
            browser.FindElement(By.Id("login_login_password")).SendKeys("XXXX");//password is omitted
            browser.FindElement(By.Id("login_submit")).Click();

            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (isElementPresent(By.XPath("//input[@value='auth']")))
                        break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Thread.Sleep(1000);
            }
            try
            {
                Assert.IsTrue(isElementPresent(By.XPath("//input[@value='auth']")));
                testResultUtility.AddTestPassToTestResultString("Open User Setting Page Test", "Pass");
            }
            catch
            {
                testResultUtility.AddTestFailToTestResultString("Open User Setting Page Test", "Fail");

            }


        }

        //Change a User settings to add as a friends after authorization")
        public void ChangeUserSettings()
        {
            browser.FindElement(By.XPath("//input[@value='auth']")).Click();
            browser.FindElement(By.Id("accountprefs_submit")).Click();
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (isElementPresent(By.CssSelector("div.ok")))
                        break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Thread.Sleep(1000);
            }
            try
            {
                Assert.AreEqual("Preferences saved", browser.FindElement(By.CssSelector("div.ok")).Text);
                testResultUtility.AddTestPassToTestResultString("Change User Settings Test", "Pass");
            }

            catch
            {
                testResultUtility.AddTestFailToTestResultString("Change User Settings Test", "Fail");
            }

        }

        //Log out the system
        public void Logout()
        {
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (isElementPresent(By.LinkText("Logout")))
                        break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Thread.Sleep(1000);
            }
            browser.FindElement(By.LinkText("Logout")).Click();
            try
            {
                Assert.IsTrue(isElementPresent(By.Id("login_login_username")));
                testResultUtility.AddTestPassToTestResultString("Logout Test", "Pass");
            }
            catch

            {
                testResultUtility.AddTestFailToTestResultString("Logout Test", "Fail");
            }

        }


        //tear down
        public void TearDown()
        {
            testResultUtility.EndTestResultString();
            testResultUtility.WriteToHtmlFile(testResultUtility.testResultHtmlString.ToString(), "SeleniumMasterLoginTestResult.html");
            browser.Quit();
        }

        private bool isElementPresent(By by)
        {
            try
            {
                browser.FindElement(by);
                return true;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }

}