using CurrencyExchange.ExchangeData.Queue;
using MassTransit;

namespace CurrencyExchange.Saga
{
    public sealed class CurrencyExchangeProcessSaga : MassTransitStateMachine<CurrencyExchangeProcessSagaState>
    {
        public CurrencyExchangeProcessSaga()
        {
            InstanceState(x => x.CurrenctState);

            Event(() => Process, x => x.CorrelateById(y => y.Message.Id));

            Request(() => Convert);
            Request(() => Store);

            Initially(When(Process).Then(x =>
            {
                x.Saga.RequestId = x.GetPayload<SagaConsumeContext<CurrencyExchangeProcessSagaState, CurrencyRateToRubleList>>().RequestId;
            })
                .Request(Convert, x => x.Init<CurrencyRateToRubleList>(new { x.Message.Id, x.Message.Values }))
                .TransitionTo(Convert.Pending));

            During(Convert.Pending,
                When(Convert.Completed)
                    .Request(Store, x => x.Init<CurrencyRateItemList>(new { x.Message.Id, x.Message.Values }))
                    .TransitionTo(Store.Pending),
                When(Convert.Faulted)
                    .ThenAsync(async x => await RespondeProcessResponse(x, x.Message.Exceptions, x.Message.Message.LastProcessRequestDate))
                    .TransitionTo(Failed));

            During(Store.Pending,
                When(Store.Completed)
                    .ThenAsync(async x => await RespondeProcessResponse(x, null, null))
                    .Finalize(),
                When(Convert.Faulted)
                    .ThenAsync(async x => await RespondeProcessResponse(x, x.Message.Exceptions, x.Message.Message.LastProcessRequestDate))
                    .TransitionTo(Failed));
        }

        public Event<CurrencyRateToRubleList> Process { get; set; }
        public Request<CurrencyExchangeProcessSagaState, CurrencyRateToRubleList, CurrencyRateItemList> Convert { get; set; }
        public Request<CurrencyExchangeProcessSagaState, CurrencyRateItemList, CurrencyRateItemList> Store { get; set; }

        public State Failed { get; set; }

        private static async Task RespondeProcessResponse<T>(
            BehaviorContext<CurrencyExchangeProcessSagaState, T> context, 
            IEnumerable<ExceptionInfo> exceptions,
            DateTime? lastProcessRequestDate)
            where T : class
        {
            var endpoint = await context.GetSendEndpoint(context.Saga.ResponseAddress);
            await endpoint.Send(new ProcessResponse
            {
                RequestId = context.Saga.CorrelationId,
                Errors = exceptions.Select(x => x.Message).ToList(),
                LastProcessRequestDate = lastProcessRequestDate,
            }, r => r.RequestId = context.Saga.RequestId);
        }
    }
}
