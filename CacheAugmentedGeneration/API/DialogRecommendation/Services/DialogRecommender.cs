using API.DialogRecommendation.Interfaces;
using API.DialogRecommendation.Models;
using CAG.Interfaces;
using CAG.Services.Recommender;

namespace API.DialogRecommendation.Services
{
    public class DialogRecommender(IComputeSimilarity similarity) : GenericRecommender<Dialog>(similarity), IDialogRecommender
    {
    }
}
