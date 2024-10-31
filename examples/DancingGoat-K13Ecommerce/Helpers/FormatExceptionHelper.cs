namespace DancingGoat.Helpers
{
    internal static class FormatExceptionHelper
    {
        public static object Format(Exception ex)
        {
            if (ex == null)
            {
                return null;
            }

            return new
            {
                ex.Message,
                Type = ex.GetType().Name,
                InnerException = Format(ex.InnerException)
            };
        }
    }
}
