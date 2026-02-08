using Caliburn.Micro;

using System.Threading.Tasks;

namespace ParserApp.Controls
{
    internal class EnterCardNameViewModel : Screen
    {
        public EnterCardNameViewModel() 
        {
            DisplayName = Properties.Resources.Card;
        }
        public string CardName { get; set; }
        public Task Apply()
        {
            return TryCloseAsync(true);
        }

        public Task Cancel()
        {
            return TryCloseAsync(false);
        }
    }
}
