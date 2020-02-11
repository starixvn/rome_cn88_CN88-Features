namespace CN88.Features.Shared.Services
{
    using System;
    using CN88.Features.Shared.Criteria;
    using Fanex.Data.Repository;

    public interface HeadlineService
    {
        bool IsCustomerBelonging(int headlineId, int custId, DateTime from, DateTime to);
    }

    public class HeadlineServiceImpl : HeadlineService
    {
        private readonly IDynamicRepository dynamicRepository;

        public HeadlineServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public bool IsCustomerBelonging(int headlineId, int custId, DateTime from, DateTime to)
        {
            var command = new CheckHeadlineBelongingByDateCommand(headlineId, custId, from, to);
            dynamicRepository.Execute(command);

            return command.IsBelonging;
        }
    }
}
