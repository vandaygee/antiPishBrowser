using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using HtmlAgilityPack;




namespace AntiPhishBrowser
{
   public class antiPhisEngine
    {
       string url;
       public HtmlWeb web;
       public HtmlDocument doc;
       public Uri uri;

       public static string url1;

       public antiPhisEngine(string URL)
       {
           this.url = URL;
       }

       public void startAntiPhishEngine()
       {

           if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
           {
               url = @"http://" + url;
               uri = new Uri(url);
           }

           if (url.Contains(@"file://"))
           {
               url1 = url;
           }
           else
           {
               var connectionType = uri.HostNameType.ToString();
               if (connectionType == "Dns")
               {
                   if (DoDotCount(url) > 3)
                   {
                       string path = Environment.CurrentDirectory + @"\sites\doterror.html";
                       url1 = url;
                   }
                   else if (url.Contains("@"))
                   {
                       string path = Environment.CurrentDirectory + @"\sites\aterror.html";
                       url1 = path;
                   }
                   else
                   {
                       //url1 = new Uri(url);
                       try
                       {
                           HtmlNode.ElementsFlags.Remove("form");
                           web = new HtmlWeb();
                           doc = web.Load(url);

                           if (CheckLinkValidity(uri.Host))
                           {
                               if (checkFormActionValidity(uri.Host))
                               {
                                   //string path = Environment.CurrentDirectory + @"\sites\dnserror.html";
                                   url1 = url;
                                   //new Thread(new MyProxy(ctx).ProcessRequest).Start();
                               }
                           }
                       }
                       catch (Exception ex)
                       { }
                   }
               }
               else
               {
                   string path = Environment.CurrentDirectory + @"\sites\dnserror.html";
                   url1 = path;
               }
           }


           
       }

       public int DoDotCount(string url)
       {
           int counter = 0;
           foreach (char chr in url)
           {
               if (chr == '.')
               {
                   counter += 1;
               }
           }
           return counter;
       }

       public bool CheckLinkValidity(string host)
       {
           bool ret = false;
           if (doc.DocumentNode.SelectNodes("//a[@href]") != null)
           {
               HtmlNodeCollection node = doc.DocumentNode.SelectNodes("//a[@href]");
               int nooflinks = node.Count;
               //string action = node[0].Attributes["href"].Value;
              // Console.WriteLine("Total Link on Page: " + nooflinks);
               int count = 0;
               foreach (HtmlNode nd in node)
               {
                   if (nd.Attributes["href"].Value.Contains(host) || nd.Attributes["href"].Value[0] == '/')
                   {
                       count++;
                   }
               }

               //Console.WriteLine("Valid links: " + count + "/" + nooflinks);
               float percent = (float)count / nooflinks * 100;
               //Console.WriteLine("Valid links (%): {0:f2}", percent);
               if (percent >= 75.0) { ret = true; }
               else
               {
                   string path = Environment.CurrentDirectory + @"\sites\linkerror.html";
                   url1 = path;
                   //StringBuilder sb = new StringBuilder();
                   //sb.Append("<html><body><h1>Phising Act Detected on " + url + "</h1>");
                   //sb.Append("<br/><p> The proxy server blocked you from visiting web page");
                   //sb.Append("<br/>Reason:");
                   //sb.Append("<br><b>Number of links found on page: " + nooflinks);
                   //sb.Append("<br>" + count + "/" + nooflinks + " is found valid");
                   //sb.Append("<br>" + string.Format("{0:f2}% of link is valid", percent) + "</b>");
                   //sb.Append("</body></html>");
               }
           }

           return ret;
       }

       public bool checkFormActionValidity(string host)
       {
           bool ret = false;
           if (doc.DocumentNode.SelectNodes("//form[@action]") != null)// && doc.DocumentNode.SelectNodes("//a[@href]").Count != 0)
           {
               //int nodeCount = doc.DocumentNode.SelectNodes("//a[@href]").Count;
               HtmlNodeCollection node = doc.DocumentNode.SelectNodes("//form[@action]");
               int noofforms = node.Count;
               string action = node[0].Attributes["action"].Value;
               //Console.WriteLine("Total Form found on page: " + noofforms);
               int count = 0;
               foreach (HtmlNode nd in node)
               {
                   if (nd.Attributes["action"].Value.Contains(host) || nd.Attributes["action"].Value[0] == '/')
                   {
                       count++;
                   }
               }
               //Console.WriteLine("Valid Form: " + count + "/" + noofforms);
               float percent = (float)count / noofforms * 100;
               //Console.WriteLine("Valid form (%): {0:f2}%", percent);
               if (percent == 100.0) { ret = true; }
               else
               {
                   string path = Environment.CurrentDirectory + @"\sites\formerror.html";
                   url1 = path;
                   //StringBuilder sb = new StringBuilder();
                   //sb.Append("<html><body><h1>Phising Act Detected on " + ctx.Request.RawUrl + "</h1>");
                   //sb.Append("<br/><p> The proxy server blocked you from visiting web page");
                   //sb.Append("<br/>Reason:");
                   //sb.Append("<br><b>Number of form found on page: " + noofforms);
                   //sb.Append("<br>" + count + "/" + noofforms + " is found valid");
                   //sb.Append("<br>" + string.Format("{0:f2}% of form is valid", percent) + "</b>");
                   //sb.Append("</body></html>");
               }
           }
           return ret;
       }

       public string URL
       {
           get { return url1; }
       }

    }
}
