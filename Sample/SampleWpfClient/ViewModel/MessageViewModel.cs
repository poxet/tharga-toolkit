namespace SampleWpfClient.ViewModel
{
    public class MessageViewModel
    {
        public string Message { get; private set; }

        public MessageViewModel(string message)
        {
            Message = message;
        }
    }
}