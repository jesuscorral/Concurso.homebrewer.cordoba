using BeerContest.Domain.Models;
using BeerContest.Domain.Repositories;
using BeerContest.Infrastructure.Common.Abstractions;
using BeerContest.Infrastructure.Common.Implementations;
using BeerContest.Infrastructure.Firestore.FirestoreModels;
using Microsoft.Extensions.Logging;

namespace BeerContest.Infrastructure.Repositories
{
    public class ParticipantRepository : FirestoreRepositoryBase<Participant, FirestoreParticipant, string>, IParticipantRepository
    {
        protected override string CollectionName => "participants";
        
        public ParticipantRepository(IFirestoreContext context,
        ILogger<ParticipantRepository> logger)
            : base(context, logger)
        {
        }

        protected override Participant ToDomainEntity(FirestoreParticipant firestoreModel)
        {
            return firestoreModel.ToParticipant();
        }

        protected override FirestoreParticipant ToFirestoreModel(Participant entity)
        {
            return FirestoreParticipant.FromParticipant(entity);
        }

        protected override string KeyToString(string key) => key;

        protected override string StringToKey(string documentId) => documentId;


        public async Task<string> CreateAsync(Participant participant)
        {
             try
            {
                participant.CreatedAt = DateTime.UtcNow;
                var id = await AddAsync(participant);
                _logger.LogInformation("Created Participant {ParticipantId}", id);
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create participant {ParticipantEmail}", participant.EmailUser);
                throw;
            }

        }

        public async Task<Participant> GetByEmailUserAsync(string emailUser)
        {
            try
            {
                var participants = await FindAsync(query => query.WhereEqualTo("EmailUser", emailUser));
                var participant = participants.FirstOrDefault();
                return participant ?? throw new InvalidOperationException($"Participant with email {emailUser} not found");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get participant by email: {emailUser}", emailUser);
                throw;
            }
        }
    }
}
