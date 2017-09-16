using System;
using System.Net;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace TransientFaultHandlingSample
{
    class Program
    {
        const int RETRY_COUNT = 5;

        static void Main(string[] args)
        {
            try
            {
                var retryStrategy = new Incremental(RETRY_COUNT, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2));

                var retryPolicy = new RetryPolicy<CustomTransientErrorDetectionStrategy>(retryStrategy);

                retryPolicy.ExecuteAction(NavigateTo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static private void NavigateTo()
        {
            Console.WriteLine(DateTime.Now);

            WebClient wc = new WebClient();
            wc.DownloadString("c:\\temp.txt");
        }
    }

    internal class CustomTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        public bool IsTransient(Exception ex)
        {
            if (ex is WebException)
                return true;
            return false;
        }
    }
}
