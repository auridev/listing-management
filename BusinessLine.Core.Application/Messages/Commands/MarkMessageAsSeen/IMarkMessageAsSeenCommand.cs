namespace BusinessLine.Core.Application.Messages.Commands.MarkMessageAsSeen
{
    public interface IMarkMessageAsSeenCommand
    {
        void Execute(MarkMessageAsSeenModel model);
    }
}