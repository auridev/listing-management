namespace BusinessLine.Core.Application.Messages.Commands.SendMessage
{
    public interface ISendMessageCommand
    {
        void Execute(SendMessageModel model);
    }
}