namespace FruitSA.Web.Providers
{    
    public class UniqueCodeGenerator
    {
        private static int sequenceNumber = 1;

        public static string GenerateUniqueCode()
        {
            string yearMonth = DateTime.Now.ToString("yyyyMM");
            string sequentialCode = GetSequentialCode();

            return $"{yearMonth}-{sequentialCode}";
        }

        private static string GetSequentialCode()
        {
            lock (typeof(UniqueCodeGenerator))
            {
                string sequentialCode = sequenceNumber.ToString().PadLeft(3, '0');
                sequenceNumber = (sequenceNumber % 999) + 1; // Reset sequence if it reaches 999
                return sequentialCode;
            }
        }
    }
}
