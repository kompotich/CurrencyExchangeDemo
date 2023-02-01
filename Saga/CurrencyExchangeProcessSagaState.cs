using MassTransit;

namespace CurrencyExchange.Saga
{
    public sealed class CurrencyExchangeProcessSagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public string? CurrenctState { get; set; }

        public Guid? RequestId { get; set; }

        public Uri? ResponseAddress { get; set; }
    }
}
