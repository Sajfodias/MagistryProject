using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Http;
using System.IO;
using HtmlAgilityPack;
using System.Diagnostics;
using System.Threading;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Wyszukiwarka_publikacji_v0._2.Logic
{
    class Downloader
    {
        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32")]
        public static extern void FreeConsole();

        private readonly static HttpClient httpClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(40.0) };
        public static HtmlDocument contentHtmlDoc;
        public static HtmlDocument downloadedContentHtmlDoc;
        //public static string path = @"F:\\Magistry files\Magistry_new_test_data\";
        public static string bibtexPath = @"F:\\Magistry files\html_articles_to_txt\";
        //public static string newbibtexPath = @"F:\\Magistry files\new_Magistry_test_data\";
        public static string newbibtexPath = @"F:\\Magistry files\1\";
        static int countOfArticles = 141022;//141022; //tu potrzebnie zaimplementowac algorytm znajdowania ostatniego id
        public static Queue<Task> tasks = new Queue<Task>();
    
        public static async Task<HtmlDocument> getContent(string url)
        {
            contentHtmlDoc = new HtmlDocument();
      
            try
            {
                using (HttpResponseMessage response =  await httpClient.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                        using (HttpContent content = response.Content)
                        {
                                    HtmlWeb web = new HtmlWeb();                                   
                                    contentHtmlDoc =  web.Load(url);
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There is an exception: { ex } for query: { url } occured!");
                Console.ReadKey();
            }
            return contentHtmlDoc;
        }

        public static void downloadBibtexFile1(string bibtexURL, int index)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(bibtexURL, newbibtexPath + index.ToString() + ".bib");
                }
                    Debug.WriteLine(string.Format("DownloadFileTaskAsync (downloaded): {0}", bibtexPath + index.ToString() + ".bib"));
                }
            catch(Exception ex)
            {
                Debug.WriteLine($"The exception {0} occered.", ex.ToString());
            }
         }
             
        public static async Task downloadBibtexFile(string bibtexUrl, int index)
        {
            int timeOut = 3000;
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    TimerCallback timerCallback = c =>
                    {
                        var client = (WebClient)c;
                        if (!client.IsBusy) return;
                        client.CancelAsync();
                        Debug.WriteLine(string.Format("DownloadFileTaskAsync (time out due): {0}", bibtexUrl));
                    };
                    using (var timer = new Timer(timerCallback, webClient, timeOut, Timeout.Infinite))
                    {
                        Uri newUri = new Uri(bibtexUrl);
                        await Task.Run(() =>
                        {
                            webClient.DownloadFileAsync(newUri, bibtexPath + index.ToString() + ".bib");
                        });
                        //await webClient.DownloadFileTaskAsync(bibtexUrl, bibtexPath + index.ToString() + ".bib");
                    }
                    Debug.WriteLine(string.Format("DownloadFileTaskAsync (downloaded): {0}", bibtexPath + index.ToString() + ".bib"));
                }
                /*Uri newUri = new Uri(bibtexUrl);

                await Task.Run(() => {
                    webClient.DownloadFileAsync(newUri, bibtexPath + index.ToString() + ".bib");
                }); 
                webClient.Dispose();*/
                
            }
            
            catch (Exception ex)
            {
                Debug.WriteLine($"The exception {0} occured!", ex);
            }
        }

        public static void bruteForce()
        {
            int maxCountOfThreads = 5; //nie widze duzej roznicy jezeli tu jest 5
            var options = new ParallelOptions() { MaxDegreeOfParallelism = maxCountOfThreads };
            try
            {
                for (int z = 0; z <= countOfArticles; z++)
                {
                    //tu zmiany po 100 a nie po 1000
                    int beginRegion = z * 100 + 1;
                    int endRegion = (z + 1) * 100;
                    /*
                    Parallel.For(beginRegion, endRegion, options, async l =>
                    {
                        downloadedContentHtmlDoc = await getContent($"http://pg.edu.pl/publikacje?id=" + l.ToString());
                        using (StreamWriter outputfile = new StreamWriter(File.Create(bibtexPath + l.ToString() + ".txt")))
                        {
                            outputfile.WriteLine(downloadedContentHtmlDoc.DocumentNode.InnerText);
                            outputfile.Flush();
                        }
                        //await downloadBibtexFile("http://pg.edu.pl/publikacje?p_p_id=3_WAR_espeosciportlet&p_p_lifecycle=2&p_p_state=normal&p_p_mode=view&p_p_cacheability=cacheLevelPage&p_p_col_id=column-1&p_p_col_count=1&_3_WAR_espeosciportlet_publicationId=" + l.ToString() + "&_3_WAR_espeosciportlet_action=bib", l);
                        /*
                        Task.CompletedTask.Dispose();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                        
                    });
                    */

                    Parallel.For(beginRegion, endRegion, options, x => {
                        downloadBibtexFile("http://pg.edu.pl/publikacje?p_p_id=3_WAR_espeosciportlet&p_p_lifecycle=2&p_p_state=normal&p_p_mode=view&p_p_cacheability=cacheLevelPage&p_p_col_id=column-1&p_p_col_count=1&_3_WAR_espeosciportlet_publicationId=" + x.ToString() + "&_3_WAR_espeosciportlet_action=bib", x);
                    });
                    //chyba wiem w czym problem na raz probuje ściągnąć zadużo plików bibtex
                    //gdzieś tu musimy czyścić pamięć i zoptymalizować kod;
                    Thread.Sleep(3000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception {0} was occured.", ex.ToString());
                Console.ReadKey();
            }
        }
    }
}
