namespace Core.Application.Profiles.Commands.MarkProfileAsIntroduced
{
    public interface IMarkProfileAsIntroducedCommand
    {
        void Execute(MarkProfileAsIntroducedModel model);
    }
}