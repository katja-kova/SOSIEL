/// Name: AgentArchetype.cs
/// Description:
/// Authors: Multiple.
/// Last updated: July 10th, 2020.
/// Copyright: Garry Sotnik

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SOSIEL.Exceptions;

namespace SOSIEL.Entities
{
    public class AgentArchetype
    {
        public string NamePrefix { get; set; }

        public Dictionary<string, dynamic> CommonVariables { get; set; }

        public List<Goal> Goals { get; set; }

        public Dictionary<string, MentalModelConfiguration> MentalModel { get; set; }

        [JsonProperty]
        public List<DecisionOption> DecisionOptions { get; set; }


        public Dictionary<string, double> DoNothingAnticipatedInfluence { get; private set; }


        private List<MentalModel> mentalProto;

        public List<MentalModel> MentalProto
        {
            get { return mentalProto == null ? TransformDOToMentalModel() : mentalProto; }
        }

        public bool IsDataSetOriented { get; set; }

        public bool UseImportanceAdjusting { get; set; }

        public AgentArchetype()
        {
            CommonVariables = new Dictionary<string, dynamic>();
            Goals = new List<Goal>();
            MentalModel = new Dictionary<string, MentalModelConfiguration>();
            DecisionOptions = new List<DecisionOption>();
        }

        public dynamic this[string key]
        {
            get
            {
                if (CommonVariables.ContainsKey(key))
                    return CommonVariables[key];

                throw new UnknownVariableException(key);
            }
            set
            {
                CommonVariables[key] = value;
            }

        }

        /// <summary>
        /// Transforms from kh list to mental model
        /// </summary>
        /// <returns></returns>
        private List<MentalModel> TransformDOToMentalModel()
        {
            mentalProto = DecisionOptions.GroupBy(kh => kh.MentalModel).OrderBy(g => g.Key).Select(g =>
                   new MentalModel(g.Key, Goals.Where(goal => MentalModel[g.Key.ToString()].AssociatedWith.Contains(goal.Name)).ToArray(),
                       g.GroupBy(kh => kh.DecisionOptionsLayer).OrderBy(g2 => g2.Key).
                       Select(g2 => new DecisionOptionLayer(MentalModel[g.Key.ToString()].Layer[g2.Key.ToString()], g2)))).ToList();

            return mentalProto;
        }



        /// <summary>
        /// Adds decision option to mental model of current archetype if it isn't exists in the scope.
        /// </summary>
        /// <param name="newDecisionOption"></param>
        /// <param name="layer"></param>
        public void AddNewDecisionOption(DecisionOption newDecisionOption, DecisionOptionLayer layer)
        {
            if (mentalProto == null)
                TransformDOToMentalModel();

            layer.Add(newDecisionOption);

            DecisionOptions.Add(newDecisionOption);
        }


        /// <summary>
        /// Checks for similar decision options
        /// </summary>
        /// <param name="decisionOption"></param>
        /// <returns></returns>
        public bool IsSimilarDecisionOptionExists(DecisionOption decisionOption)
        {
            return DecisionOptions.Any(kh => kh == decisionOption);
        }
    }
}
