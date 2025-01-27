using API.DialogRecommendation.Interfaces;
using API.DialogRecommendation.Models;
using CAG.Interfaces;
using CAG.Services.Recommender.Decorator;

namespace API.DialogRecommendation.Services.Decorator
{
    public class DialogRecommenderCacheDecorator(IGenericRecommender<Dialog> genericRecommender, ICacheService cacheService) : GenericRecommenderCacheDecorator<Dialog>(genericRecommender, cacheService), IDialogRecommender
    {
    }
}
