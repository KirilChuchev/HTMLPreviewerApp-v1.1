namespace HTMLPreviewerApp.Helper_Services
{
    using System;
    using System.Text;

    public static class DiscSizeEstimator
    {
        public static double Estimate(string rawHtmlCode)
        {
            var size = 0.0;

            if (rawHtmlCode == null)
            {
                return size;
            }
            var byteArr = Encoding.UTF8.GetBytes(rawHtmlCode.Trim());

            size = Math.Round(((byteArr.Length * 1.0) / 1024) / 1024, 2);

            return size;
        }
    }
}
