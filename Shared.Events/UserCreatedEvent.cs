namespace Shared.Events
{
    public record UserCreatedEvent
    {
        public int Id { get; init; }

        public string Name { get; init; } = default!;

        public string Email { get; init; } = default!;
    }
}